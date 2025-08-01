# User Guide - Przewodnik Użytkownika

## Spis Treści

1. [Wprowadzenie](#wprowadzenie)
2. [Pierwsze Kroki](#pierwsze-kroki)
3. [Przewodnik dla Pracownika](#przewodnik-dla-pracownika)
4. [Przewodnik dla Menedżera](#przewodnik-dla-menedżera)
5. [Przewodnik dla Administratora](#przewodnik-dla-administratora)
6. [Funkcje Wspólne](#funkcje-wspólne)
7. [Rozwiązywanie Problemów](#rozwiązywanie-problemów)

## Wprowadzenie

WebTimeSheetManagement to system zarządzania kartami czasu pracy i wydatkami służbowymi. Aplikacja umożliwia:

- **Pracownikom**: Rejestrowanie czasu pracy i wydatków
- **Menedżerom**: Zatwierdzanie kart czasu i wydatków podwładnych
- **Administratorom**: Zarządzanie użytkownikami i projektami

## Pierwsze Kroki

### Logowanie do Systemu

1. Otwórz aplikację w przeglądarce
2. Wprowadź swoją **nazwę użytkownika** i **hasło**
3. Kliknij **Zaloguj**

![Ekran logowania](screenshots/login.png)

### Zmiana Hasła przy Pierwszym Logowaniu

Jeśli system wymaga zmiany hasła:

1. Zostaniesz automatycznie przekierowany do formularza zmiany hasła
2. Wprowadź **aktualne hasło**
3. Wprowadź **nowe hasło** (minimum 7 znaków)
4. Potwierdź **nowe hasło**
5. Kliknij **Zmień hasło**

### Interfejs Użytkownika

Po zalogowaniu zobaczysz:
- **Pasek nawigacji** - główne menu
- **Dashboard** - podsumowanie aktywności
- **Powiadomienia** - alerty i komunikaty
- **Menu użytkownika** - profil i ustawienia

## Przewodnik dla Pracownika

### 1. Dashboard Pracownika

Dashboard zawiera:
- **Podsumowanie obecnego tygodnia**
- **Status ostatnich kart czasu**
- **Powiadomienia o zatwierdzeniach**
- **Skróty do najważniejszych funkcji**

### 2. Zarządzanie Kartami Czasu Pracy

#### Tworzenie Nowej Karty Czasu

1. Przejdź do **TimeSheet** > **Create**
2. Wybierz **zakres dat** (tydzień roboczy)
3. System automatycznie wygeneruje dni tygodnia
4. Dla każdego dnia:
   - Wybierz **projekt** z listy rozwijanej
   - Wprowadź **liczbę godzin** (0-24)
   - Dodaj **opis zadań** (opcjonalnie)

**Przykład wypełnienia:**

| Dzień | Projekt | Godziny | Opis |
|-------|---------|---------|------|
| Poniedziałek | E-commerce Platform | 8 | Implementacja logowania |
| Wtorek | E-commerce Platform | 8 | Testy jednostkowe |
| Środa | Internal Tools | 6 | Dokumentacja API |
| Czwartek | E-commerce Platform | 8 | Code review |
| Piątek | E-commerce Platform | 4 | Bugfixi |

5. Kliknij **Zapisz jako szkic** lub **Prześlij do zatwierdzenia**

#### Edycja Karty Czasu

1. Przejdź do **TimeSheet** > lista kart
2. Kliknij **Edit** przy wybranej karcie
3. Możesz edytować tylko karty ze statusem **Draft**
4. Po wprowadzeniu zmian kliknij **Zapisz**

#### Przesyłanie do Zatwierdzenia

1. Upewnij się, że karta jest kompletna
2. Kliknij **Submit for Approval**
3. Karta zmieni status na **Submitted**
4. Menedżer otrzyma powiadomienie

#### Statusy Kart Czasu

- **Draft** (Szkic) - można edytować
- **Submitted** (Przesłane) - oczekuje na zatwierdzenie
- **Approved** (Zatwierdzone) - zaakceptowane przez menedżera
- **Rejected** (Odrzucone) - wymaga poprawek

### 3. Zarządzanie Wydatkami

#### Dodawanie Nowego Wydatku

1. Przejdź do **Expense** > **Create**
2. Wypełnij formularz:

**Podstawowe informacje:**
- **Projekt** - wybierz z listy
- **Cel/Powód** - opisz wydatek
- **Data od/do** - okres wydatku
- **Numer dokumentu** - ID paragonu/faktury

**Kategorie wydatków:**
- **Hotel** - koszty noclegów
- **Posiłki** - wydatki na jedzenie
- **Transport** - przejazdy, bilety
- **Telefon** - koszty komunikacji
- **Rozrywka** - wydatki biznesowe
- **Paliwo** - koszty paliwa
- **Konserwacja** - naprawy, serwis
- **Inne** - pozostałe wydatki

3. System automatycznie obliczy **sumę wydatków**
4. Kliknij **Zapisz** lub **Prześlij do zatwierdzenia**

#### Przykład Wydatku

```
Projekt: E-commerce Platform
Cel: Spotkanie z klientem w Warszawie
Data: 15-17.01.2025
Numer dokumentu: EXP-2025-001

Hotel: 600 zł (2 noce x 300 zł)
Posiłki: 180 zł (3 dni x 60 zł)
Transport: 240 zł (pociąg tam i z powrotem)
Telefon: 50 zł (roaming)
Inne: 30 zł (parking)

Suma: 1100 zł
```

### 4. Załączniki do Wydatków

1. W formularzu wydatku kliknij **Dodaj załącznik**
2. Wybierz pliki (PDF, JPG, PNG)
3. Maksymalny rozmiar: 5MB na plik
4. Dozwolone typy: faktury, paragony, bilety

### 5. Profil Użytkownika

#### Aktualizacja Profilu

1. Kliknij na swoje **imię** w prawym górnym rogu
2. Wybierz **Profil**
3. Możesz zmienić:
   - Numer telefonu
   - Adres email (wymaga zatwierdzenia)
   - Dane osobowe

#### Zmiana Hasła

1. Przejdź do **Profil** > **Zmień hasło**
2. Wprowadź aktualne hasło
3. Wprowadź nowe hasło (minimum 7 znaków)
4. Potwierdź nowe hasło
5. Kliknij **Zmień hasło**

## Przewodnik dla Menedżera

### 1. Dashboard Menedżera

Dashboard menedżera pokazuje:
- **Karty czasu do zatwierdzenia**
- **Wydatki do zatwierdzenia**
- **Statystyki zespołu**
- **Alerty i powiadomienia**

### 2. Zatwierdzanie Kart Czasu

#### Przegląd Oczekujących Kart

1. Przejdź do **All TimeSheets** lub **Pending Approvals**
2. Zobacz listę kart ze statusem **Submitted**
3. Kliknij **Details** przy wybranej karcie

#### Proces Zatwierdzania

1. **Przegląd szczegółów:**
   - Sprawdź przypisanie godzin do projektów
   - Zweryfikuj opisy zadań
   - Sprawdź czy suma godzin jest realna

2. **Decyzja:**
   - **Zatwierdź** - jeśli wszystko jest poprawne
   - **Odrzuć** - jeśli wymaga poprawek

3. **Komentarz:**
   - Zawsze dodaj komentarz przy odrzuceniu
   - Komentarz przy zatwierdzeniu jest opcjonalny

**Przykłady komentarzy:**
- Zatwierdzenie: "Zatwierdzone - dobra robota w tym tygodniu"
- Odrzucenie: "Proszę o więcej szczegółów dla zadań projektowych w środę"

#### Masowe Zatwierdzanie

1. Zaznacz checkboxy przy kartach do zatwierdzenia
2. Kliknij **Approve Selected**
3. Dodaj wspólny komentarz (opcjonalnie)
4. Potwierdź działanie

### 3. Zatwierdzanie Wydatków

#### Przegląd Wydatków

1. Przejdź do **All Expenses**
2. Filtruj po statusie **Submitted**
3. Sprawdź szczegóły każdego wydatku

#### Kryteria Oceny Wydatków

- **Zgodność z polityką firmową**
- **Dostępność załączników (paragony/faktury)**
- **Uzasadnienie biznesowe**
- **Kwoty zgodne z dokumentami**

#### Proces Zatwierdzania Wydatku

1. Kliknij **Review** przy wydatku
2. Sprawdź:
   - Cel wydatku
   - Załączone dokumenty
   - Kategoryzację kosztów
   - Zgodność z budżetem projektu

3. Podjąć decyzję:
   - **Approve** - zatwierdź
   - **Reject** - odrzuć z uzasadnieniem

### 4. Raporty dla Menedżera

#### Raport Czasu Pracy Zespołu

1. Przejdź do **Reports** > **Team TimeSheets**
2. Wybierz zakres dat
3. Wybierz członków zespołu
4. Kliknij **Generate Report**

Raport zawiera:
- Łączny czas pracy każdego pracownika
- Podział czasu na projekty
- Efektywność zespołu
- Trendy czasowe

#### Raport Wydatków

1. Przejdź do **Reports** > **Team Expenses**
2. Ustaw filtry (daty, projekty, pracownicy)
3. Generuj raport Excel lub PDF

### 5. Zarządzanie Zespołem

#### Przegląd Aktywności Zespołu

1. **Team Dashboard** - podsumowanie zespołu
2. **Individual Reports** - raporty poszczególnych pracowników
3. **Project Allocation** - alokacja czasu na projekty

## Przewodnik dla Administratora

### 1. Panel Administracyjny

Główne funkcje administracyjne:
- **Zarządzanie użytkownikami**
- **Zarządzanie projektami**
- **Konfiguracja systemu**
- **Raporty systemowe**
- **Audyt systemu**

### 2. Zarządzanie Użytkownikami

#### Dodawanie Nowego Użytkownika

1. Przejdź do **Admin** > **Users** > **Add New**
2. Wypełnij formularz:
   - **Imię i nazwisko**
   - **Email** (unikalny)
   - **Nazwa użytkownika** (unikalna)
   - **Numer telefonu**
   - **ID pracownika**
   - **Data zatrudnienia**
   - **Rola początkowa**

3. System automatycznie wygeneruje hasło tymczasowe
4. Użytkownik otrzyma email z danymi logowania

#### Zarządzanie Rolami

**Dostępne role:**
- **Employee** - podstawowy pracownik
- **Manager** - może zatwierdzać karty i wydatki
- **Admin** - zarządzanie użytkownikami i projektami
- **SuperAdmin** - pełny dostęp do systemu

**Zmiana roli:**
1. Znajdź użytkownika na liście
2. Kliknij **Edit Role**
3. Wybierz nową rolę
4. Potwierdź zmianę

#### Deaktywacja/Aktywacja Użytkownika

1. Znajdź użytkownika na liście
2. Kliknij **Deactivate/Activate**
3. Potwierdź działanie
4. Dezaktywowany użytkownik nie może się logować

### 3. Zarządzanie Projektami

#### Tworzenie Nowego Projektu

1. Przejdź do **Admin** > **Projects** > **Add New**
2. Wypełnij formularz:
   - **Kod projektu** (unikalny, np. PROJ-2025-001)
   - **Nazwa projektu**
   - **Branża/Sektor**
   - **Status** (Aktywny/Nieaktywny)

#### Przypisywanie Użytkowników do Projektów

1. Otwórz szczegóły projektu
2. Kliknij **Assign Users**
3. Wybierz użytkowników z listy
4. Ustaw daty przypisania
5. Zapisz zmiany

#### Archiwizacja Projektów

1. Znajdź zakończony projekt
2. Kliknij **Archive**
3. Projekt będzie ukryty w standardowych widokach
4. Historyczne dane pozostaną dostępne

### 4. Raporty Systemowe

#### Raport Wykorzystania Systemu

1. **Daily Active Users** - dzienne logowania
2. **TimeSheet Completion Rate** - procent wypełnionych kart
3. **Approval Times** - średni czas zatwierdzania
4. **System Performance** - wydajność aplikacji

#### Raport Audytu

1. Przejdź do **Admin** > **Audit Reports**
2. Wybierz zakres dat
3. Filtruj po użytkownikach lub akcjach
4. Exportuj do Excel

Raport zawiera:
- Kto się logował i kiedy
- Jakie strony były odwiedzane
- Jakie akcje były wykonywane
- Adresy IP użytkowników

### 5. Konfiguracja Systemu

#### Ustawienia Globalne

1. **Company Settings** - dane firmy
2. **Email Configuration** - ustawienia SMTP
3. **Security Settings** - polityki haseł
4. **Backup Settings** - harmonogram kopii zapasowych

#### Konfiguracja Powiadomień

1. **Email Templates** - szablony wiadomości
2. **Notification Rules** - reguły wysyłania
3. **User Preferences** - preferencje użytkowników

## Funkcje Wspólne

### 1. System Powiadomień

#### Typy Powiadomień

- **Email** - ważne komunikaty (zatwierdzenia, odrzucenia)
- **In-App** - powiadomienia w aplikacji
- **Real-time** - natychmiastowe alerty (SignalR)

#### Zarządzanie Powiadomieniami

1. Kliknij ikonę **dzwonka** w pasku nawigacji
2. Zobacz listę najnowszych powiadomień
3. Kliknij powiadomienie, aby je otworzyć
4. Nieodczytane powiadomienia są pogrubione

### 2. Eksport Danych

#### Export do Excel

1. W dowolnej liście (TimeSheets, Expenses, Users)
2. Kliknij **Export to Excel**
3. Wybierz zakres dat (jeśli dotyczy)
4. Plik zostanie pobrany automatycznie

#### Dostępne Formaty

- **Excel (XLSX)** - dla większości raportów
- **PDF** - dla oficjalnych dokumentów
- **CSV** - dla importu do innych systemów

### 3. Wyszukiwanie i Filtrowanie

#### Globalne Wyszukiwanie

1. Użyj pola wyszukiwania w górnej części strony
2. Wpisz frazę (nazwa projektu, użytkownik, opis)
3. System przeszuka wszystkie dostępne dane

#### Zaawansowane Filtry

Na listach dostępne są filtry:
- **Zakres dat** - od/do
- **Status** - Draft, Submitted, Approved, Rejected
- **Użytkownik** - konkretny pracownik
- **Projekt** - konkretny projekt

### 4. Keyboard Shortcuts

- **Ctrl + S** - Zapisz formularz
- **Ctrl + Enter** - Prześlij do zatwierdzenia
- **Escape** - Anuluj/Zamknij
- **F5** - Odśwież stronę

## Rozwiązywanie Problemów

### Częste Problemy

#### Problem: Nie mogę się zalogować

**Możliwe przyczyny:**
- Błędne dane logowania
- Konto zostało dezaktywowane
- Wymagana zmiana hasła

**Rozwiązanie:**
1. Sprawdź caps lock
2. Spróbuj opcji "Forgot Password"
3. Skontaktuj się z administratorem

#### Problem: Nie widzę swoich kart czasu

**Możliwe przyczyny:**
- Nie masz uprawnień do tej sekcji
- Karty zostały usunięte
- Problem z filtrowaniem

**Rozwiązanie:**
1. Sprawdź filtry (resetuj do domyślnych)
2. Sprawdź zakres dat
3. Skontaktuj się z administratorem

#### Problem: Nie mogę edytować karty czasu

**Możliwe przyczyny:**
- Karta ma status inny niż "Draft"
- Nie jesteś właścicielem karty
- Przekroczony termin edycji

**Rozwiązanie:**
1. Sprawdź status karty
2. Jeśli karta została odrzucona, możesz ją edytować
3. Skontaktuj się z menedżerem

#### Problem: Błąd podczas uploadu załącznika

**Możliwe przyczyny:**
- Plik za duży (max 5MB)
- Nieobsługiwany format pliku
- Problem z połączeniem

**Rozwiązanie:**
1. Sprawdź rozmiar pliku
2. Użyj formatów: PDF, JPG, PNG
3. Spróbuj ponownie

### Wsparcie Techniczne

#### Informacje do Zgłoszeń

Przy zgłaszaniu problemów podaj:
- **Opis problemu** - co się stało
- **Kroki do odtworzenia** - jak wywołać błąd
- **Przeglądarka i wersja** - Chrome, Firefox, Edge
- **Zrzut ekranu** - jeśli możliwe
- **Komunikat błędu** - dokładny tekst

#### Kontakt

- **Email**: support@company.com
- **Telefon**: +48 123 456 789
- **Ticket System**: help.company.com
- **Chat**: dostępny w aplikacji (ikona w prawym dolnym rogu)

### Najlepsze Praktyki

#### Dla Pracowników

1. **Regularne wypełnianie** - nie czekaj do końca tygodnia
2. **Dokładne opisy** - pomagają menedżerom w ocenie
3. **Backup dokumentów** - zachowaj kopie paragonów
4. **Komunikacja** - informuj o problemach z projektami

#### Dla Menedżerów

1. **Szybkie zatwierdzanie** - nie opóźniaj płatności
2. **Konstruktywne komentarze** - pomagaj pracownikom się rozwijać
3. **Regularne sprawdzanie** - monitoruj status zespołu
4. **Feedback** - przekazuj informacje zwrotne

#### Dla Administratorów

1. **Regularne backupy** - zabezpieczaj dane
2. **Monitoring systemu** - obserwuj wydajność
3. **Aktualizacje** - utrzymuj system w najnowszej wersji
4. **Szkolenia** - organizuj szkolenia dla użytkowników

## Dodatki

### Skróty i Porady

#### Szybkie Wypełnianie TimeSheet

1. Użyj funkcji **Copy from Previous Week**
2. Skonfiguruj **Default Projects** w profilu
3. Użyj **Templates** dla powtarzających się zadań

#### Efektywne Zarządzanie Wydatkami

1. Fotografuj paragony od razu
2. Używaj aplikacji mobilnej (jeśli dostępna)
3. Kategoryzuj wydatki podczas dodawania

#### Optymalizacja Pracy Menedżera

1. Ustaw **Email Notifications** dla nowych zgłoszeń
2. Używaj **Bulk Actions** do masowego zatwierdzania
3. Konfiguruj **Dashboard Widgets** podle swoich potrzeb

### Glosariusz

- **TimeSheet** - Karta czasu pracy
- **Expense** - Wydatek służbowy
- **Approval** - Zatwierdzenie
- **Draft** - Szkic (niezatwierdzony)
- **Submitted** - Przesłane do zatwierdzenia
- **Approved** - Zatwierdzone
- **Rejected** - Odrzucone
- **Audit Trail** - Ślad audytowy
- **Dashboard** - Pulpit/Tablica rozdzielcza 