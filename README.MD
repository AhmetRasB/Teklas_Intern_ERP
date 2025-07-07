# Teklas Intern ERP

Modern .NET tabanlı monolitik ERP sistemi. Malzeme, üretim, depo, satın alma ve satış süreçlerini merkezi olarak yönetir.

## 🏗️ Mimari

**Backend:** .NET 8, Entity Framework Core, AutoMapper, FluentValidation  
**Frontend:** React, Bootstrap, DataTable  
**Veritabanı:** SQL Server  
**Cache:** Redis  

### Katmanlar
- **Entities** - Veritabanı modelleri
- **DataAccess** - Repository pattern, EF Core
- **Business** - İş mantığı, validasyon
- **API** - RESTful endpoints
- **UI** - React frontend

## 🚀 Kurulum

```bash
# Backend
dotnet restore
dotnet ef database update
dotnet run

# Frontend
cd Teklas_Intern_UI
npm install
npm start

# Redis (Docker)
docker run -p 6379:6379 redis
```

## 📋 Modüller

- **Material Management** - Malzeme kartları, kategoriler
- **Production Management** - BOM, iş emirleri
- **Warehouse Management** - Ambar, lokasyon, stok
- **Purchasing Management** - Satın alma siparişleri
- **Sales Management** - Müşteri siparişleri

## 🔧 Teknolojiler

- **.NET 8** - Backend framework
- **Entity Framework Core** - ORM
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Serilog** - Logging
- **Redis** - Caching
- **React** - Frontend
- **SQL Server** - Database

## 📁 Proje Yapısı

```
Teklas_Intern_ERP/
├── Teklas_Intern_ERP/          # API Layer
├── Teklas_Intern_ERP.Business/ # Business Logic
├── Teklas_Intern_ERP.DataAccess/ # Data Access
├── Teklas_Intern_ERP.Entities/ # Domain Models
├── Teklas_Intern_ERP.DTOs/     # Data Transfer Objects
└── Teklas_Intern_UI/           # React Frontend
```

## 🎯 Özellikler

- ✅ Clean Architecture
- ✅ SOLID Principles
- ✅ Repository Pattern
- ✅ Unit of Work Pattern
- ✅ Caching Strategy
- ✅ Exception Handling
- ✅ Input Validation
- ✅ Modern UI/UX