---
description: Mevcut değişiklikleri kategorilere ayırır, commit mesajları üretir ve sıralı bir tablo halinde listeler
allowed-tools: Bash(git status:*), Bash(git diff:*), Bash(git log:*), Bash(git ls-files:*), Read, Glob, Grep
---

# analyze-changes

Çalışma ağacındaki mevcut değişiklikleri analiz et ve commit'e hazır bir plan çıkar.

## Bağlam toplama

Aşağıdaki komutları çalıştırarak mevcut durumu topla:

- `git status --porcelain=v1 -uall` — staged / unstaged / untracked tüm dosyalar
- `git diff --stat` ve `git diff --staged --stat` — değişim büyüklüğü
- `git diff` ve `git diff --staged` — asıl içerik (büyükse dosya bazında incele)
- `git log --oneline -15` — projedeki mevcut commit mesajı stilini yakala

Untracked dosyaların içeriğini gerektiği kadar oku. Unity projesi olduğunu unutma:
`.meta` dosyaları ait oldukları asset/klasörle **aynı** commit'e girer, ayrı bir kategori
oluşturmaz. `Library/`, `Temp/`, `Logs/`, `Build/` gibi ignore edilmiş yollar analize dahil edilmez.

## Kategorilere ayırma

Değişiklikleri mantıksal birimlere ayır. Kategori, dosya klasörüne göre değil **amaca** göre belirlenir:
birlikte anlam ifade eden ve birlikte geri alınabilecek değişiklikler aynı gruba girer.

Tipik kategoriler (uyanları kullan, zorlama):

| Kategori | Kapsam |
|---|---|
| `feat` | Yeni özellik, yeni sistem, yeni gameplay mekaniği |
| `fix` | Hata düzeltmesi |
| `refactor` | Davranışı değiştirmeyen yeniden yapılandırma |
| `perf` | Performans iyileştirmesi |
| `assets` | Model, texture, audio, prefab, materyal, animasyon |
| `scene` | Sahne (`.unity`) değişiklikleri |
| `config` | ProjectSettings, Packages/manifest.json, .gitignore, editor ayarları |
| `ui` | UI prefab / layout / stil değişiklikleri |
| `test` | Test ekleme veya güncelleme |
| `chore` | Bakım, temizlik, dosya taşıma |
| `docs` | Dokümantasyon |

Bir dosya birden fazla amaca hizmet ediyorsa baskın amacı seç ve bunu notlarda belirt.

## Commit mesajı üretimi

Her kategori için Conventional Commits formatında tek satırlık bir başlık üret:

```
<tip>(<kapsam>): <ne yapıldığı, emir kipinde, küçük harfle>
```

- Türkçe değil **İngilizce** yaz — mevcut `git log` farklı bir dil/stil gösteriyorsa ona uy.
- 72 karakteri geçme, sonuna nokta koyma.
- "ne değişti" değil "ne sağlandı" anlat: `fix(player): prevent double jump on slope` gibi.
- Gerekiyorsa 1-3 satırlık bir gövde öner (neden / etki), ama zorunlu değil.

## Çıktı

Önce kısa bir özet cümlesi (kaç dosya, kaç önerilen commit), ardından **tek bir tablo**.
Başka tablo, alt liste veya grup grup döküm yazma — okunması zor oluyor.
Tablo tam olarak şu üç sütundan oluşur:

| Numara | Kategori | Mesaj |
|---|---|---|
| 1 | chore(docs) | update docs |
| 2 | config(input) | add input system action asset |

- `Numara`: önerilen commit sırası, 1'den başlar — `/commit` bu numaraları kullanır.
- `Kategori`: commit mesajının `<tip>(<kapsam>)` kısmı.
- `Mesaj`: commit mesajının `:` sonrası kalan açıklama kısmı.

Tablo sırası = önerilen commit sırası ve şu mantığa göre kurulur:

1. Config / bağımlılık / proje ayarları (diğerlerinin çalışması buna bağlı olabilir)
2. Assets ve import edilen kaynaklar
3. Kod: refactor → feat → fix
4. Scene ve prefab bağlamaları (kod ve asset'lere referans verdikleri için sonra)
5. Test
6. Docs / chore

Hangi dosyanın hangi gruba ait olduğunu tabloda **yazma**; bu eşleşmeyi kendi
analizinde tut, `/commit` aynı oturumda oradan kullanır. Kullanıcı açıkça isterse
dosya dökümünü ayrıca ver.

Tablodan sonra yalnızca gerekiyorsa 1-3 satır uyarı ekle: yanlışlıkla eklenmiş dosya,
sızmış secret, büyük binary, eksik `.meta`, birleştirilebilecek gruplar gibi.

**Bu komut hiçbir şey commit etmez ve `git add` çalıştırmaz.** Sadece analiz üretir.
Sonunda kullanıcıya `/commit` ile devam edebileceğini hatırlat.
