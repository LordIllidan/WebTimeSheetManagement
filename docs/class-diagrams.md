# Class Diagrams - Diagramy Klas

## Przegląd

Ten dokument zawiera szczegółowe diagramy klas UML dla głównych komponentów aplikacji WebTimeSheetManagement, przedstawiające strukturę klas, ich właściwości, metody oraz relacje między nimi.

## 1. TimeSheet Domain Classes

Główne klasy związane z zarządzaniem kartami czasu pracy:

```plantuml
@startuml
title TimeSheet Domain Model

class TimeSheetMaster {
    +TimeSheetMasterID : int
    +FromDate : DateTime
    +ToDate : DateTime
    +UserID : int
    +CreatedOn : DateTime?
    +TimeSheetStatus : int
    +TotalHours : decimal?
    +SubmittedDate : DateTime?
    +ApprovedDate : DateTime?
    +Comment : string
}

class TimeSheetDetails {
    +TimeSheetDetailsID : int
    +TimeSheetMasterID : int
    +ProjectID : int
    +Period1 : int?
    +Period2 : int?
    +Period3 : int?
    +Period4 : int?
    +Period5 : int?
    +Period6 : int?
    +Period7 : int?
    +UserStoryDescription : string
    +TaskDescription : string
    +MainTaskDescription : string
}

class TimeSheetModel {
    +hdtext1 : DateTime
    +hdtext2 : DateTime
    +hdtext3 : DateTime
    +hdtext4 : DateTime
    +hdtext5 : DateTime
    +hdtext6 : DateTime
    +hdtext7 : DateTime
    +text1_p1 : int?
    +text2_p1 : int?
    +text3_p1 : int?
    +text4_p1 : int?
    +text5_p1 : int?
    +text6_p1 : int?
    +text7_p1 : int?
    +ProjectName : string
    +TimeSheetStatus : int
    +TotalHours : int?
}

class TimeSheetMasterView {
    +TimeSheetMasterID : int
    +FromDate : DateTime
    +ToDate : DateTime
    +CreatedOn : DateTime?
    +TimeSheetStatus : string
    +TotalHours : decimal?
}

class TimeSheetDetailsView {
    +TimeSheetDetailsID : int
    +ProjectName : string
    +DaysofWeek : string
    +Hours : int?
    +Period : DateTime
    +ProjectID : int
}

class TimeSheetAuditTB {
    +TimeSheetAuditID : int
    +TimeSheetID : int
    +Status : int
    +AuditDate : DateTime
    +Comment : string
    +UserID : int
    +ApprovalUserID : int?
}

TimeSheetMaster ||--o{ TimeSheetDetails : "Contains"
TimeSheetMaster ||--o{ TimeSheetAuditTB : "Audited"
TimeSheetModel ..> TimeSheetMaster : "Maps to"
TimeSheetMasterView ..> TimeSheetMaster : "View of"
TimeSheetDetailsView ..> TimeSheetDetails : "View of"

@enduml
```

## 2. Expense Domain Classes

Klasy związane z zarządzaniem wydatkami:

```plantuml
@startuml
title Expense Domain Model

class ExpenseModel {
    +ExpenseID : int
    +ProjectID : int?
    +PurposeorReason : string
    +ExpenseStatus : int
    +FromDate : DateTime?
    +ToDate : DateTime?
    +UserID : int
    +CreatedOn : DateTime?
    +VoucherID : string
    +Hotel : int?
    +Meal : int?
    +Transportation : int?
    +Phone : int?
    +Entertainment : int?
    +Fuel : int?
    +Maintenance : int?
    +Others : int?
    +TotalExpense : int?
    +Comment : string
}

class ExpenseModelView {
    +ExpenseID : int
    +ProjectName : string
    +PurposeorReason : string
    +ExpenseStatus : string
    +FromDate : DateTime?
    +ToDate : DateTime?
    +TotalExpense : int?
    +VoucherID : string
}

class ExpenseApprovalModel {
    +ExpenseID : int
    +UserID : int
    +Comment : string
    +ExpenseStatus : int
}

class ExpenseAuditTB {
    +ExpenseAuditID : int
    +ExpenseID : int
    +Status : int
    +AuditDate : DateTime
    +Comment : string
    +UserID : int
    +ApprovalUserID : int?
}

ExpenseModel ||--o{ ExpenseAuditTB : "Audited"
ExpenseModelView ..> ExpenseModel : "View of"
ExpenseApprovalModel ..> ExpenseModel : "Approval for"

@enduml
```

## 3. User Management Classes

Klasy związane z zarządzaniem użytkownikami:

```plantuml
@startuml
title User Management Model

class Registration {
    +RegistrationID : int
    +Name : string
    +Mobileno : string
    +EmailID : string
    +Username : string
    +Password : string
    +ConfirmPassword : string
    +Gender : string
    +Birthdate : DateTime?
    +DateofJoining : DateTime?
    +RoleID : int?
    +EmployeeID : string
    +CreatedOn : DateTime?
    +ForceChangePassword : int?
    --
    +IsValid() : bool
    +GetAge() : int
}

class Role {
    +RoleID : int
    +RoleName : string
}

class AssignedRoles {
    +AssignedRoleID : int
    +EmployeeID : string
    +RoleID : int
    +AssignRoleDate : DateTime?
    +Status : int
}

class LoginViewModel {
    +Username : string
    +Password : string
    +RememberMe : bool
    --
    +Validate() : bool
}

class ChangePasswordModel {
    +Username : string
    +OldPassword : string
    +NewPassword : string
    +ConfirmPassword : string
    --
    +ValidatePasswords() : bool
}

class DisplayViewModel {
    +TotalUser : int
    +TotalTimeSheet : int
    +TotalExpense : int
    +TotalProject : int
}

Registration }o--|| Role : "Has"
Registration ||--o{ AssignedRoles : "Assigned"
Role ||--o{ AssignedRoles : "In"

@enduml
```

## 4. Project Management Classes

Klasy związane z projektami:

```plantuml
@startuml
title Project Management Model

class ProjectMaster {
    +ProjectID : int
    +ProjectCode : string
    +NatureofIndustry : string
    +ProjectName : string
    --
    +IsCodeUnique() : bool
    +GetProjectInfo() : string
}

class ProjectViewModel {
    +ProjectID : int
    +ProjectCode : string
    +ProjectName : string
    +NatureofIndustry : string
    +IsActive : bool
    +CreatedDate : DateTime
    +AssignedUsers : List<string>
}

ProjectViewModel ..> ProjectMaster : "View of"

@enduml
```

## 5. Notification Classes

Klasy systemu powiadomień:

```plantuml
@startuml
title Notification Model

class NotificationsTB {
    +NotificationID : int
    +UserID : int
    +NotificationTitle : string
    +NotificationDetails : string
    +IsRead : bool
    +CreatedOn : DateTime
    +ReadOn : DateTime?
    +NotificationType : string
    +SourceID : int?
    +Priority : int
    --
    +MarkAsRead() : void
    +GetTimeAgo() : string
}

class NotificationViewModel {
    +NotificationID : int
    +Title : string
    +Details : string
    +IsRead : bool
    +TimeAgo : string
    +NotificationType : string
    +Priority : string
}

NotificationViewModel ..> NotificationsTB : "View of"

@enduml
```

## 6. Document Management Classes

Klasy zarządzania dokumentami:

```plantuml
@startuml
title Document Management Model

class Documents {
    +DocumentID : int
    +DocumentName : string
    +DocumentType : string
    +DocumentPath : string
    +UploadDate : DateTime
    +UserID : int
    +ExpenseID : int?
    +TimeSheetID : int?
    +FileSize : long
    +ContentType : string
    --
    +GetFileExtension() : string
    +ValidateFileType() : bool
}

class DocumentsVM {
    +DocumentID : int
    +DocumentName : string
    +DocumentType : string
    +FileSize : string
    +UploadDate : string
    +CanDelete : bool
}

DocumentsVM ..> Documents : "View of"

@enduml
```

## 7. Audit Classes

Klasy systemu audytu:

```plantuml
@startuml
title Audit Model

class AuditTB {
    +AuditID : int
    +UserID : int
    +SessionID : string
    +IPAddress : string
    +PageAccessed : string
    +LoggedInAt : DateTime
    +LoggedOutAt : DateTime?
    +UserAgent : string
    +ActionPerformed : string
    --
    +GetSessionDuration() : TimeSpan
    +IsActiveSession() : bool
}

class DescriptionTB {
    +DescriptionID : int
    +TimeSheetMasterID : int
    +ProjectID : int
    +Description : string
    +DaysofWeek : string
    +CreatedOn : DateTime
    +UserID : int
}

AuditTB }o-- Registration : "Tracks"
DescriptionTB }o-- TimeSheetMaster : "Describes"
DescriptionTB }o-- ProjectMaster : "For"

@enduml
```

## 8. Business Logic Interfaces

Interfejsy warstwy biznesowej:

```plantuml
@startuml
title Business Logic Interfaces

interface ITimeSheet {
    +AddTimeSheetMaster(TimeSheetMaster) : int
    +AddTimeSheetDetail(TimeSheetDetails) : int
    +CheckIsDateAlreadyUsed(DateTime, int) : bool
    +ShowTimeSheet(string, string, string, int) : IQueryable<TimeSheetMasterView>
    +UpdateTimeSheetStatus(TimeSheetApproval, int) : bool
    +DeleteTimesheetByTimeSheetMasterID(int, int) : int
    +InsertTimeSheetAuditLog(TimeSheetAuditTB) : void
}

interface IExpense {
    +AddExpense(ExpenseModel) : int
    +UpdateExpenseStatus(ExpenseApprovalModel, int) : bool
    +ShowExpenses(string, string, string, int) : IQueryable<ExpenseModelView>
    +DeleteExpense(int, int) : int
    +GetExpensesByDateRange(DateTime, DateTime, int) : List<ExpenseModel>
}

interface IUsers {
    +GetAllUsers() : IQueryable<Registration>
    +GetUserById(int) : Registration
    +UpdateUser(Registration) : bool
    +DeleteUser(int) : bool
    +ChangePassword(ChangePasswordModel) : bool
    +GetUsersByRole(int) : List<Registration>
}

interface IRegistration {
    +ValidateUserName(string) : bool
    +SaveUser(Registration) : int
    +IsEmailAlreadyExists(string) : bool
    +IsUsernameExists(string) : bool
}

interface ILogin {
    +ValidateUser(string, string) : int
    +GetUserByCredentials(string, string) : Registration
    +UpdateLastLogin(int) : bool
}

interface IProject {
    +GetAllProject() : IQueryable<ProjectMaster>
    +GetProjectById(int) : ProjectMaster
    +SaveProject(ProjectMaster) : int
    +UpdateProject(ProjectMaster) : bool
    +DeleteProject(int) : bool
    +IsProjectCodeExists(string) : bool
}

@enduml
```

## 9. Repository Implementation Classes

Implementacje interfejsów:

```plantuml
@startuml
title Repository Implementation

class TimeSheetConcrete {
    -context : DatabaseContext
    --
    +AddTimeSheetMaster(TimeSheetMaster) : int
    +AddTimeSheetDetail(TimeSheetDetails) : int
    +CheckIsDateAlreadyUsed(DateTime, int) : bool
    +ShowTimeSheet(string, string, string, int) : IQueryable<TimeSheetMasterView>
    +UpdateTimeSheetStatus(TimeSheetApproval, int) : bool
    +ValidateTimeSheetData(TimeSheetModel) : bool
    +CalculateWeeklyHours(TimeSheetDetails) : int
    -SendNotificationToManager(int) : void
}

class ExpenseConcrete {
    -context : DatabaseContext
    --
    +AddExpense(ExpenseModel) : int
    +UpdateExpenseStatus(ExpenseApprovalModel, int) : bool
    +ShowExpenses(string, string, string, int) : IQueryable<ExpenseModelView>
    +ValidateExpenseData(ExpenseModel) : bool
    +CalculateTotalExpense(ExpenseModel) : int
    -SendApprovalNotification(int) : void
}

class UsersConcrete {
    -context : DatabaseContext
    --
    +GetAllUsers() : IQueryable<Registration>
    +GetUserById(int) : Registration
    +UpdateUser(Registration) : bool
    +HashPassword(string) : string
    +ValidatePassword(string, string) : bool
    -SendWelcomeEmail(Registration) : void
}

class RegistrationConcrete {
    -context : DatabaseContext
    --
    +ValidateUserName(string) : bool
    +SaveUser(Registration) : int
    +IsEmailAlreadyExists(string) : bool
    +HashPasswordForStorage(string) : string
    -GenerateEmployeeID() : string
}

ITimeSheet <|.. TimeSheetConcrete
IExpense <|.. ExpenseConcrete
IUsers <|.. UsersConcrete
IRegistration <|.. RegistrationConcrete

TimeSheetConcrete --> DatabaseContext
ExpenseConcrete --> DatabaseContext
UsersConcrete --> DatabaseContext
RegistrationConcrete --> DatabaseContext

@enduml
```

## 10. Database Context

Kontekst Entity Framework:

```plantuml
@startuml
title Database Context

class DatabaseContext {
    +Registration : DbSet<Registration>
    +Role : DbSet<Roles>
    +ProjectMaster : DbSet<ProjectMaster>
    +TimeSheetMaster : DbSet<TimeSheetMaster>
    +TimeSheetDetails : DbSet<TimeSheetDetails>
    +ExpenseModel : DbSet<ExpenseModel>
    +Documents : DbSet<Documents>
    +TimeSheetAuditTB : DbSet<TimeSheetAuditTB>
    +ExpenseAuditTB : DbSet<ExpenseAuditTB>
    +AuditTB : DbSet<AuditTB>
    +DescriptionTB : DbSet<DescriptionTB>
    +AssignedRoles : DbSet<AssignedRoles>
    +NotificationsTBs : DbSet<NotificationsTB>
    --
    +SaveChanges() : int
    +SaveChangesAsync() : Task<int>
    #OnModelCreating(DbModelBuilder) : void
    +Dispose() : void
}

DatabaseContext --> Registration
DatabaseContext --> ProjectMaster
DatabaseContext --> TimeSheetMaster
DatabaseContext --> TimeSheetDetails
DatabaseContext --> ExpenseModel
DatabaseContext --> Documents
DatabaseContext --> AuditTB
DatabaseContext --> NotificationsTB

@enduml
```

## 11. Controller Classes

Główne kontrolery MVC:

```plantuml
@startuml
title Controller Classes

abstract class BaseController {
    #GetCurrentUser() : Registration
    #LogUserActivity(string) : void
    #ShowSuccessMessage(string) : void
    #ShowErrorMessage(string) : void
    #RedirectToLogin() : ActionResult
}

class TimeSheetController {
    -timeSheetService : ITimeSheet
    -projectService : IProject
    -auditService : IAudit
    --
    +Index() : ActionResult
    +Create() : ActionResult
    +Create(TimeSheetModel) : ActionResult
    +Edit(int) : ActionResult
    +Edit(TimeSheetModel) : ActionResult
    +Delete(int) : ActionResult
    +Submit(int) : ActionResult
    +Approve(int) : ActionResult
    +Reject(int, string) : ActionResult
    +Export(string) : ActionResult
    -ValidateModel(TimeSheetModel) : bool
    -SendNotifications(int, string) : void
}

class ExpenseController {
    -expenseService : IExpense
    -projectService : IProject
    --
    +Index() : ActionResult
    +Create() : ActionResult
    +Create(ExpenseModel) : ActionResult
    +Edit(int) : ActionResult
    +Approve(int) : ActionResult
    +Reject(int, string) : ActionResult
    +Details(int) : ActionResult
    -CalculateTotal(ExpenseModel) : void
}

class UserController {
    -userService : IUsers
    --
    +Profile() : ActionResult
    +Profile(Registration) : ActionResult
    +ChangePassword() : ActionResult
    +ChangePassword(ChangePasswordModel) : ActionResult
    +Dashboard() : ActionResult
    -UpdateUserProfile(Registration) : bool
}

class AdminController {
    -userService : IUsers
    -projectService : IProject
    -auditService : IAudit
    --
    +Index() : ActionResult
    +Users() : ActionResult
    +Projects() : ActionResult
    +Reports() : ActionResult
    +SystemSettings() : ActionResult
    -HasAdminRights() : bool
}

BaseController <|-- TimeSheetController
BaseController <|-- ExpenseController
BaseController <|-- UserController
BaseController <|-- AdminController

@enduml
```

## Relacje między Klasami

Główne relacje w systemie:

```plantuml
@startuml
title Class Relationships Overview

class Registration
class TimeSheetMaster
class TimeSheetDetails
class ExpenseModel
class ProjectMaster
class AuditTB
class NotificationsTB

Registration ||--o{ TimeSheetMaster : "Creates"
Registration ||--o{ ExpenseModel : "Submits"
Registration ||--o{ AuditTB : "Tracked by"
Registration ||--o{ NotificationsTB : "Receives"

ProjectMaster ||--o{ TimeSheetDetails : "Worked on"
ProjectMaster ||--o{ ExpenseModel : "Expenses for"

TimeSheetMaster ||--o{ TimeSheetDetails : "Contains"
TimeSheetMaster ||--o{ TimeSheetAuditTB : "Audited"

ExpenseModel ||--o{ ExpenseAuditTB : "Audited"

@enduml
``` 