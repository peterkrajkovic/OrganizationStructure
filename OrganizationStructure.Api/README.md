# Organization Structure API

> REST API pre správu organizačných štruktúr firiem s 4-úrovňovou hierarchiou.

**Firma → Divízia → Projekt → Oddelenie**

---

## Obsah

- [Technológie](#technológie)
- [Požiadavky](#požiadavky)
- [Inštalácia a spustenie](#inštalácia-a-spustenie)
- [API Endpoints](#api-endpoints)
- [Testovanie](#testovanie)
- [Databáza](#databáza)
- [Validácie](#validácie)
- [Features](#features)

---

## Technológie

| Technológia | Verzia | Účel |
|---|---|---|
| .NET | 10 | Najnovší .NET framework |
| C# | 14.0 | Moderný jazyk |
| Entity Framework Core | 10 | ORM pre databázový prístup |
| SQL Server | 2022 Express | Databázový server |
| FluentValidation | 11.3.0 | Validácia vstupných dát |
| Scalar | najnovšia | Interaktívna API dokumentácia |
| Repository Pattern | — | Abstrakcia dátovej vrstvy |
| Service Layer | — | Separácia business logiky |

---

## Požiadavky

### Software

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2022 Express](https://www.microsoft.com/sk-sk/sql-server/sql-server-downloads)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) alebo [VS Code](https://code.visualstudio.com/)

### NuGet balíčky

- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.0
- `Microsoft.EntityFrameworkCore.Tools` 10.0.0
- `FluentValidation.AspNetCore` 11.3.0
- `Scalar.AspNetCore` (najnovšia verzia)

---

## Inštalácia a spustenie

### 1. Klonovanie projektu

```bash
git clone <repository-url>
cd OrganizationStructure
```

### 2. Konfigurácia databázy

Upravte `appsettings.json` podľa vášho SQL Servera:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=OrganizationStructureDb;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
  }
}
```

**Možné varianty connection stringu:**

| Typ | Connection String |
|---|---|
| SQL Server Express | `Server=localhost\\SQLEXPRESS;...` |
| LocalDB | `Server=(localdb)\\MSSQLLocalDB;...` |
| SQL Server s prihlásením | `Server=localhost;User Id=sa;Password=...;...` |

### 3. Vytvorenie databázy

```bash
# Inštalácia EF Core tools
dotnet tool install --global dotnet-ef --version 10.0.0

# Vytvorenie databázy cez migrácie
dotnet ef database update --project OrganizationStructure.Api

# Alternatíva — manuálny SQL skript
sqlcmd -S localhost\SQLEXPRESS01 -E -i Database\InitialSetup.sql
```

### 4. Spustenie aplikácie

```bash
dotnet run --project OrganizationStructure.Api
```

| Rozhranie | URL |
|---|---|
| HTTPS | `https://localhost:5001` |
| HTTP | `http://localhost:5000` |
| Scalar UI | `https://localhost:5001/scalar/v1` |

---

## API Endpoints

### Zamestnanci — `/api/employees`

| Metóda | Endpoint | Popis |
|---|---|---|
| `GET` | `/api/employees` | Získať všetkých zamestnancov |
| `GET` | `/api/employees/{id}` | Získať zamestnanca podľa ID |
| `POST` | `/api/employees` | Vytvoriť nového zamestnanca |
| `PUT` | `/api/employees/{id}` | Aktualizovať zamestnanca |
| `DELETE` | `/api/employees/{id}` | Vymazať zamestnanca |

### Firmy — `/api/companies`

| Metóda | Endpoint | Popis |
|---|---|---|
| `GET` | `/api/companies` | Získať všetky firmy |
| `GET` | `/api/companies/{id}` | Získať firmu podľa ID |
| `POST` | `/api/companies` | Vytvoriť novú firmu |
| `PUT` | `/api/companies/{id}` | Aktualizovať firmu |
| `DELETE` | `/api/companies/{id}` | Vymazať firmu |

### Divízie — `/api/divisions`

| Metóda | Endpoint | Popis |
|---|---|---|
| `GET` | `/api/divisions` | Získať všetky divízie |
| `GET` | `/api/divisions?companyId={id}` | Získať divízie firmy |
| `GET` | `/api/divisions/{id}` | Získať divíziu podľa ID |
| `POST` | `/api/divisions` | Vytvoriť novú divíziu |
| `PUT` | `/api/divisions/{id}` | Aktualizovať divíziu |
| `DELETE` | `/api/divisions/{id}` | Vymazať divíziu |

### Projekty — `/api/projects`

| Metóda | Endpoint | Popis |
|---|---|---|
| `GET` | `/api/projects` | Získať všetky projekty |
| `GET` | `/api/projects?divisionId={id}` | Získať projekty divízie |
| `GET` | `/api/projects/{id}` | Získať projekt podľa ID |
| `POST` | `/api/projects` | Vytvoriť nový projekt |
| `PUT` | `/api/projects/{id}` | Aktualizovať projekt |
| `DELETE` | `/api/projects/{id}` | Vymazať projekt |

### Oddelenia — `/api/departments`

| Metóda | Endpoint | Popis |
|---|---|---|
| `GET` | `/api/departments` | Získať všetky oddelenia |
| `GET` | `/api/departments?projectId={id}` | Získať oddelenia projektu |
| `GET` | `/api/departments/{id}` | Získať oddelenie podľa ID |
| `POST` | `/api/departments` | Vytvoriť nové oddelenie |
| `PUT` | `/api/departments/{id}` | Aktualizovať oddelenie |
| `DELETE` | `/api/departments/{id}` | Vymazať oddelenie |

---

## Testovanie

### Scalar UI

```
https://localhost:5001/scalar/v1
```

### HTTP súbor (Visual Studio)

Otvorte `OrganizationStructure.Api.http` a spúšťajte požiadavky pomocou **Send Request**.

### cURL

```bash
# Vytvoriť zamestnanca
curl -X POST https://localhost:5001/api/employees \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Ing.",
    "firstName": "Ján",
    "lastName": "Novák",
    "phone": "+421901234567",
    "email": "jan.novak@example.com"
  }' \
  --insecure

# Získať všetkých zamestnancov
curl https://localhost:5001/api/employees --insecure
```

---

## Databáza



### Pravidlá integrity

| Pravidlo | Popis |
|---|---|
| Unique Constraints | Email zamestnancov, kódy firiem, kódy v rámci parent entity |
| Cascade Delete | Vymazanie firmy vymaže divízie → projekty → oddelenia |
| Restrict Delete | Zamestnanec, ktorý je vedúci, sa nedá vymazať |
| Automatické timestamps | `CreatedAt` a `UpdatedAt` na každej entite |

### Užitočné SQL queries

```sql
-- Celá hierarchia
SELECT
  c.Name  AS Company,
  d.Name  AS Division,
  p.Name  AS Project,
  dep.Name AS Department,
  e.FirstName + ' ' + e.LastName AS Manager
FROM Companies c
LEFT JOIN Divisions   d   ON c.Id  = d.CompanyId
LEFT JOIN Projects    p   ON d.Id  = p.DivisionId
LEFT JOIN Departments dep ON p.Id  = dep.ProjectId
LEFT JOIN Employees   e   ON dep.ManagerId = e.Id
ORDER BY c.Name, d.Name, p.Name, dep.Name;

-- Počet entít
SELECT 'Employees'   AS Entity, COUNT(*) AS Count FROM Employees
UNION ALL
SELECT 'Companies',  COUNT(*) FROM Companies
UNION ALL
SELECT 'Divisions',  COUNT(*) FROM Divisions
UNION ALL
SELECT 'Projects',   COUNT(*) FROM Projects
UNION ALL
SELECT 'Departments',COUNT(*) FROM Departments;
```

---


### Design patterns

| Pattern | Účel |
|---|---|
| Repository Pattern | Abstrakcia dátového prístupu |
| Service Layer Pattern | Business logika oddelená od kontrolérov |
| DTO Pattern | Separácia API kontraktov od entít |
| Dependency Injection | Loose coupling, testovateľnosť |
| Generic Repository | Reusable CRUD operácie |

---

## Validácie

### Employee

| Pole | Pravidlo |
|---|---|
| `Title` | Povinný, max 50 znakov |
| `FirstName` | Povinný, max 100 znakov |
| `LastName` | Povinný, max 100 znakov |
| `Phone` | Povinný, formát telefónneho čísla, max 20 znakov |
| `Email` | Povinný, email formát, unikátny, max 255 znakov |

### Company / Division / Project / Department

| Pole | Pravidlo |
|---|---|
| `Name` | Povinný, max 200 znakov |
| `Code` | Povinný, iba veľké písmená / čísla / pomlčky / podčiarkovníky, max 20 znakov, unikátny v rámci parent entity |
| `ManagerId` / `DirectorId` | Povinný, musí existovať v databáze |

---

## Features

- 4-úrovňová hierarchia organizačnej štruktúry
- CRUD operácie pre všetky entity
- FluentValidation — validácia vstupných dát
- Repository Pattern — čistý dátový prístup
- Service Layer — business logika mimo kontrolérov
- Cascade Delete — automatické mazanie závislostí
- Referential Integrity — zamestnanec používaný ako vedúci sa nedá vymazať
- Unique Constraints — unikátne emaily a kódy
- Structured Logging — logovanie chýb a operácií
- Scalar — interaktívna API dokumentácia
- Error Handling — správne HTTP status kódy a chybové správy
- Async/Await — asynchrónne operácie