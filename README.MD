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

# Teklas Intern ERP - Backend Katmanları ve Mimari Açıklama

## Genel Bakış

Bu proje, bir ERP (Kurumsal Kaynak Planlama) sisteminin temel modüllerini içeren, .NET tabanlı, katmanlı monolitik mimariye sahip bir uygulamadır.

Amaç; malzeme, üretim, depo, satın alma ve satış sipariş süreçlerini yönetebilmektir.

Proje, aşağıdaki ana katmanlardan oluşur:

- Entities (Varlıklar)

- DataAccess (Veri Erişimi)

- Business (İş Mantığı)

- API (Sunum/Servis Katmanı)

Her katman, belirli bir sorumluluğa sahiptir ve kodun okunabilirliğini, sürdürülebilirliğini ve test edilebilirliğini artırmak için ayrılmıştır.

--- 
## 1. Entities Katmanı (Teklas_Intern_ERP.Entities)

### Görev

- Veritabanındaki tabloları temsil eden POCO (Plain Old CLR Object) sınıflarını içerir.

- Sadece veri taşır, iş mantığı veya veri erişim kodu içermez.

### Yapı

Her modülün (ör. Malzeme Yönetimi, Depo Yönetimi) kendi alt klasörü vardır.

Örnek dosyalar:

- MaterialCard.cs

- Warehouse.cs

- Supplier.cs

- WorkOrder.cs

### Örnek

```c#
public class MaterialCard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}
```
Bu sınıf, veritabanındaki MaterialCards tablosunu temsil eder.


---

## 2. DataAccess Katmanı (Teklas_Intern_ERP.DataAccess)

### Görev

- Veritabanı ile doğrudan iletişim kurar.

- Entity Framework Core kullanılarak, CRUD (Create, Read, Update, Delete) işlemlerini gerçekleştirir.

- Her entity için bir repository sınıfı bulunur.

- Veritabanı bağlantı ve konfigürasyonları burada yapılır.

### Ana Bileşenler

#### a) AppDbContext.cs

- Tüm entity’lerin DbSet olarak tanımlandığı, Entity Framework’ün ana context sınıfıdır.

- Veritabanı bağlantısı ve tabloların ilişkileri burada yönetilir

```c#
public class AppDbContext : DbContext

{

    public DbSet<MaterialCard> MaterialCards { get; set; }

   

}
```

#### b) Repository Sınıfları

- Her entity için bir repository bulunur.

- Temel CRUD işlemleri burada tanımlanır.

```c#
public class MaterialCardRepository

{

    private readonly AppDbContext _context;

    public List<MaterialCard> GetAll() => _context.MaterialCards.ToList();

    public MaterialCard GetById(int id) => _context.MaterialCards.Find(id);

    public MaterialCard Add(MaterialCard card) { ... }

    public bool Update(MaterialCard card) { ... }

    public bool Delete(int id) { ... }

}
```

#### c) Migrations

- Migrations klasörü, veritabanı şemasının kod üzerinden yönetilmesini sağlar.

- Migration dosyaları, tabloların nasıl oluşturulacağını ve güncelleneceğini belirtir.

#### d) appsettings.json

- Veritabanı bağlantı dizesi ve temel uygulama ayarları burada tutulur.
```json
"ConnectionStrings": {

  "DefaultConnection": "Server=...;Database=TeklasERP;Trusted_Connection=True;"

}
```

---
## 3. Business Katmanı (Teklas_Intern_ERP.Business)

### Görev

- Uygulamanın iş kurallarını ve süreçlerini barındırır.

- Repository’leri kullanarak, iş mantığına uygun işlemleri gerçekleştirir.

- Validasyon, kontrol ve iş süreçleri burada uygulanır.

### Yapı

Her modülün kendi manager sınıfı vardır:

- MaterialCardManager.cs

- SupplierManager.cs

- WorkOrderManager.cs

- vb.

```c#
public class MaterialCardManager
{
    private readonly MaterialCardRepository _repo;
    public List<MaterialCard> GetAll() => _repo.GetAll();
    public MaterialCard GetById(int id) => _repo.GetById(id);
    public MaterialCard Add(MaterialCard card) => _repo.Add(card);
    public bool Update(MaterialCard card) => _repo.Update(card);
    public bool Delete(int id) => _repo.Delete(id);
}
```

Manager sınıfları, repository’ler ile entity’ler arasında bir köprü görevi görür ve iş kurallarını uygular.

## 4. API Katmanı (Teklas_Intern_ERP)

### Görev

- Dış dünyadan (genellikle frontend’den) gelen HTTP isteklerini karşılar.

- Controller sınıfları, ilgili manager’ları kullanarak iş mantığını tetikler ve sonuçları döner.

- RESTful API prensiplerine uygun olarak çalışır.

### Yapı

Her modül için bir veya birden fazla controller bulunur:

- MaterialCardController.cs

- SupplierController.cs

- WorkOrderController.cs

- vb.

### Örnek

```c#
[ApiController]
[Route("api/[controller]")]
public class MaterialCardController : ControllerBase
{
    private readonly MaterialCardManager _manager;
    public MaterialCardController(AppDbContext context)
    {
        _manager = new MaterialCardManager(context);
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_manager.GetAll());

    [HttpPost]
    public IActionResult Add([FromBody] MaterialCard card)
    {
        var created = _manager.Add(card);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
```
Controller, HTTP isteklerini alır, ilgili manager’a iletir ve sonucu HTTP yanıtı olarak döner

 ---
 
###  5. Program.cs ve Uygulama Başlatma

- Program.cs dosyası, uygulamanın başlangıç noktasıdır.

- Servislerin (DbContext, Controller’lar, Swagger, CORS vb.) konfigürasyonu burada yapılır.

- Uygulama başlatılırken, ortam (development/production) ayarlarına göre Swagger ve diğer middleware’ler devreye alınır.
---
## 6. Katmanlar Arası İlişki ve Akış

1. Kullanıcı, frontend arayüzünden bir işlem başlatır (ör. malzeme ekle).

2. API Katmanı (Controller), HTTP isteğini alır.

3. Controller, ilgili Business Katmanı (Manager) metodunu çağırır.

4. Manager, gerekli iş kurallarını uygular ve DataAccess Katmanı (Repository) üzerinden veritabanı işlemini gerçekleştirir.

5. Entities Katmanındaki nesneler kullanılarak veri taşınır.

6. Sonuç, aynı yol üzerinden kullanıcıya döner.
---
## 7. Modül ve Dosya Yapısı

Her modül (ör. Malzeme Yönetimi, Depo Yönetimi) için aşağıdaki gibi bir yapı vardır:
```
Entities/
  MaterialManagement/
    MaterialCard.cs
    MaterialCategory.cs
    MaterialMovement.cs
DataAccess/
  MaterialManagement/
    MaterialCardRepository.cs
    MaterialCategoryRepository.cs
    MaterialMovementRepository.cs
Business/
  MaterialManagement/
    MaterialCardManager.cs
    MaterialCategoryManager.cs
    MaterialMovementManager.cs
Controllers/
  MaterialManagement/
    MaterialCardController.cs
    MaterialCategoryController.cs
    MaterialMovementController.cs
```

Bu yapı, diğer modüller (Üretim, Satın Alma, Depo, Satış Siparişi) için de benzer şekilde tekrarlanır.

---
## 8. Konfigürasyon ve Ortam Ayarları

- appsettings.json: Uygulamanın temel ayarları ve veritabanı bağlantı dizesi burada tutulur.

- appsettings.Development.json: Geliştirme ortamına özel ayarlar burada yer alır.

- Swagger/OpenAPI: Geliştirme ortamında API dokümantasyonu ve test arayüzü sağlar.

---

## 9. Migration ve Veritabanı Yönetimi

- Migrations klasörü, Entity Framework Core’un migration dosyalarını içerir.

- Migration dosyaları, veritabanı şemasının kod üzerinden yönetilmesini ve güncellenmesini sağlar.

- Örneğin, InitialCreate migration’ı, tüm ana tabloları oluşturur.

---

## 10. Güvenlik ve CORS

- CORS (Cross-Origin Resource Sharing) ayarları, farklı domainlerden gelen isteklerin kontrolünü sağlar.

- Geliştirme ortamında tüm domainlere izin verilmiş, prod ortamında kısıtlama önerilir.