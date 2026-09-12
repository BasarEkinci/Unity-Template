# Syntac — Technical Rules

Unity 6 (`6000.6.0f1`), URP, .NET Standard 2.1, C# 9+. `Syntac` is a modular framework: each system is
its own assembly. This file is the source of truth for *how to work in the repo*; the existing code under
`Assets/Syntac/` is the source of truth for what each module contains.

## Layers

| Path | Layer | Rule |
|---|---|---|
| `Assets/Syntac/Systems/*` | Framework | Portable. Must not know about any specific game. Copyable to another project as-is. |
| `Assets/Syntac/Scripts/DI/Installers/*` | Game | The only place framework modules are composed. |
| `Assets/Resources/DI/ProjectLifetimeScope.prefab` | Invariant | Path is a string literal in `DIBootStrapper`. Never move or rename. |
| `Assets/Settings/_Dependencies/*` | Config assets | Referenced by installers via `[SerializeField]`, never by `Resources.Load`. |

## Hard rules

1. **One assembly per module.** A module may reference another only as already declared in that module's
   `.asmdef`. Adding a new cross-module reference is a design decision, not a style issue — raise it first.
2. **Naming**
   - private instance fields → `m_PascalCase`
   - private static fields → `s_PascalCase`
   - private/local constants → `k_PascalCase`
   - public constants → `PascalCase`
   - explicit types over `var` in first-party code
   - block-scoped `namespace { }`, never file-scoped
   - namespace mirrors the folder path below the module root
3. **Assembly references by name** (`"Syntac.Core"`), not by GUID — names survive re-import into a new
   project. The two exceptions, whose GUIDs are stable: `Unity.InputSystem`
   (`75469ad4d38634e559750d17036d5f7c`) and `VContainer` (`b0214a6008ed146ff8f122a6a9c2f6cc`).
4. **Do not invent systems.** Build exactly what is asked. Where an extension point exists, note it in the
   type's `<summary>` instead of implementing a speculative provider.
5. **Branding.** The framework identifies itself as `Syntac`. No other company/product/personal name in
   constants, menu paths, or comments.
6. **Vendored code** (`GenericEventBus`, `TargetedGenericEventBus`, the `Helpers/`) keeps its original
   style — do not reformat it to match rule 2.
7. **No comments.** Code explains itself; if it does not, fix the naming. Only a genuinely complex system
   may carry an XML `<summary>`.
8. **YAGNI.** Write only what was asked. No speculative feature, generalisation, or extension point.
9. **No hardcoded values** — no magic numbers, no magic strings. Use a named constant (rule 2 naming),
   an enum, or a serialized config field.
10. **Review after every plan implementation.** Once the plan is implemented, re-read the new code and
    refactor it toward KISS and YAGNI as far as the logic stays unchanged.

## Dependency injection (VContainer)

- Composition happens in installers only. `ProjectInstaller.FetchInstallers()` is the single root wiring
  point; registration order there *is* entry-point `Start()` order (`InputInstaller` first, pipes last).
- Installers that need asset references (`configs`, prefabs, mixers) derive from `MonoInstaller`; pure
  wiring uses a plain `IInstaller`.
- Services take dependencies through a `[Inject]` constructor. Do not resolve from the container inside a
  service, and do not add static service locators.
- Anything that must run at scope start implements `IStartable`; anything holding unmanaged/global state
  implements `IDisposable` so the scope tears it down.
- Editor requirement: **Enter Play Mode Options → Reload Domain must be enabled.** `DIBootStrapper` and
  `ProjectLifetimeScope` guard with static flags that otherwise survive between Play sessions.

## Event bus (`Syntac.MessagePipe`)

- Signals are `readonly struct` types implementing `ISignal` — the bus passes them by `ref`, so structs
  stay allocation-free.
- `Raise` defers while another event is being raised; `RaiseImmediately` re-enters. Prefer `Raise` unless
  ordering demands otherwise.
- Pipes are registered as singletons in a scope and never constructed with a free-floating `new` — the
  listener tables are static and keyed by bus instance, so an undisposed bus leaks its table.
- Every `SubscribeTo` needs a matching `UnsubscribeFrom` (delegate identity matters — store the handler in
  a field, never unsubscribe a freshly written lambda).

## Settings (`Syntac.SettingsSystem`)

- A setting derives from `SettingsBase<T>`: it owns its `Key`, its lazily evaluated default, its `Apply`,
  and its (de)serialization. Adding one costs a class plus a registration line — nothing else changes.
- `Load(string)` must parse and then call `Set(T)`, never write state directly, so apply/notify behave the
  same on the disk path and the UI path.
- Keys are unique; duplicates throw at container build time on purpose.
- Any numeric parse/serialize uses `CultureInfo.InvariantCulture`. A comma-decimal machine writing `0,5`
  and a dot-decimal machine failing to read it is the bug this prevents.

## Verifying changes

The Editor is normally open and holds the project lock, so batch mode cannot open the project. Drive the
running instance over the `unity` CLI (`C:\Users\basar\AppData\Local\Unity\bin\unity.exe`) instead:

```bash
unity status --json                               # confirm an instance is connected and ready
unity command eval --code 'UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport); return 1;'
unity command recompile                           # then poll `unity command recompile_status`
unity command console
```

`recompile` alone reports `up_to_date` when Unity has not imported new files yet — force the refresh
first. After a refresh or a Play-mode transition, poll `editor_status` until `compiling` and
`domainReloadInProgress` are both false; commands otherwise die with "Main thread operation timed out
after 5000ms".

## Definition of done

1. The project compiles: `unity command console` reports no errors after a refresh + recompile.
2. Play mode still boots — the DI root instantiates and the scene loads without errors.
3. No new cross-module assembly reference beyond what the `.asmdef` files already declare.
4. Naming rules hold in the new code.

## Commits

Conventional commits with a scope, matching the existing history:
`feat(installers):`, `config(project):`, `docs:`, `scene:`. No co-author trailers.
