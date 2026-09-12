# Syntac Template

Unity 6 (`6000.6.0f1`) project template. URP, VContainer DI, modular assembly layout.
Each system lives in its own assembly and can be copied into another project as-is.

## Contents

```
Assets/
├── Resources/DI/ProjectLifetimeScope.prefab   DI root prefab (fixed path)
├── Settings/                                  URP assets, input, audio mixer
│   ├── PC_RPAsset / Mobile_RPAsset            two render pipeline profiles
│   ├── InputSchema.inputactions               Move, Look, Jump, Dash
│   ├── MainMixer.mixer                        Master / BGM / SFX / Voice / Ambience
│   └── _Dependencies/                         config assets referenced by installers
└── Syntac/
    ├── Scenes/                                BootScene, MainMenuScene, GameScene
    ├── Scripts/DI/Installers/                 game-specific wiring
    └── Systems/                               framework modules
```

## Systems

| Module | Assembly | Contents |
|---|---|---|
| Core | `Syntac.Core` | `MonoEntity`, `IBaseEntity`, `IPoolObject`, `ISelectable`, `LayerUtils`, `CfgParser`, `InputSchema`. |
| DI | `Syntac.DI` | `DIBootStrapper`, `ProjectLifetimeScope`, `SceneLifetimeScope`, `MonoInstaller`. The root scope is created no matter which scene Play starts from. |
| Signals | `Syntac.Signals` | `ISignal` marker interface. |
| MessagePipe | `Syntac.MessagePipe` | One event bus per scope: `ProjectPipe`, `BootPipe`, `MainMenuPipe`, `GamePipe`, plus their installers. |
| SettingsSystem | `Syntac.SettingsSystem` | Audio (Master, BGM, SFX, Voice, Ambience) and graphics (resolution, fullscreen, VSync, quality, target FPS) settings, saved as `settings.cfg` under `persistentDataPath`. |
| ConfigManagement | `Syntac.ConfigManagement` | Editor window listing every config asset implementing `IVisibleConfig`. |

## Third-party packages

| Package | Version |
|---|---|
| VContainer | 1.19.0 |
| UniTask | git (master) |
| LitMotion | git (master) |
| Input System | 1.20.0 |
| Universal RP | 17.6.0 |
| Test Framework | 1.8.0 |
| AI Assistant / Inference / Navigation | 2.19.0-pre.2 / 2.6.1 / 2.0.14 |

Coding rules and architecture details: [`CLAUDE.md`](CLAUDE.md).
