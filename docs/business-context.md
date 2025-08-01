# Business Context - Kontekst Biznesowy

## Cel Biznesowy

WebTimeSheetManagement to system wspierający organizacje w efektywnym zarządzaniu czasem pracy pracowników oraz kontroli wydatków służbowych. System automatyzuje procesy raportowania czasu pracy, ułatwia rozliczenia projektowe oraz zapewnia transparentność w zarządzaniu zasobami ludzkimi.

## Użytkownicy Systemu

### 👤 Employee (Pracownik)
**Rola**: Końcowy użytkownik systemu
**Uprawnienia**:
- Tworzenie i edycja własnych kart czasu pracy
- Dodawanie wydatków służbowych
- Przeglądanie historii własnych timesheetów
- Otrzymywanie powiadomień o statusie zatwierdzenia

**Przepływ pracy**:
```plantuml
@startuml
title Employee Workflow

start
:Logowanie do systemu;
:Wybór okresu rozliczeniowego;
:Wypełnienie karty czasu pracy;
:Przypisanie czasu do projektów;
:Dodanie wydatków (opcjonalnie);
:Przesłanie do zatwierdzenia;
:Oczekiwanie na decyzję managera;
if (Zatwierdzone?) then (Tak)
  :Karta zatwierdzona;
  stop
else (Nie)
  :Korekta karty;
  :Ponowne przesłanie;
endif
@enduml
```

### 👨‍💼 Manager (Menedżer)
**Rola**: Przełożony zatwierdzający karty czasu i wydatki
**Uprawnienia**:
- Przeglądanie kart czasu podwładnych
- Zatwierdzanie/odrzucanie timesheetów
- Zatwierdzanie/odrzucanie wydatków
- Dodawanie komentarzy do decyzji
- Generowanie raportów zespołowych

**Przepływ pracy**:
```plantuml
@startuml
title Manager Approval Workflow

start
:Otrzymanie powiadomienia o nowej karcie;
:Przegląd karty czasu pracy;
:Weryfikacja przypisania do projektów;
if (Karta poprawna?) then (Tak)
  :Zatwierdzenie karty;
  :Wysłanie powiadomienia do pracownika;
else (Nie)
  :Odrzucenie z komentarzem;
  :Powiadomienie o konieczności korekty;
endif
:Aktualizacja statusu w systemie;
stop
@enduml
```

### 🔧 Administrator
**Rola**: Zarządzanie systemem i użytkownikami
**Uprawnienia**:
- Zarządzanie kontami użytkowników
- Tworzenie i edycja projektów
- Konfiguracja ról i uprawnień
- Generowanie raportów systemowych
- Zarządzanie powiadomieniami

### 👑 Super Administrator
**Rola**: Pełne zarządzanie systemem
**Uprawnienia**:
- Wszystkie uprawnienia Administratora
- Zarządzanie konfiguracją systemu
- Dostęp do logów systemowych
- Zarządzanie backupami
- Konfiguracja integracji z systemami zewnętrznymi

## Główne Procesy Biznesowe

### 1. Proces Zarządzania Kartami Czasu Pracy

```plantuml
@startuml
title TimeSheet Management Process

|Employee|
start
:Wybór tygodnia rozliczeniowego;
:Wprowadzenie godzin pracy na projekty;
:Dodanie opisów zadań;
:Zapisanie karty jako draft;
note right: Możliwość wielokrotnej edycji
:Przesłanie do zatwierdzenia;

|System|
:Powiadomienie managera;
:Zmiana statusu na "Submitted";

|Manager|
:Przegląd karty czasu;
if (Zatwierdza?) then (Tak)
  :Zatwierdzenie karty;
  |System|
  :Status "Approved";
  :Powiadomienie pracownika;
else (Nie)
  :Odrzucenie z komentarzem;
  |System|
  :Status "Rejected";
  :Powiadomienie pracownika;
  |Employee|
  :Korekta karty;
  :Ponowne przesłanie;
endif

stop
@enduml
```

### 2. Proces Zarządzania Wydatkami

```plantuml
@startuml
title Expense Management Process

|Employee|
start
:Dodanie nowego wydatku;
:Wybór projektu;
:Określenie kategorii wydatku;
:Wprowadzenie kwoty i daty;
:Dodanie opisu/uzasadnienia;
:Załączenie dokumentów (opcjonalne);
:Przesłanie do zatwierdzenia;

|System|
:Powiadomienie managera;
:Status "Submitted";

|Manager|
:Przegląd wydatku;
:Weryfikacja zgodności z polityką;
if (Zatwierdza?) then (Tak)
  :Zatwierdzenie wydatku;
  |System|
  :Status "Approved";
  :Powiadomienie pracownika;
  :Przekazanie do rozliczenia;
else (Nie)
  :Odrzucenie z uzasadnieniem;
  |System|
  :Status "Rejected";
  :Powiadomienie pracownika;
endif

stop
@enduml
```

## Korzyści Biznesowe

### 📈 Zwiększenie Efektywności
- **Automatyzacja raportowania**: Eliminacja papierowych timesheetów
- **Szybszy proces zatwierdzania**: Workflow elektroniczny
- **Redukcja błędów**: Walidacja danych na poziomie systemu
- **Oszczędność czasu**: Automatyczne kalkulacje i raporty

### 💰 Kontrola Kosztów
- **Transparentność wydatków**: Pełna widoczność kosztów projektowych
- **Kontrola budżetu**: Monitoring wydatków w czasie rzeczywistym
- **Audyt finansowy**: Kompletny audit trail wszystkich transakcji
- **Optymalizacja zasobów**: Analiza wykorzystania czasu pracy

### 📊 Wsparcie Decyzyjne
- **Raporty projektowe**: Analiza rentowności projektów
- **Monitoring wydajności**: KPI dla zespołów i projektów
- **Planowanie zasobów**: Przewidywanie potrzeb kadrowych
- **Compliance**: Zgodność z regulacjami prawnymi

## Metryki Biznesowe

### KPI (Key Performance Indicators)
- **Czas zatwierdzania kart**: Średni czas od przesłania do zatwierdzenia
- **Wskaźnik odrzuceń**: Procent odrzuconych kart czasu/wydatków
- **Wykorzystanie projektów**: Liczba godzin na projekt
- **Efektywność zespołu**: Liczba zatwierdzonych kart na osobę
- **Kontrola budżetu**: Rzeczywiste vs planowane wydatki

### Cele Biznesowe
1. **Skrócenie czasu rozliczenia** o 50% w porównaniu do procesu papierowego
2. **Zwiększenie dokładności raportowania** do 99%
3. **Redukcja kosztów administracyjnych** o 30%
4. **Poprawa satysfakcji użytkowników** poprzez uproszczenie procesów
5. **Zapewnienie compliance** z wymaganiami audytowymi

## Integracje Systemowe

### Istniejące Systemy
- **HR System**: Synchronizacja danych pracowników
- **Accounting System**: Eksport danych finansowych
- **Project Management**: Synchronizacja projektów
- **Email System**: Powiadomienia i alerty

### Planowane Integracje
- **Mobile App**: Aplikacja mobilna dla pracowników
- **BI Dashboard**: Zaawansowane raportowanie
- **Calendar Integration**: Synchronizacja z kalendarzami
- **Document Management**: Zarządzanie załącznikami 