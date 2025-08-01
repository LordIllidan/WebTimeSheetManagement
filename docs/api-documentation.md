# API Documentation - Dokumentacja Kontrolerów

## Przegląd

WebTimeSheetManagement to aplikacja MVC, więc nie posiada tradycyjnego REST API. Zamiast tego wykorzystuje kontrolery MVC, które obsługują żądania HTTP i zwracają widoki HTML lub dane JSON. Ten dokument opisuje wszystkie dostępne endpoint'y kontrolerów.

## Struktura URL

Aplikacja wykorzystuje standardowe routing MVC:
```
/{Controller}/{Action}/{id?}
```

## Autoryzacja

Większość kontrolerów wymaga autoryzacji. Systemy ról:
- **Employee**: Podstawowy użytkownik
- **Manager**: Może zatwierdzać karty i wydatki
- **Admin**: Zarządzanie użytkownikami i projektami  
- **SuperAdmin**: Pełny dostęp do systemu

## 1. TimeSheetController

Zarządzanie kartami czasu pracy.

### Endpoints

#### GET /TimeSheet
```http
GET /TimeSheet
Authorization: Required (Employee+)
Description: Lista kart czasu pracy użytkownika
Parameters:
  - sortColumn (string): Kolumna sortowania
  - sortColumnDir (string): Kierunek sortowania (asc/desc)
  - Search (string): Fraza wyszukiwania
Returns: HTML View z listą timesheetów
```

#### GET /TimeSheet/Create
```http
GET /TimeSheet/Create
Authorization: Required (Employee+)
Description: Formularz tworzenia nowej karty czasu pracy
Returns: HTML View z formularzem
```

#### POST /TimeSheet/Create
```http
POST /TimeSheet/Create
Authorization: Required (Employee+)
Content-Type: application/x-www-form-urlencoded
Description: Tworzenie nowej karty czasu pracy

Body Parameters:
{
  "FromDate": "2025-01-01",
  "ToDate": "2025-01-07",
  "ProjectID": 1,
  "text1_p1": 8,  // Monday hours
  "text2_p1": 8,  // Tuesday hours
  "text3_p1": 8,  // Wednesday hours
  "text4_p1": 8,  // Thursday hours
  "text5_p1": 8,  // Friday hours
  "text6_p1": 0,  // Saturday hours
  "text7_p1": 0,  // Sunday hours
  "UserStoryDescription": "Feature implementation",
  "TaskDescription": "Implementing login functionality",
  "MainTaskDescription": "Backend development"
}

Returns: 
  - Success: Redirect to TimeSheet list
  - Error: View with validation errors
```

#### GET /TimeSheet/Edit/{id}
```http
GET /TimeSheet/Edit/123
Authorization: Required (Employee+, Own records only)
Description: Formularz edycji karty czasu pracy
Parameters:
  - id (int): ID karty czasu pracy
Returns: HTML View z wypełnionym formularzem
```

#### POST /TimeSheet/Edit/{id}
```http
POST /TimeSheet/Edit/123
Authorization: Required (Employee+, Own records only)
Content-Type: application/x-www-form-urlencoded
Description: Aktualizacja karty czasu pracy
Returns: Redirect to TimeSheet list or View with errors
```

#### POST /TimeSheet/Submit/{id}
```http
POST /TimeSheet/Submit/123
Authorization: Required (Employee+, Own records only)
Description: Przesłanie karty do zatwierdzenia
Parameters:
  - id (int): ID karty czasu pracy
Returns: JSON
{
  "success": true,
  "message": "TimeSheet submitted successfully"
}
```

#### POST /TimeSheet/Approve/{id}
```http
POST /TimeSheet/Approve/123
Authorization: Required (Manager+)
Description: Zatwierdzenie karty czasu pracy
Parameters:
  - id (int): ID karty czasu pracy
Body:
{
  "Comment": "Approved - good work"
}
Returns: JSON response
```

#### POST /TimeSheet/Reject/{id}
```http
POST /TimeSheet/Reject/123
Authorization: Required (Manager+)
Description: Odrzucenie karty czasu pracy
Body:
{
  "Comment": "Please provide more details for Task X"
}
Returns: JSON response
```

#### GET /TimeSheet/Delete/{id}
```http
GET /TimeSheet/Delete/123
Authorization: Required (Employee+, Own records in Draft status only)
Description: Usunięcie karty czasu pracy
Returns: Redirect to TimeSheet list
```

## 2. ExpenseController

Zarządzanie wydatkami służbowymi.

### Endpoints

#### GET /Expense
```http
GET /Expense
Authorization: Required (Employee+)
Description: Lista wydatków użytkownika
Returns: HTML View z listą wydatków
```

#### GET /Expense/Create
```http
GET /Expense/Create
Authorization: Required (Employee+)
Description: Formularz dodawania wydatku
Returns: HTML View z formularzem
```

#### POST /Expense/Create
```http
POST /Expense/Create
Authorization: Required (Employee+)
Content-Type: application/x-www-form-urlencoded
Description: Tworzenie nowego wydatku

Body Parameters:
{
  "ProjectID": 1,
  "PurposeorReason": "Business trip to client",
  "FromDate": "2025-01-15",
  "ToDate": "2025-01-17",
  "VoucherID": "EXP-2025-001",
  "Hotel": 300,
  "Meal": 150,
  "Transportation": 200,
  "Phone": 50,
  "Entertainment": 100,
  "Fuel": 80,
  "Maintenance": 0,
  "Others": 25
}

Returns: Redirect to Expense list or View with errors
```

#### POST /Expense/Approve/{id}
```http
POST /Expense/Approve/123
Authorization: Required (Manager+)
Description: Zatwierdzenie wydatku
Body:
{
  "Comment": "Approved as per company policy"
}
Returns: JSON response
```

#### POST /Expense/Reject/{id}
```http
POST /Expense/Reject/123
Authorization: Required (Manager+)
Description: Odrzucenie wydatku
Body:
{
  "Comment": "Receipts required for hotel expenses"
}
Returns: JSON response
```

## 3. UserController

Zarządzanie profilem użytkownika.

### Endpoints

#### GET /User/Profile
```http
GET /User/Profile
Authorization: Required (Employee+)
Description: Profil użytkownika
Returns: HTML View z danymi profilu
```

#### POST /User/Profile
```http
POST /User/Profile
Authorization: Required (Employee+)
Content-Type: application/x-www-form-urlencoded
Description: Aktualizacja profilu użytkownika

Body Parameters:
{
  "Name": "Jan Kowalski",
  "Mobileno": "123456789",
  "EmailID": "jan.kowalski@company.com",
  "Gender": "Male",
  "Birthdate": "1990-01-01"
}

Returns: View with success/error message
```

#### GET /User/ChangePassword
```http
GET /User/ChangePassword
Authorization: Required (Employee+)
Description: Formularz zmiany hasła
Returns: HTML View z formularzem
```

#### POST /User/ChangePassword
```http
POST /User/ChangePassword
Authorization: Required (Employee+)
Content-Type: application/x-www-form-urlencoded
Description: Zmiana hasła użytkownika

Body Parameters:
{
  "Username": "jkowalski",
  "OldPassword": "currentpassword",
  "NewPassword": "newpassword123",
  "ConfirmPassword": "newpassword123"
}

Returns: View with success/error message
```

#### GET /User/Dashboard
```http
GET /User/Dashboard
Authorization: Required (Employee+)
Description: Dashboard użytkownika z podsumowaniem
Returns: HTML View z statystykami użytkownika
```

## 4. AdminController

Funkcje administracyjne systemu.

### Endpoints

#### GET /Admin
```http
GET /Admin
Authorization: Required (Admin+)
Description: Panel administracyjny
Returns: HTML View z opcjami administracyjnymi
```

#### GET /Admin/Users
```http
GET /Admin/Users
Authorization: Required (Admin+)
Description: Lista wszystkich użytkowników
Parameters:
  - sortColumn (string): Kolumna sortowania
  - sortColumnDir (string): Kierunek sortowania
  - Search (string): Fraza wyszukiwania
Returns: HTML View z listą użytkowników
```

#### GET /Admin/UserDetails/{id}
```http
GET /Admin/UserDetails/123
Authorization: Required (Admin+)
Description: Szczegóły użytkownika
Returns: HTML View z danymi użytkownika
```

#### POST /Admin/UpdateUserRole
```http
POST /Admin/UpdateUserRole
Authorization: Required (Admin+)
Content-Type: application/json
Description: Aktualizacja roli użytkownika

Body:
{
  "EmployeeID": "EMP001",
  "RoleID": 2
}

Returns: JSON
{
  "success": true,
  "message": "User role updated successfully"
}
```

## 5. ProjectController

Zarządzanie projektami.

### Endpoints

#### GET /Project
```http
GET /Project
Authorization: Required (Admin+)
Description: Lista projektów
Returns: HTML View z listą projektów
```

#### GET /Project/Create
```http
GET /Project/Create
Authorization: Required (Admin+)
Description: Formularz tworzenia projektu
Returns: HTML View z formularzem
```

#### POST /Project/Create
```http
POST /Project/Create
Authorization: Required (Admin+)
Content-Type: application/x-www-form-urlencoded
Description: Tworzenie nowego projektu

Body Parameters:
{
  "ProjectCode": "PROJ-2025-001",
  "ProjectName": "E-commerce Platform",
  "NatureofIndustry": "Software Development"
}

Returns: Redirect to Project list or View with errors
```

#### GET /Project/Edit/{id}
```http
GET /Project/Edit/123
Authorization: Required (Admin+)
Description: Formularz edycji projektu
Returns: HTML View z wypełnionym formularzem
```

#### POST /Project/Edit/{id}
```http
POST /Project/Edit/123
Authorization: Required (Admin+)
Description: Aktualizacja projektu
Returns: Redirect to Project list or View with errors
```

#### GET /Project/Delete/{id}
```http
GET /Project/Delete/123
Authorization: Required (Admin+)
Description: Usunięcie projektu
Returns: Redirect to Project list
```

## 6. LoginController

Autoryzacja użytkowników.

### Endpoints

#### GET /Login
```http
GET /Login
Authorization: Public
Description: Formularz logowania
Returns: HTML View z formularzem logowania
```

#### POST /Login
```http
POST /Login
Authorization: Public
Content-Type: application/x-www-form-urlencoded
Description: Logowanie użytkownika

Body Parameters:
{
  "Username": "jkowalski",
  "Password": "password123",
  "RememberMe": false
}

Returns: 
  - Success: Redirect to Dashboard
  - Error: Login view with error message
```

#### GET /Login/Logout
```http
GET /Login/Logout
Authorization: Required
Description: Wylogowanie użytkownika
Returns: Redirect to Login page
```

#### GET /Login/ForgotPassword
```http
GET /Login/ForgotPassword
Authorization: Public
Description: Formularz resetowania hasła
Returns: HTML View z formularzem
```

#### POST /Login/ForgotPassword
```http
POST /Login/ForgotPassword
Authorization: Public
Content-Type: application/x-www-form-urlencoded
Description: Wysłanie linku resetującego hasło

Body Parameters:
{
  "EmailID": "user@company.com"
}

Returns: View with confirmation message
```

## 7. RegistrationController

Rejestracja nowych użytkowników.

### Endpoints

#### GET /Registration
```http
GET /Registration
Authorization: Public
Description: Formularz rejestracji
Returns: HTML View z formularzem rejestracji
```

#### POST /Registration
```http
POST /Registration
Authorization: Public
Content-Type: application/x-www-form-urlencoded
Description: Rejestracja nowego użytkownika

Body Parameters:
{
  "Name": "Jan Kowalski",
  "Mobileno": "123456789",
  "EmailID": "jan.kowalski@company.com",
  "Username": "jkowalski",
  "Password": "password123",
  "ConfirmPassword": "password123",
  "Gender": "Male",
  "Birthdate": "1990-01-01",
  "DateofJoining": "2025-01-15",
  "EmployeeID": "EMP001"
}

Returns: 
  - Success: Redirect to Login
  - Error: Registration view with validation errors
```

## 8. Export Controllers

### TimeSheetExportController

#### GET /TimeSheetExport/ExportToExcel
```http
GET /TimeSheetExport/ExportToExcel
Authorization: Required (Employee+)
Parameters:
  - FromDate (DateTime): Data od
  - ToDate (DateTime): Data do
  - UserID (int): ID użytkownika (opcjonalne dla adminów)
Description: Eksport kart czasu do Excel
Returns: Excel file (application/vnd.openxmlformats-officedocument.spreadsheetml.sheet)
```

### ExpenseExportController

#### GET /ExpenseExport/ExportToExcel
```http
GET /ExpenseExport/ExportToExcel
Authorization: Required (Employee+)
Parameters:
  - FromDate (DateTime): Data od
  - ToDate (DateTime): Data do
  - UserID (int): ID użytkownika (opcjonalne dla adminów)
Description: Eksport wydatków do Excel
Returns: Excel file
```

## 9. NotificationController

Zarządzanie powiadomieniami.

### Endpoints

#### GET /Notification
```http
GET /Notification
Authorization: Required (Employee+)
Description: Lista powiadomień użytkownika
Returns: HTML View z powiadomieniami
```

#### POST /Notification/MarkAsRead
```http
POST /Notification/MarkAsRead
Authorization: Required (Employee+)
Content-Type: application/json
Description: Oznaczanie powiadomienia jako przeczytane

Body:
{
  "NotificationID": 123
}

Returns: JSON
{
  "success": true
}
```

#### GET /Notification/GetUnreadCount
```http
GET /Notification/GetUnreadCount
Authorization: Required (Employee+)
Description: Liczba nieprzeczytanych powiadomień
Returns: JSON
{
  "count": 5
}
```

## 10. SignalR Hub

### NotificationHub

Real-time powiadomienia wykorzystujące SignalR.

#### Methods

```javascript
// Connect to hub
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Listen for notifications
connection.on("ReceiveNotification", function (title, message, type) {
    // Display notification to user
    showNotification(title, message, type);
});

// Send notification (server-side)
Clients.User(userId).SendAsync("ReceiveNotification", title, message, type);
```

## Error Handling

### Standardowe Kody Odpowiedzi

```http
200 OK - Żądanie zakończone sukcesem
302 Found - Przekierowanie (typowe dla MVC)
400 Bad Request - Błędne dane wejściowe
401 Unauthorized - Brak autoryzacji
403 Forbidden - Brak uprawnień
404 Not Found - Zasób nie istnieje
500 Internal Server Error - Błąd serwera
```

### Przykład Odpowiedzi Error

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    {
      "field": "FromDate",
      "message": "From Date is required"
    },
    {
      "field": "ProjectID", 
      "message": "Please select a project"
    }
  ]
}
```

## Przykłady Użycia

### 1. Tworzenie TimeSheet (jQuery)

```javascript
$.ajax({
    url: '/TimeSheet/Create',
    type: 'POST',
    data: {
        FromDate: '2025-01-01',
        ToDate: '2025-01-07',
        ProjectID: 1,
        text1_p1: 8,
        text2_p1: 8,
        text3_p1: 8,
        text4_p1: 8,
        text5_p1: 8,
        text6_p1: 0,
        text7_p1: 0,
        UserStoryDescription: 'Login feature',
        TaskDescription: 'Implement authentication'
    },
    success: function(response) {
        window.location.href = '/TimeSheet';
    },
    error: function(xhr, status, error) {
        alert('Error creating timesheet');
    }
});
```

### 2. Zatwierdzanie wydatku (JavaScript)

```javascript
function approveExpense(expenseId) {
    $.ajax({
        url: '/Expense/Approve/' + expenseId,
        type: 'POST',
        data: {
            Comment: 'Approved as per policy'
        },
        success: function(response) {
            if(response.success) {
                alert('Expense approved successfully');
                location.reload();
            }
        }
    });
}
```

### 3. SignalR Notifications

```javascript
// Initialize SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Start connection
connection.start().then(function () {
    console.log('SignalR Connected');
}).catch(function (err) {
    console.error(err.toString());
});

// Listen for notifications
connection.on("ReceiveNotification", function (title, message, type) {
    toastr[type](message, title);
    updateNotificationCount();
});
```

## Validacja Danych

### TimeSheet Validation Rules

```csharp
[RegularExpression(@"^\d+$", ErrorMessage = "Enter Only Numbers")]
[Range(0, 24, ErrorMessage = "0 to 24")]
public int? text1_p1 { get; set; }

[Required(ErrorMessage = "Choose Project")]
public int? ProjectID { get; set; }
```

### Expense Validation Rules

```csharp
[Required(ErrorMessage = "Please Enter Purpose/Reason")]
public string PurposeorReason { get; set; }

[Required(ErrorMessage = "Please Choose From Date")]
public DateTime? FromDate { get; set; }

[RegularExpression(@"^\d+$", ErrorMessage = "Enter Only Numbers")]
public int? Hotel { get; set; }
```

### User Registration Validation

```csharp
[Required(ErrorMessage = "Enter Name")]
public string Name { get; set; }

[RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
public string Mobileno { get; set; }

[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
    ErrorMessage = "Please enter a valid e-mail address")]
public string EmailID { get; set; }
``` 