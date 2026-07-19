# CommissionApp – Uruchamianie projektu (.NET 8 + EF Core 9)

Aplikacja konsolowa oparta o .NET 8 oraz Entity Framework Core 9, wykorzystująca SQL Server jako bazę danych.

---

## 🔧 Wymagania

### ✅ Wspólne:
- .NET SDK 8.0+
- EF Core CLI (`dotnet tool install --global dotnet-ef`)
- Git

---

## 🐧 Debian / Linux – Instrukcja krok po kroku

### 1. Instalacja zależności

```bash
sudo apt update
sudo apt install -y dotnet-sdk-8.0 git docker.io
dotnet tool install --global dotnet-ef
```

### 2. Uruchomienie SQL Server w Dockerze

```bash
Instalacja Dockera:

sudo apt update
sudo apt install docker.io
sudo systemctl enable docker
sudo systemctl start docker

SQL server w Dockerze:

sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Pass123' \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Klonowanie repozytorium i uruchomienie

```bash
git clone https://github.com/gregpec/CommissionSQLApp
cd repo/CommissionApp
dotnet build
dotnet ef database update
dotnet run
```

---

## 🪟 Windows – Instrukcja krok po kroku

# Instalacja i uruchomienie (Windows)

## Wymagania

Przed uruchomieniem projektu zainstaluj:

- Windows 10 lub Windows 11
- .NET SDK 8.0
- Visual Studio 2022 (lub nowszy)
- SQL Server Express 2022
- SQL Server Management Studio (SSMS)

---

## 1. Sklonowanie repozytorium

```powershell
git clone https://github.com/gregpec/CommissionSQLApp.git
cd CommissionSQLApp
```

---

## 2. Sprawdzenie wersji .NET

```powershell
dotnet --version
```

Powinna zostać wyświetlona wersja 8.x.

---

## 3. Instalacja Entity Framework Core CLI

Sprawdzenie instalacji:

```powershell
dotnet ef --version
```

Jeżeli polecenie nie działa:

```powershell
dotnet tool install --global dotnet-ef
```

Aktualizacja:

```powershell
dotnet tool update --global dotnet-ef
```

---

## 4. Instalacja SQL Server Express

Zainstaluj:

- SQL Server Express 2022
- SQL Server Management Studio (SSMS)

---

## 5. Włączenie logowania SQL Server

Uruchom **SQL Server Management Studio**.

Połącz się z serwerem:

```
.\SQLEXPRESS
```

Uwierzytelnianie:

```
Windows Authentication
```

Następnie:

```
Server
→ Properties
→ Security
```

Wybierz:

```
SQL Server and Windows Authentication mode
```

Kliknij:

```
OK
```

---

## 6. Włączenie konta sa

Przejdź do:

```
Security
→ Logins
→ sa
```

Kliknij:

```
Properties
```

### General

Ustaw hasło:

```
YourStrong!Pass123
```

### Status

Ustaw:

```
Permission to connect to database engine
Grant

Login
Enabled
```

Kliknij **OK**.

---

## 7. Włączenie TCP/IP

Uruchom:

```
SQL Server Configuration Manager
```

Przejdź do:

```
SQL Server Network Configuration
→ Protocols for SQLEXPRESS
```

Kliknij prawym przyciskiem:

```
TCP/IP
```

i wybierz:

```
Enable
```

---

## 8. Ustawienie portu 1433

Kliknij dwukrotnie:

```
TCP/IP
```

Przejdź do zakładki:

```
IP Addresses
```

Przewiń na sam dół do sekcji:

```
IPAll
```

Ustaw:

```
TCP Dynamic Ports

(puste)
```

```
TCP Port

1433
```

Kliknij **OK**.

---

## 9. Restart SQL Server

Przejdź do:

```
SQL Server Services
```

Kliknij prawym przyciskiem:

```
SQL Server (SQLEXPRESS)
```

i wybierz:

```
Restart
```

---

## 10. Sprawdzenie połączenia

Uruchom PowerShell:

```powershell
Test-NetConnection localhost -Port 1433
```

Oczekiwany wynik:

```
TcpTestSucceeded : True
```

---

## 11. Connection String

Projekt wykorzystuje następujący connection string:

```csharp
.UseSqlServer(
    "Server=localhost,1433;Database=CarsStorage;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True");
```

---

## 12. Utworzenie bazy danych

W katalogu projektu wykonaj:

```powershell
dotnet ef database update
```

lub uruchom aplikację – baza zostanie utworzona automatycznie dzięki:

```csharp
dbContext.Database.Migrate();
```

---

## 13. Uruchomienie projektu

Visual Studio:

```
F5
```

lub terminal:

```powershell
dotnet build
dotnet run
```

---

## Rozwiązywanie problemów

### Login failed for user 'sa'

Sprawdź:

- SQL Server Authentication jest włączone.
- Konto **sa** jest aktywne.
- Hasło jest poprawne.

---

### A network-related or instance-specific error occurred

Sprawdź:

- SQL Server jest uruchomiony.
- TCP/IP jest włączony.
- Port 1433 został ustawiony.
- Usługa SQL Server została zrestartowana.
- Polecenie:

```powershell
Test-NetConnection localhost -Port 1433
```

zwraca:

```
TcpTestSucceeded : True
```

---

### The certificate chain was issued by an authority that is not trusted

Podczas logowania w SSMS zaznacz opcję:

```
Trust server certificate
```

---

# Installation and Setup (Windows)

## Requirements

Install the following software before running the project:

- Windows 10 or Windows 11
- .NET SDK 8.0
- Visual Studio 2022 or later
- SQL Server Express 2022
- SQL Server Management Studio (SSMS)

## 📂 Pliki konfiguracyjne (.vscode)

Jeśli używasz VS Code, dodaj folder `.vscode/` z zawartością:

### `.vscode/launch.json`

```
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (console)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/CommissionApp/bin/Debug/net8.0/CommissionApp.dll",
      "args": [],
      "cwd": "${workspaceFolder}/CommissionApp",
      "console": "integratedTerminal",
      "stopAtEntry": false
    }
  ]
}
```

### `.vscode/tasks.json`

```
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/CommissionApp/CommissionApp.csproj"
      ],
      "problemMatcher": "$msCompile",
      "group": {
        "kind": "build",
        "isDefault": true
      }
    }
  ]
}
```

---

## ℹ️ Uwagi

- Plik `appsettings.json` zawiera dane do połączenia z bazą danych.
- Migracje EF Core możesz wykonywać ręcznie (`dotnet ef migrations add`) lub programowo (`dbContext.Database.Migrate()`).
- Do połączenia z bazą wymagane są uprawnienia i poprawna konfiguracja portu 1433.

# CommissionApp / działanie aplikacji eng

## Description

**CommissionApp** is a console application for managing customer and car data using an SQL database.  
Users can perform various operations on the data, including CRUD, sorting, importing/exporting, and querying.

---

## Features

### CRUD Operations:
- Creating and adding records manually or from a CSV file  
- Updating records  
- Deleting records  
- Reading and displaying records  

### Additional Functionalities:
- Sorting the database using LINQ and SQL queries  
- Displaying SQL queries and results  
- Creating an audit log file  
- Exporting the database to XML, CSV, and JSON formats  

---

## Main Menu Options

When the application starts, the main menu is displayed with the following options:

1. Add a customer to the SQL database  
2. Add a car to the SQL database  
3. Display all customers from the SQL database  
4. Display all cars from the SQL database  
5. Display all customers and cars from the SQL database  
6. Delete all customers from the SQL database  
7. Delete all cars from the SQL database  
8. Delete all customers and cars from the SQL database  
9. Delete a customer by ID  
10. Delete a car by ID  
11. Display the audit file  
12. Import car data from the `Cars.csv` file  
13. Import customer data from the `Customers.csv` file  
14. Create JSON files for customers and cars  
15. Load data from JSON files into SQL  
16. Create an XML file for cars  
17. Sort car data from SQL by price  
18. Display cars more expensive than 500,000 from SQL  
19. Display customers who can buy cars for a given price  
20. Display cars a customer can buy based on their budget  
21. Export cars grouped by customers to an XML file  

---

## Summary

The **CommissionApp** provides a comprehensive toolkit for managing customer and car data.  
With support for CSV, JSON, and XML file formats, integration with other systems is convenient and flexible.  
The use of an SQL database ensures efficient storage and manipulation of large datasets.  

This application is ideal for businesses needing a streamlined solution for managing sales-related data and analytics.
