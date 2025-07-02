### 1. **Material Management (Malzeme Yönetimi)**

- **Material Cards** → Malzeme Kartları
    
- **Material Categories** → Malzeme Sınıfları
    
- **Material Movements** → Malzeme Hareketleri
    

### 2.  **Production Management (Üretim Yönetimi)**

- **Bill of Materials (BOM)** → Ürün Ağaçları
    
- **Work Orders** → İş Emirleri
    
- **Production Confirmations** → Üretim Teyitleri
    

### 3.  **Warehouse Management (Depo / Ambar Yönetimi)**

- **Warehouse Definitions** → Ambar Tanımları
    
- **Location Management** → Lokasyon / Raf Tanımı
    
- **Stock Entries** → Sipariş Stok Girişleri
    

### 4.  **Purchasing Management (Satın Alma Yönetimi)**

- **Purchase Orders** → Satın Alma Siparişleri
    
- **Suppliers** → Tedarikçi Tanımları
    
- **Supplier Types** → Tedarikçi Tipleri
    

### 5. **Sales & Order Management (Satış ve Sipariş Yönetimi)**

- **Customer Orders** → Müşteri Siparişleri

----

# Teklas Intern ERP - Backend Katmanları ve Mimari Açıklama

## Genel Bakış

Bu proje, .NET tabanlı, katmanlı, monolitik bir ERP uygulamasıdır. Amaç; malzeme, üretim, depo, satın alma ve satış süreçlerini merkezi olarak yönetmektir. Proje, modern bir React tabanlı frontend ve RESTful API sunan bir backend içerir.

---

## Ana Modüller

- **Material Management (Malzeme Yönetimi):** Malzeme kartları, kategoriler, hareketler.
- **Production Management (Üretim Yönetimi):** Ürün ağaçları (BOM), iş emirleri, üretim teyitleri.
- **Warehouse Management (Depo Yönetimi):** Ambar, lokasyon, stok girişleri.
- **Purchasing Management (Satın Alma):** Satın alma siparişleri, tedarikçi ve tedarikçi tipleri.
- **Sales & Order Management (Satış/Sipariş):** Müşteri ve müşteri siparişleri.

---

## Katmanlı Mimari

### 1. **Entities Katmanı** (`Teklas_Intern_ERP.Entities`)
- **Amaç:** Veritabanı tablolarını temsil eden POCO sınıflar.
- **Ne içerir:** Sadece veri taşıyan property'ler (iş mantığı yok).
- **Örnek:** `MaterialCard.cs`, `Warehouse.cs`
- **Klasör Yapısı:** Her modülün kendi alt klasörü vardır.

### 2. **DataAccess Katmanı** (`Teklas_Intern_ERP.DataAccess`)
- **Amaç:** Veritabanı ile doğrudan iletişim (CRUD işlemleri).
- **Ne içerir:** 
  - `AppDbContext.cs`: EF Core context'i, DbSet'ler.
  - Repository'ler: Her entity için CRUD işlemleri.
  - DTO'lar ve Mapping profilleri (AutoMapper).
  - Migrations: Veritabanı şema yönetimi.
- **Klasör Yapısı:** Modül bazlı repository klasörleri, DTO ve Mapping klasörleri.

### 3. **Business Katmanı** (`Teklas_Intern_ERP.Business`)
- **Amaç:** İş kuralları ve süreç yönetimi.
- **Ne içerir:** 
  - Manager sınıfları: Her entity için iş mantığı.
  - Validasyon (FluentValidation).
  - Repository'leri kullanarak iş süreçlerini yönetir.
- **Klasör Yapısı:** Modül bazlı manager klasörleri.

### 4. **API Katmanı** (`Teklas_Intern_ERP/Controllers`)
- **Amaç:** RESTful API uç noktaları.
- **Ne içerir:** 
  - Controller'lar: Her modül için CRUD ve işlevsel endpoint'ler.
  - DTO ile veri alışverişi, validasyon, hata yönetimi.
- **Klasör Yapısı:** Modül bazlı controller klasörleri.

### 5. **Frontend (React UI)** (`Teklas_Intern_UI`)
- **Amaç:** Kullanıcı arayüzü.
- **Ne içerir:** 
  - React component'leri, sayfalar, layout'lar.
  - API ile haberleşme (axios/fetch).
  - Modern UI/UX, DataTable, modal, filtreleme, pagination.
- **Klasör Yapısı:** Modül bazlı component klasörleri.

---

## Temel Teknolojiler ve Kütüphaneler

- **Backend:** .NET 7/8, Entity Framework Core, AutoMapper, FluentValidation, Serilog, Redis (cache)
- **Frontend:** React, Axios, SweetAlert2, DataTable, Bootstrap, Iconify
- **Veritabanı:** SQL Server (EF Core ile)
- **Cache:** Redis (IDistributedCache)
- **Logging:** Serilog (console, file, Seq, MSSQL sink)
- **Mapping:** AutoMapper (DTO ↔ Entity)
- **Validasyon:** FluentValidation (API katmanında)

---

## Önemli Dosya ve Klasörler

- `Teklas_Intern_ERP.sln` : Çözüm dosyası.
- `appsettings.json` : Uygulama ve veritabanı ayarları.
- `Migrations/` : Veritabanı şema değişiklikleri.
- `Mapping/EntityMappingProfile.cs` : AutoMapper profilleri.
- `DTOs/` : Veri transfer objeleri.
- `Validators/` : FluentValidation kuralları.
- `Controllers/` : API uç noktaları.
- `Teklas_Intern_UI/` : React frontend projesi.

---

## Veri Akışı ve Katmanlar Arası İletişim

1. **Kullanıcı** React UI üzerinden bir işlem başlatır (ör. malzeme ekle).
2. **Frontend** API'ye istek atar (POST/PUT/GET/DELETE).
3. **Controller** isteği alır, validasyon yapar, manager'a iletir.
4. **Manager** iş kurallarını uygular, repository'den veri çeker/günceller.
5. **Repository** veritabanı ile iletişim kurar, gerekirse cache'e bakar.
6. **AutoMapper** ile DTO ↔ Entity dönüşümü yapılır.
7. **Sonuç** frontend'e döner.

---

## Redis Cache Entegrasyonu

### **Neden Redis?**
- Yüksek performanslı, RAM tabanlı, dağıtık cache.
- Uygulama restart olsa bile cache silinmez.
- Birden fazla instance ile çalışabilir.

### **Nasıl Çalışır?**
- Sık sorgulanan veriler (ör. lookup tabloları, sabit listeler) Redis'te tutulur.
- Veri önce cache'de aranır, yoksa veritabanından çekilip cache'e eklenir.
- Veri güncellenirse ilgili cache silinir veya güncellenir.

### **.NET'te Redis Kullanımı**
1. **NuGet Paketi:**  
   `Microsoft.Extensions.Caching.StackExchangeRedis`
2. **Startup/Program.cs:**  
   Redis cache servisi eklenir:
   ```csharp
   builder.Services.AddStackExchangeRedisCache(options =>
   {
       options.Configuration = "localhost:6379";
       options.InstanceName = "TeklasERP_";
   });
   ```
3. **Kullanım:**  
   `IDistributedCache` arayüzü ile veri ekle, oku, sil:
   ```csharp
   public class MaterialCardManager
   {
       private readonly IDistributedCache _cache;
       private readonly MaterialCardRepository _repo;
       public async Task<List<MaterialCard>> GetAllAsync()
       {
           var cacheKey = "MaterialCards_All";
           var cached = await _cache.GetStringAsync(cacheKey);
           if (cached != null)
               return JsonConvert.DeserializeObject<List<MaterialCard>>(cached);
           var data = await _repo.GetAllAsync();
           await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
           return data;
       }
   }
   ```
4. **Cache Invalidation:**  
   Veri güncellenirse/silinirse ilgili cache anahtarı silinir.

---

## Logging (Serilog)

- **Amaç:** Hata, bilgi, uyarı ve debug loglarını merkezi olarak toplamak.
- **Kullanım:** Console, dosya, Seq, MSSQL sink.
- **Yapılandırma:** `appsettings.json` veya kod üzerinden.

---

## Mapping (AutoMapper)

- **Amaç:** Entity ↔ DTO dönüşümlerini otomatikleştirmek.
- **Kullanım:** `EntityMappingProfile.cs` ile mapping kuralları tanımlanır.
- **Örnek:**  
  ```csharp
  CreateMap<MaterialCard, MaterialCardDto>().ReverseMap();
```

---

## Validasyon (FluentValidation)

- **Amaç:** API'ya gelen verinin doğruluğunu kontrol etmek.
- **Kullanım:** Validator sınıfları `Validators/` klasöründe tutulur, API katmanında kullanılır.

---

## Frontend (React)

- **Component'ler:** Her modül için ayrı component ve sayfa.
- **Tablolar:** DataTable benzeri, filtreleme, sıralama, pagination, sticky action column.
- **API ile iletişim:** Axios/fetch ile RESTful API.
- **UI/UX:** Modern, responsive, Türkçe dil desteği.

---

## Geliştirici Notları

- Kodun okunabilirliği ve sürdürülebilirliği için katmanlı mimari kullanıldı.
- Her katman sadece kendi sorumluluğundaki işleri yapar.
- Cache, mapping, validasyon gibi altyapı servisleri merkezi olarak yönetilir.
- Geliştirme sırasında loglar ve cache kullanımı ile performans ve hata takibi kolaylaştırıldı.

---

## Kurulum ve Çalıştırma

1. **Backend:**
   - `dotnet restore`
   - `dotnet ef database update`
   - `dotnet run`
2. **Frontend:**
   - `cd Teklas_Intern_UI`
   - `npm install`
   - `npm start`
3. **Redis:**
   - Redis sunucusunu başlat (örn. Docker: `docker run -p 6379:6379 redis`)
   - `appsettings.json` veya `Program.cs`'de Redis bağlantı ayarını yap.

---

## Sıkça Sorulanlar

- **Cache neden kullanılıyor?**  
  Performans ve ölçeklenebilirlik için.
- **Veri güncellenirse cache ne olur?**  
  İlgili cache anahtarı silinir veya güncellenir.
- **Frontend ile backend nasıl haberleşiyor?**  
  RESTful API üzerinden JSON ile.
- **Hangi alanlar cache'lenmeli?**  
  Sık sorgulanan, nadiren değişen veriler.

---

Daha fazla detay veya örnek kod için ilgili klasör ve dosyalara bakabilirsin.  
Her modül, katman ve teknoloji için açıklamalar README'de yer alıyor.  
Ekstra soruların olursa, belirli bir alanı daha da detaylandırabilirim!

# Detaylı Açıklamalar ve Teknolojiler

## Genel Bakış

Bu proje, .NET tabanlı, katmanlı, monolitik bir ERP uygulamasıdır. Amaç; malzeme, üretim, depo, satın alma ve satış süreçlerini merkezi olarak yönetmektir. Proje, modern bir React tabanlı frontend ve RESTful API sunan bir backend içerir.

---

## Ana Modüller

- **Material Management (Malzeme Yönetimi):** Malzeme kartları, kategoriler, hareketler.
- **Production Management (Üretim Yönetimi):** Ürün ağaçları (BOM), iş emirleri, üretim teyitleri.
- **Warehouse Management (Depo Yönetimi):** Ambar, lokasyon, stok girişleri.
- **Purchasing Management (Satın Alma):** Satın alma siparişleri, tedarikçi ve tedarikçi tipleri.
- **Sales & Order Management (Satış/Sipariş):** Müşteri ve müşteri siparişleri.

---

## Katmanlı Mimari

### 1. **Entities Katmanı** (`Teklas_Intern_ERP.Entities`)
- **Amaç:** Veritabanı tablolarını temsil eden POCO sınıflar.
- **Ne içerir:** Sadece veri taşıyan property'ler (iş mantığı yok).
- **Örnek:** `MaterialCard.cs`, `Warehouse.cs`
- **Klasör Yapısı:** Her modülün kendi alt klasörü vardır.

### 2. **DataAccess Katmanı** (`Teklas_Intern_ERP.DataAccess`)
- **Amaç:** Veritabanı ile doğrudan iletişim (CRUD işlemleri).
- **Ne içerir:** 
  - `AppDbContext.cs`: EF Core context'i, DbSet'ler.
  - Repository'ler: Her entity için CRUD işlemleri.
  - DTO'lar ve Mapping profilleri (AutoMapper).
  - Migrations: Veritabanı şema yönetimi.
- **Klasör Yapısı:** Modül bazlı repository klasörleri, DTO ve Mapping klasörleri.

### 3. **Business Katmanı** (`Teklas_Intern_ERP.Business`)
- **Amaç:** İş kuralları ve süreç yönetimi.
- **Ne içerir:** 
  - Manager sınıfları: Her entity için iş mantığı.
  - Validasyon (FluentValidation).
  - Repository'leri kullanarak iş süreçlerini yönetir.
- **Klasör Yapısı:** Modül bazlı manager klasörleri.

### 4. **API Katmanı** (`Teklas_Intern_ERP/Controllers`)
- **Amaç:** RESTful API uç noktaları.
- **Ne içerir:** 
  - Controller'lar: Her modül için CRUD ve işlevsel endpoint'ler.
  - DTO ile veri alışverişi, validasyon, hata yönetimi.
- **Klasör Yapısı:** Modül bazlı controller klasörleri.

### 5. **Frontend (React UI)** (`Teklas_Intern_UI`)
- **Amaç:** Kullanıcı arayüzü.
- **Ne içerir:** 
  - React component'leri, sayfalar, layout'lar.
  - API ile haberleşme (axios/fetch).
  - Modern UI/UX, DataTable, modal, filtreleme, pagination.
- **Klasör Yapısı:** Modül bazlı component klasörleri.

---

## Temel Teknolojiler ve Kütüphaneler

- **Backend:** .NET 7/8, Entity Framework Core, AutoMapper, FluentValidation, Serilog, Redis (cache)
- **Frontend:** React, Axios, SweetAlert2, DataTable, Bootstrap, Iconify
- **Veritabanı:** SQL Server (EF Core ile)
- **Cache:** Redis (IDistributedCache)
- **Logging:** Serilog (console, file, Seq, MSSQL sink)
- **Mapping:** AutoMapper (DTO ↔ Entity)
- **Validasyon:** FluentValidation (API katmanında)

---

## Önemli Dosya ve Klasörler

- `Teklas_Intern_ERP.sln` : Çözüm dosyası.
- `appsettings.json` : Uygulama ve veritabanı ayarları.
- `Migrations/` : Veritabanı şema değişiklikleri.
- `Mapping/EntityMappingProfile.cs` : AutoMapper profilleri.
- `DTOs/` : Veri transfer objeleri.
- `Validators/` : FluentValidation kuralları.
- `Controllers/` : API uç noktaları.
- `Teklas_Intern_UI/` : React frontend projesi.

---

## Veri Akışı ve Katmanlar Arası İletişim

1. **Kullanıcı** React UI üzerinden bir işlem başlatır (ör. malzeme ekle).
2. **Frontend** API'ye istek atar (POST/PUT/GET/DELETE).
3. **Controller** isteği alır, validasyon yapar, manager'a iletir.
4. **Manager** iş kurallarını uygular, repository'den veri çeker/günceller.
5. **Repository** veritabanı ile iletişim kurar, gerekirse cache'e bakar.
6. **AutoMapper** ile DTO ↔ Entity dönüşümü yapılır.
7. **Sonuç** frontend'e döner.

---

## Redis Cache Entegrasyonu

### **Neden Redis?**
- Yüksek performanslı, RAM tabanlı, dağıtık cache.
- Uygulama restart olsa bile cache silinmez.
- Birden fazla instance ile çalışabilir.

### **Nasıl Çalışır?**
- Sık sorgulanan veriler (ör. lookup tabloları, sabit listeler) Redis'te tutulur.
- Veri önce cache'de aranır, yoksa veritabanından çekilip cache'e eklenir.
- Veri güncellenirse ilgili cache silinir veya güncellenir.

### **.NET'te Redis Kullanımı**
1. **NuGet Paketi:**  
   `Microsoft.Extensions.Caching.StackExchangeRedis`
2. **Startup/Program.cs:**  
   Redis cache servisi eklenir:
   ```csharp
   builder.Services.AddStackExchangeRedisCache(options =>
   {
       options.Configuration = "localhost:6379";
       options.InstanceName = "TeklasERP_";
   });
   ```
3. **Kullanım:**  
   `IDistributedCache` arayüzü ile veri ekle, oku, sil:
   ```csharp
public class MaterialCardManager
{
       private readonly IDistributedCache _cache;
    private readonly MaterialCardRepository _repo;
       public async Task<List<MaterialCard>> GetAllAsync()
       {
           var cacheKey = "MaterialCards_All";
           var cached = await _cache.GetStringAsync(cacheKey);
           if (cached != null)
               return JsonConvert.DeserializeObject<List<MaterialCard>>(cached);
           var data = await _repo.GetAllAsync();
           await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
           return data;
    }
}
```
4. **Cache Invalidation:**  
   Veri güncellenirse/silinirse ilgili cache anahtarı silinir.

---

## Logging (Serilog)

- **Amaç:** Hata, bilgi, uyarı ve debug loglarını merkezi olarak toplamak.
- **Kullanım:** Console, dosya, Seq, MSSQL sink.
- **Yapılandırma:** `appsettings.json` veya kod üzerinden.

---

## Mapping (AutoMapper)

- **Amaç:** Entity ↔ DTO dönüşümlerini otomatikleştirmek.
- **Kullanım:** `EntityMappingProfile.cs` ile mapping kuralları tanımlanır.
- **Örnek:**  
  ```csharp
  CreateMap<MaterialCard, MaterialCardDto>().ReverseMap();
  ```

---

## Validasyon (FluentValidation)

- **Amaç:** API'ya gelen verinin doğruluğunu kontrol etmek.
- **Kullanım:** Validator sınıfları `Validators/` klasöründe tutulur, API katmanında kullanılır.

---

## Frontend (React)

- **Component'ler:** Her modül için ayrı component ve sayfa.
- **Tablolar:** DataTable benzeri, filtreleme, sıralama, pagination, sticky action column.
- **API ile iletişim:** Axios/fetch ile RESTful API.
- **UI/UX:** Modern, responsive, Türkçe dil desteği.

---

## Geliştirici Notları

- Kodun okunabilirliği ve sürdürülebilirliği için katmanlı mimari kullanıldı.
- Her katman sadece kendi sorumluluğundaki işleri yapar.
- Cache, mapping, validasyon gibi altyapı servisleri merkezi olarak yönetilir.
- Geliştirme sırasında loglar ve cache kullanımı ile performans ve hata takibi kolaylaştırıldı.

---

## Kurulum ve Çalıştırma

1. **Backend:**
   - `dotnet restore`
   - `dotnet ef database update`
   - `dotnet run`
2. **Frontend:**
   - `cd Teklas_Intern_UI`
   - `npm install`
   - `npm start`
3. **Redis:**
   - Redis sunucusunu başlat (örn. Docker: `docker run -p 6379:6379 redis`)
   - `appsettings.json` veya `Program.cs`'de Redis bağlantı ayarını yap.

---

## Sıkça Sorulanlar

- **Cache neden kullanılıyor?**  
  Performans ve ölçeklenebilirlik için.
- **Veri güncellenirse cache ne olur?**  
  İlgili cache anahtarı silinir veya güncellenir.
- **Frontend ile backend nasıl haberleşiyor?**  
  RESTful API üzerinden JSON ile.
- **Hangi alanlar cache'lenmeli?**  
  Sık sorgulanan, nadiren değişen veriler.

---

Daha fazla detay veya örnek kod için ilgili klasör ve dosyalara bakabilirsin.  
Her modül, katman ve teknoloji için açıklamalar README'de yer alıyor.  
Ekstra soruların olursa, belirli bir alanı daha da detaylandırabilirim!