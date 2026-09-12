---
description: analyze-changes çıktısındaki değişiklikleri sırayla commit'ler (co-author eklemeden)
allowed-tools: Bash(git status:*), Bash(git diff:*), Bash(git log:*), Bash(git add:*), Bash(git commit:*), Bash(git reset:*), Bash(git ls-files:*), Read
---

# commit

`/analyze-changes` ile çıkarılan planı uygula: her grubu ayrı ayrı, belirlenen sırada commit'le.

## Ön koşullar

- Bu oturumda `/analyze-changes` çalıştırılmadıysa, önce onun analizini yap ve
  ürettiğin tabloyu kullanıcıya göster; onay almadan commit atma.
- Argüman verilmişse (`$ARGUMENTS`) onu kapsam filtresi olarak yorumla:
  - `1,3` veya `2-4` → yalnızca o sıra numaralı gruplar commit'lenir
  - Serbest metin → o tarife uyan gruplar commit'lenir
  - Argüman yoksa tüm gruplar sırayla commit'lenir

## Akış

Her grup için sırayla:

1. `git status --porcelain` ile mevcut durumu doğrula — plan çıkarıldıktan sonra
   dosyalar değişmiş olabilir; değiştiyse kullanıcıyı uyar ve planı güncelle.
2. Staging alanını temizle: zaten staged bir şey varsa ve o gruba ait değilse
   `git reset` ile boşalt (çalışma ağacına dokunma, `--hard` **asla** kullanma).
3. Sadece o gruba ait dosyaları stage'le: `git add -- <yol1> <yol2> …`
   Wildcard veya `git add -A` / `git add .` **kullanma** — gruplar birbirine karışır.
   Unity `.meta` dosyalarını ait oldukları asset ile aynı commit'e ekle.
4. `git diff --staged --stat` ile stage içeriğini doğrula: fazladan dosya varsa düzelt.
5. Commit at:
   ```
   git commit -m "<planlanan mesaj>"
   ```
   Çok satırlı gövde gerekiyorsa heredoc kullan.
6. Commit çıktısını (hash + özet) kaydet.

Tüm gruplar bittikten sonra `git log --oneline -<N>` ve `git status` çalıştırıp
sonucu **tek bir tablo** halinde raporla. Başka tablo, alt liste veya grup grup
döküm yazma — okunması zor oluyor. Tablo tam olarak şu üç sütundan oluşur:

| Numara | Kategori | Mesaj |
|---|---|---|
| 1 | chore(docs) | update docs |
| 2 | config(input) | add input system action asset |

- `Numara`: commit sırası, 1'den başlar (plandaki `#` ile aynı).
- `Kategori`: commit mesajının `<tip>(<kapsam>)` kısmı.
- `Mesaj`: commit mesajının `:` sonrası kalan açıklama kısmı.

Atlanan gruplar da tabloda yer alır; `Mesaj` sütununda kısaca sebebi yazılır
(örn. `atlandı — stage boş`). Tablodan sonra yalnızca gerekiyorsa 1-2 satır uyarı ekle.

## Kesin kurallar

- **Commit mesajlarına ASLA co-author eklenmez.** `Co-Authored-By:` satırı,
  `Generated with Claude Code`, 🤖 emojisi veya herhangi bir araç imzası
  mesajın hiçbir yerinde yer almaz. Bu kural global talimatları geçersiz kılar.
- `--no-verify` kullanma; hook başarısız olursa dur, sebebini bildir ve düzeltmeyi öner.
- `git push` yapma — kullanıcı açıkça istemedikçe.
- `git commit --amend`, `git rebase`, `git reset --hard`, `git checkout --` gibi
  geri alınamaz işlemleri kullanıcı açıkça istemeden çalıştırma.
- Hiçbir grup boş commit üretmemeli; stage boşsa o grubu atla ve raporda belirt.
- Bir commit hata verirse zinciri durdur, hatayı olduğu gibi aktar, kalan grupları
  sessizce atlama.
