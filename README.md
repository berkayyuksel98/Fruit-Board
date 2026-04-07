# Fruit Board

Monopoly GO tarzından ilham alınarak yapılmış, 3D tek oyunculu bir board game prototipi. Unity 6, URP.

---

## Nasıl Çalışır

Oyuncu zar atar, çıkan sayı kadar tile ilerler. Her tile'da ödül olabilir, olursa envanteri güncellenir.

---

## Sistemler

### Board

Tile'lar oyun başında JSON dosyasından yüklenir. Dosya yoksa ScriptableObject'teki veriye fallback yapar. Her tile'ın ödül tipi ve miktarı bu veride tutulur. Tile üzerinde hem yazı hem ikon var, ikon ItemDataSO'dan çekiliyor.

Karakter bir tile'a bastığında tile küçük bir scale animasyonu oynatır. Highlight, karakterin yere değdiği anda tetiklenir.

### Zar Sistemi

Zarlar fizik simülasyonu ile Unity Recorder'da kaydedildi. 3 farklı atış animasyonu var. Oyun içinde bu kayıtlar Animator üzerinden oynatılıyor, gerçek fizik yok. Hangi yüzün üstte kalacağını biz seçiyoruz: animasyonun doğal olarak hangi yüzü üste bıraktığını Inspector'a giriyorsunuz, sistem aradaki farkı child objeye uygulayarak istenen yüzü gösteriyor.

Atış sırasında her zar önce konumlanır (hepsi aynı anda), sonra animasyon başlar.

Animasyon bittikten sonra zarlar bir curve ile scale 0'a küçülüp devre dışı kalıyor. Tekrar atılacağında scale'i 1'e getirip aktif ediliyor.

### Envanter

Elma, armut, çilek. Her tile'da bunlardan biri (veya hiçbiri) ödül olarak tanımlanmış. Oyuncu tile'a bastığında ödül varsa envantere ekleniyor. Veriler PlayerPrefs'e kaydediliyor, oyun kapanıp açılınca korunuyor.

Ödül geldiğinde UI'daki ilgili item bir kez büyüyüp küçülüyor.

### Karakter Hareketi

Adım adım ilerliyor, her tile için ayrı bir zıplama. Arc Mathf.Sin ile hesaplanıyor. Yere değdiği anda tile highlight tetikleniyor ve particle effect çalışıyor.

Karakter hareket etmeye başlamadan önce zarlar sahneden küçülüp kayboluyor.

### UI

Üstte sağda envanter görünüyor. Üst solda ise zar sayısı seçimi ve her zar için değer girişi var. Ayrica kaç zar atmak istediğimizi Dropdown menu ile seçebiliyoruz. Roll butonu zarları atar.

Atış sırasında butonlar kilitlenir, karakter hedefe ulaşınca tekrar açılır.

### EventBus

Sistemler birbirini doğrudan çağırmıyor. Her şey event'ler üzerinden haberleşiyor. Örneğin zar atıldığında bir event gidiyor, DiceView onu dinleyip animasyonu başlatıyor, animasyon bitince başka bir event gidiyor, BoardController onu dinleyip hareketi tetikliyor gibi.

---

## Inspector Kurulumu (Dice)

Her clip'in doğal olarak hangi yüzü üste bıraktığını `Clip End Faces`'e yazıyorsunuz.

`Landing Target` zarın düşeceği alanın merkezi. `Landing Radius` ne kadar dağılacağını belirliyor.

---

## Klasör Yapısı

```
Assets/_Project/Scripts/
  Board/          - Tile modeli, controller, view
  Dice/           - Zar modeli, controller, view, fizik sistemi
  Inventory/      - Envanter modeli, controller, view, item view
  Player/         - Oyuncu modeli, controller, view
  UI/             - Zar input arayüzü
  Core/           - EventBus, GameInstaller, event tanımları
  Persistence/    - PlayerPrefs kayıt sistemi
  ScriptableObjects/ - Config ve data SO'ları
```
