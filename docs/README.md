# WebTimeSheetManagement - Dokumentacja Aplikacji

## Przegląd

WebTimeSheetManagement to kompleksowa aplikacja webowa ASP.NET MVC do zarządzania kartami czasu pracy i wydatkami pracowników. System umożliwia śledzenie czasu pracy nad projektami, zarządzanie wydatkami służbowymi oraz proces zatwierdzania przez przełożonych.

## Główne Funkcjonalności

### 🕒 Zarządzanie Kartami Czasu Pracy
- Tworzenie i edycja timesheetów na bazie tygodniowej
- Przypisywanie czasu do konkretnych projektów
- Proces zatwierdzania przez menedżerów
- Historia i audyt zmian

### 💰 Zarządzanie Wydatkami
- Rejestracja wydatków służbowych
- Kategoryzacja wydatków (hotel, transport, posiłki, inne)
- Proces zatwierdzania wydatków
- Eksport raportów wydatków

### 👥 Zarządzanie Użytkownikami
- System rejestracji i autoryzacji
- Różne role użytkowników (Admin, SuperAdmin, User)
- Profile użytkowników z danymi osobowymi
- Zarządzanie dostępem do funkcji

### 📊 Projekty i Raporty
- Zarządzanie projektami firmowymi
- Eksport danych do różnych formatów
- Dashboardy dla różnych ról użytkowników
- System powiadomień

## Architektura

Aplikacja wykorzystuje wzorzec **Repository Pattern** z czystą architekturą warstwową:

```
┌─────────────────────────────────────┐
│        Presentation Layer           │
│    (WebTimeSheetManagement)         │
│         MVC Controllers              │
└─────────────────────────────────────┘
                    │
┌─────────────────────────────────────┐
│        Interface Layer              │
│  (WebTimeSheetManagement.Interface) │
│         Service Contracts           │
└─────────────────────────────────────┘
                    │
┌─────────────────────────────────────┐
│       Implementation Layer          │
│  (WebTimeSheetManagement.Concrete)  │
│      Business Logic & Data Access   │
└─────────────────────────────────────┘
                    │
┌─────────────────────────────────────┐
│          Models Layer               │
│   (WebTimeSheetManagement.Models)   │
│         Entity Models               │
└─────────────────────────────────────┘
```

## Technologie

- **Framework**: ASP.NET MVC 5
- **ORM**: Entity Framework 6
- **Database**: SQL Server
- **Frontend**: Bootstrap 3, jQuery
- **Authentication**: ASP.NET Identity
- **Export**: ClosedXML (Excel)
- **Notifications**: SignalR
- **Logging**: Elmah

## Struktura Dokumentacji

- [Architecture Overview](architecture-overview.md) - Przegląd architektury C4
- [Business Context](business-context.md) - Kontekst biznesowy
- [Containers](containers.md) - Diagram kontenerów
- [Components](components.md) - Diagram komponentów
- [Class Diagrams](class-diagrams.md) - Diagramy klas
- [Database Schema](database-schema.md) - Schemat bazy danych
- [API Documentation](api-documentation.md) - Dokumentacja kontrolerów
- [User Guide](user-guide.md) - Przewodnik użytkownika
- [Deployment](deployment.md) - Instrukcje wdrożenia

## Instalacja i Uruchomienie

1. Sklonuj repozytorium
2. Przywróć pakiety NuGet
3. Skonfiguruj connection string w `web.config`
4. Uruchom migracje bazy danych
5. Skompiluj i uruchom aplikację

Szczegółowe instrukcje w [Deployment Guide](deployment.md).

## Kontakt

Dokumentacja aplikacji WebTimeSheetManagement
Wersja: 1.0
Data aktualizacji: 2025 