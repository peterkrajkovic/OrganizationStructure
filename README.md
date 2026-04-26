# Organization Structure API

REST API pre správu organizačných štruktúr firiem s 4-úrovňovou hierarchiou: **Firma → Divízia → Projekt → Oddelenie**.

## 📋 Obsah

- [Technológie](#technológie)
- [Požiadavky](#požiadavky)
- [Inštalácia a Spustenie](#inštalácia-a-spustenie)
- [API Endpoints](#api-endpoints)
- [Testovanie](#testovanie)
- [Databáza](#databáza)
- [Architektúra](#architektúra)

---

## 🚀 Technológie

- **.NET 10** - Najnovší .NET framework
- **C# 14.0** - Moderný jazyk
- **Entity Framework Core 10** - ORM pre databázový prístup
- **SQL Server 2022 Express** - Databázový server
- **FluentValidation** - Validácia vstupných dát
- **Swagger/Scalar** - API dokumentácia
- **Repository Pattern** - Abstrakcia dátovej vrstvy
- **Service Layer** - Business logika

---

## 📦 Požiadavky

### Software
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2022 Express](https://www.microsoft.com/sk-sk/sql-server/sql-server-downloads) alebo SQL Server Management Studio (SSMS)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) alebo [VS Code](https://code.visualstudio.com/)

### NuGet Balíčky
Projekt už obsahuje potrebné balíčky:
- `Microsoft.EntityFrameworkCore.SqlServer` (10.0.0)
- `Microsoft.EntityFrameworkCore.Tools` (10.0.0)
- `FluentValidation.AspNetCore` (11.3.0)
- `Swashbuckle.AspNetCore` (najnovšia verzia)

---

## 🔧 Inštalácia a Spustenie

### 1. Klonovanie Projektu
git clone <repository-url> cd OrganizationStructure

### 2. Konfigurácia Databázy

Upravte `appsettings.json` podľa vášho SQL Servera:
{ "ConnectionStrings": { "DefaultConnection": "Server=localhost\SQLEXPRESS01;Database=OrganizationStructureDb;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;" } }

**Možné varianty:**
- SQL Server Express: `Server=localhost\\SQLEXPRESS;...`
- LocalDB: `Server=(localdb)\\MSSQLLocalDB;...`
- SQL Server s prihlásením: `Server=localhost;Database=...;User Id=sa;Password=YourPassword;...`

### 3. Vytvorenie Databázy
Nainštalujte EF Core tools (ak ešte nemáte)
dotnet tool install --global dotnet-ef --version 10.0.0
Vytvorte databázu pomocou migrácií
dotnet ef database update --project OrganizationStructure.Api
Alternatívne - použite SQL skript
sqlcmd -S localhost\SQLEXPRESS01 -E -i Database\InitialSetup.sql
Alternatívne - použite SQL skript
sqlcmd -S localhost\SQLEXPRESS01 -E -i Database\InitialSetup.sql

### 4. Spustenie Aplikácie
dotnet run --project OrganizationStructure.Api

Aplikácia bude dostupná na:
- **HTTPS:** https://localhost:5001
- **HTTP:** http://localhost:5248
- **Scalar UI:** https://localhost:5001/scalar/v1

---



## 🌐 API Endpoints

### Zamestnanci (Employees)
| Metóda | Endpoint | Popis |
|--------|----------|-------|
| GET | `/api/employees` | Získať všetkých zamestnancov |
| GET | `/api/employees/{id}` | Získať zamestnanca podľa ID |
| POST | `/api/employees` | Vytvoriť nového zamestnanca |
| PUT | `/api/employees/{id}` | Aktualizovať zamestnanca |
| DELETE | `/api/employees/{id}` | Vymazať zamestnanca |

### Firmy (Companies)
| Metóda | Endpoint | Popis |
|--------|----------|-------|
| GET | `/api/companies` | Získať všetky firmy |
| GET | `/api/companies/{id}` | Získať firmu podľa ID |
| POST | `/api/companies` | Vytvoriť novú firmu |
| PUT | `/api/companies/{id}` | Aktualizovať firmu |
| DELETE | `/api/companies/{id}` | Vymazať firmu |

### Divízie (Divisions)
| Metóda | Endpoint | Popis |
|--------|----------|-------|
| GET | `/api/divisions` | Získať všetky divízie |
| GET | `/api/divisions?companyId={id}` | Získať divízie firmy |
| GET | `/api/divisions/{id}` | Získať divíziu podľa ID |
| POST | `/api/divisions` | Vytvoriť novú divíziu |
| PUT | `/api/divisions/{id}` | Aktualizovať divíziu |
| DELETE | `/api/divisions/{id}` | Vymazať divíziu |

### Projekty (Projects)
| Metóda | Endpoint | Popis |
|--------|----------|-------|
| GET | `/api/projects` | Získať všetky projekty |
| GET | `/api/projects?divisionId={id}` | Získať projekty divízie |
| GET | `/api/projects/{id}` | Získať projekt podľa ID |
| POST | `/api/projects` | Vytvoriť nový projekt |
| PUT | `/api/projects/{id}` | Aktualizovať projekt |
| DELETE | `/api/projects/{id}` | Vymazať projekt |

### Oddelenia (Departments)
| Metóda | Endpoint | Popis |
|--------|----------|-------|
| GET | `/api/departments` | Získať všetky oddelenia |
| GET | `/api/departments?projectId={id}` | Získať oddelenia projektu |
| GET | `/api/departments/{id}` | Získať oddelenie podľa ID |
| POST | `/api/departments` | Vytvoriť nové oddelenie |
| PUT | `/api/departments/{id}` | Aktualizovať oddelenie |
| DELETE | `/api/departments/{id}` | Vymazať oddelenie |

## 🧪 Testovanie

### 1. Cez Scalar UI
Otvorte prehliadač a prejdite na:
https://localhost:5001/scalar/v1

### 2. Cez .http Súbor (Visual Studio)
Otvorte súbor `OrganizationStructure.Api.http` a spúšťajte požiadavky pomocou **Send Request**.

Príklad:
Vytvorte zamestnanca
POST https://localhost:5001/api/employees Content-Type: application/json
{ "title": "Ing.", "firstName": "Ján", "lastName": "Novák", "phone": "+421901234567", "email": "jan.novak@example.com" }

### 3. Cez cURL
Vytvorte zamestnanca
curl -X POST https://localhost:5001/api/employees 
-H "Content-Type: application/json" 
-d '{ "title": "Ing.", "firstName": "Peter", "lastName": "Horváth", "phone": "+421901111111", "email": "peter.horvath@example.com" }' 
--insecure

Získajte všetkých zamestnancov
curl https://localhost:5001/api/employees --insecure

---

## 💾 Databáza

### Dátový Model
─────────────┐ │  Employee   │──────────────┐ │             │              │ │ Id (PK)     │              │ │ Title       │              │ │ FirstName   │              │ │ LastName    │              ▼ │ Phone       │       ┌──────────────┐ │ Email (UQ)  │       │   Company    │ │ CreatedAt   │       │              │ │ UpdatedAt   │◄──────│ Id (PK)      │ └─────────────┘       │ Name         │ │ Code (UQ)    │ │ DirectorId   │ │ CreatedAt    │ │ UpdatedAt    │ └──────┬───────┘ │ ▼ ┌──────────────┐ │   Division   │ │              │ │ Id (PK)      │ │ Name         │ │ Code         │ │ CompanyId    │ │ ManagerId    │ │ CreatedAt    │ │ UpdatedAt    │ └──────┬───────┘ │ ▼ ┌──────────────┐ │   Project    │ │              │ │ Id (PK)      │ │ Name         │ │ Code         │ │ DivisionId   │ │ ManagerId    │ │ CreatedAt    │ │ UpdatedAt    │ └──────┬───────┘ │ ▼ ┌──────────────┐ │  Department  │ │              │ │ Id (PK)      │ │ Name         │ │ Code         │ │ ProjectId    │ │ ManagerId    │ │ CreatedAt    │ │ UpdatedAt    │ └──────────────┘


### Pravidlá
- ✅ **Unique Constraints:** Email zamestnancov, kódy firiem, kódy v rámci parent entity
- ✅ **Cascade Delete:** Vymazanie firmy vymaže divízie → projekty → oddelenia
- ✅ **Restrict Delete:** Nemôžete vymazať zamestnanca, ktorý je vedúci
- ✅ **Automatické Timestamps:** CreatedAt, UpdatedAt

### SQL Queries
-- Zobrazíte celú hierarchiu SELECT c.Name AS Company, d.Name AS Division, p.Name AS Project, dep.Name AS Department, e.FirstName + ' ' + e.LastName AS Manager FROM Companies c LEFT JOIN Divisions d ON c.Id = d.CompanyId LEFT JOIN Projects p ON d.Id = p.DivisionId LEFT JOIN Departments dep ON p.Id = dep.ProjectId LEFT JOIN Employees e ON dep.ManagerId = e.Id ORDER BY c.Name, d.Name, p.Name, dep.Name;
-- Počet entít SELECT 'Employees' AS Entity, COUNT() AS Count FROM Employees UNION ALL SELECT 'Companies', COUNT() FROM Companies UNION ALL SELECT 'Divisions', COUNT() FROM Divisions UNION ALL SELECT 'Projects', COUNT() FROM Projects UNION ALL SELECT 'Departments', COUNT(*) FROM Departments;

## 🏗️ Architektúra

### Clean Architecture Layers
┌────────────────────────────────────────┐ │         Controllers (API Layer)        │ │  - HTTP Endpoints                      │ │  - Request/Response Handling           │ └──────────────┬─────────────────────────┘ │ ▼ ┌────────────────────────────────────────┐ │      Services (Business Logic)        │ │  - Domain Rules                        │ │  - Validation                          │ │  - Orchestration                       │ └──────────────┬─────────────────────────┘ │ ▼ ┌────────────────────────────────────────┐ │     Repositories (Data Access)         │ │  - CRUD Operations                     │ │  - Query Building                      │ └──────────────┬─────────────────────────┘ │ ▼ ┌────────────────────────────────────────┐ │       Database (SQL Server)            │ │  - Data Persistence                    │ └────────────────────────────────────────┘

### Design Patterns
- **Repository Pattern** - Abstrakcia dátového prístupu
- **Service Layer Pattern** - Business logika oddelená od kontrolérov
- **DTO Pattern** - Separácia API kontraktov od entít
- **Dependency Injection** - Loose coupling, testovateľnosť
- **Generic Repository** - Reusable CRUD operácie

---

## ✅ Features

- ✅ **4-úrovňová hierarchia** organizačnej štruktúry
- ✅ **CRUD operácie** pre všetky entity
- ✅ **FluentValidation** - Validácia vstupných dát
- ✅ **Repository Pattern** - Clean data access
- ✅ **Service Layer** - Business logic separation
- ✅ **Cascade Delete** - Automatické mazanie závislostí
- ✅ **Referential Integrity** - Nemožno vymazať používaných zamestnancov
- ✅ **Unique Constraints** - Unikátne emaily a kódy
- ✅ **Structured Logging** - Logovanie chýb a operácií
- ✅ **Swagger Documentation** - Interaktívna API dokumentácia
- ✅ **Error Handling** - Správne HTTP status kódy a error messages
- ✅ **Async/Await** - Asynchronous operations

---

## 🔒 Validácie

### Employee
- Title: Povinný, max 50 znakov
- FirstName: Povinný, max 100 znakov
- LastName: Povinný, max 100 znakov
- Phone: Povinný, formát telefónneho čísla, max 20 znakov
- Email: Povinný, email formát, unikátny, max 255 znakov

### Company/Division/Project/Department
- Name: Povinný, max 200 znakov
- Code: Povinný, iba veľké písmená, čísla, pomlčky, podčiarkovníky, max 20 znakov, unikátny v rámci parent entity
- ManagerId/DirectorId: Povinný, musí existovať v databáze


