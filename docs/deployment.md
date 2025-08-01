# Deployment Guide - Instrukcje Wdrożenia

## Spis Treści

1. [Wymagania Systemowe](#wymagania-systemowe)
2. [Przygotowanie Środowiska](#przygotowanie-środowiska)
3. [Konfiguracja Bazy Danych](#konfiguracja-bazy-danych)
4. [Wdrożenie Aplikacji](#wdrożenie-aplikacji)
5. [Konfiguracja IIS](#konfiguracja-iis)
6. [Konfiguracja Bezpieczeństwa](#konfiguracja-bezpieczeństwa)
7. [Monitoring i Logowanie](#monitoring-i-logowanie)
8. [Backup i Recovery](#backup-i-recovery)
9. [Rozwiązywanie Problemów](#rozwiązywanie-problemów)

## Wymagania Systemowe

### Minimalne Wymagania

**Serwer Aplikacji:**
- Windows Server 2012 R2 lub nowszy
- IIS 8.0 lub nowszy
- .NET Framework 4.5.2 lub nowszy
- 4 GB RAM (minimum)
- 10 GB wolnego miejsca na dysku
- Procesor dual-core 2.4 GHz

**Serwer Bazy Danych:**
- SQL Server 2012 lub nowszy (Express, Standard, Enterprise)
- 4 GB RAM (minimum)
- 20 GB wolnego miejsca na dysku
- Procesor dual-core 2.4 GHz

**Klient (Przeglądarka):**
- Internet Explorer 11+
- Chrome 60+
- Firefox 55+
- Edge 40+
- Safari 11+

### Zalecane Wymagania

**Serwer Aplikacji:**
- Windows Server 2019 lub nowszy
- IIS 10.0
- .NET Framework 4.8
- 8 GB RAM
- 50 GB wolnego miejsca na dysku (SSD)
- Procesor quad-core 3.0 GHz

**Serwer Bazy Danych:**
- SQL Server 2019 Standard/Enterprise
- 16 GB RAM
- 100 GB wolnego miejsca na dysku (SSD)
- Procesor octa-core 3.0 GHz

## Przygotowanie Środowiska

### 1. Instalacja Prerequisites

#### Na Serwerze Aplikacji

```powershell
# Włączenie IIS i ASP.NET
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServer
Enable-WindowsOptionalFeature -Online -FeatureName IIS-CommonHttpFeatures
Enable-WindowsOptionalFeature -Online -FeatureName IIS-NetFxExtensibility45
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ISAPIExtensions
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ISAPIFilter
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ASPNET45

# Instalacja .NET Framework 4.8
# Pobierz i zainstaluj z Microsoft Download Center
```

#### Instalacja URL Rewrite Module

```powershell
# Pobierz URL Rewrite Module 2.1 z Microsoft
# https://www.microsoft.com/en-us/download/details.aspx?id=47337
```

#### Instalacja Application Request Routing (ARR)

```powershell
# Pobierz ARR 3.0 z Microsoft
# https://www.microsoft.com/en-us/download/details.aspx?id=47333
```

### 2. Tworzenie Użytkowników Systemowych

```powershell
# Tworzenie użytkownika dla App Pool
New-LocalUser -Name "WebTimeSheetAppPool" -Password (ConvertTo-SecureString "P@ssw0rd123!" -AsPlainText -Force) -PasswordNeverExpires
Add-LocalGroupMember -Group "IIS_IUSRS" -Member "WebTimeSheetAppPool"

# Tworzenie użytkownika dla SQL Server
# Wykonaj w SQL Server Management Studio
```

### 3. Konfiguracja Folderów

```powershell
# Tworzenie struktury folderów
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\WebTimeSheetManagement"
New-Item -ItemType Directory -Path "C:\WebTimeSheetLogs"
New-Item -ItemType Directory -Path "C:\WebTimeSheetBackups"
New-Item -ItemType Directory -Path "C:\WebTimeSheetDocuments"

# Ustawienie uprawnień
icacls "C:\inetpub\wwwroot\WebTimeSheetManagement" /grant "WebTimeSheetAppPool:(OI)(CI)F"
icacls "C:\WebTimeSheetLogs" /grant "WebTimeSheetAppPool:(OI)(CI)F"
icacls "C:\WebTimeSheetDocuments" /grant "WebTimeSheetAppPool:(OI)(CI)F"
```

## Konfiguracja Bazy Danych

### 1. Instalacja SQL Server

```sql
-- Konfiguracja SQL Server
-- 1. Zainstaluj SQL Server z następującymi komponentami:
--    - Database Engine Services
--    - SQL Server Replication
--    - Client Tools Connectivity
--    - Integration Services

-- 2. Konfiguracja instancji
--    - Mixed Mode Authentication
--    - Ustaw hasło dla konta 'sa'
--    - Dodaj administratorów Windows
```

### 2. Tworzenie Bazy Danych

```sql
-- Tworzenie bazy danych
CREATE DATABASE WebTimeSheetDB
ON (
    NAME = 'WebTimeSheetDB',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\WebTimeSheetDB.mdf',
    SIZE = 100MB,
    MAXSIZE = 1GB,
    FILEGROWTH = 10MB
)
LOG ON (
    NAME = 'WebTimeSheetDB_Log',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\WebTimeSheetDB_Log.ldf',
    SIZE = 10MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 5MB
);

-- Tworzenie użytkownika aplikacji
CREATE LOGIN WebTimeSheetUser WITH PASSWORD = 'P@ssw0rd123!';
USE WebTimeSheetDB;
CREATE USER WebTimeSheetUser FOR LOGIN WebTimeSheetUser;
ALTER ROLE db_datareader ADD MEMBER WebTimeSheetUser;
ALTER ROLE db_datawriter ADD MEMBER WebTimeSheetUser;
ALTER ROLE db_ddladmin ADD MEMBER WebTimeSheetUser;
```

### 3. Wykonanie Skryptów Bazy Danych

```sql
-- 1. Wykonaj skrypty w kolejności:
--    a) Schema creation scripts
--    b) Initial data scripts
--    c) Stored procedures
--    d) Views
--    e) Indexes

-- Przykład tworzenia tabel (fragment)
CREATE TABLE Registration (
    RegistrationID int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Username nvarchar(50) NOT NULL UNIQUE,
    Password nvarchar(500) NOT NULL,
    EmailID nvarchar(100) NOT NULL UNIQUE,
    -- ... inne kolumny
    CreatedOn datetime DEFAULT GETDATE()
);

-- Wstawianie danych początkowych
INSERT INTO Roles (RoleName) VALUES ('Employee'), ('Manager'), ('Admin'), ('SuperAdmin');

-- Tworzenie domyślnego użytkownika admin
INSERT INTO Registration (Name, Username, Password, EmailID, RoleID, CreatedOn)
VALUES ('System Administrator', 'admin', 'AHashedPassword', 'admin@company.com', 4, GETDATE());
```

### 4. Connection String

```xml
<!-- Web.config connection string -->
<connectionStrings>
  <add name="TimesheetDBEntities" 
       connectionString="Data Source=SERVER_NAME;Initial Catalog=WebTimeSheetDB;User ID=WebTimeSheetUser;Password=P@ssw0rd123!;MultipleActiveResultSets=True;Application Name=WebTimeSheetManagement" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Wdrożenie Aplikacji

### 1. Publikowanie z Visual Studio

```xml
<!-- Publish Profile (.pubxml) -->
<Project>
  <PropertyGroup>
    <PublishUrl>C:\inetpub\wwwroot\WebTimeSheetManagement</PublishUrl>
    <WebPublishMethod>FileSystem</WebPublishMethod>
    <PublishProvider>FileSystem</PublishProvider>
    <LastUsedBuildConfiguration>Release</LastUsedBuildConfiguration>
    <LastUsedPlatform>Any CPU</LastUsedPlatform>
    <SiteUrlToLaunchAfterPublish />
    <LaunchSiteAfterPublish>True</LaunchSiteAfterPublish>
    <ExcludeApp_Data>False</ExcludeApp_Data>
    <ProjectGuid>guid-here</ProjectGuid>
    <publishUrl>C:\inetpub\wwwroot\WebTimeSheetManagement</publishUrl>
    <DeleteExistingFiles>False</DeleteExistingFiles>
  </PropertyGroup>
</Project>
```

### 2. Manual Deployment

```powershell
# Kopiowanie plików aplikacji
Copy-Item -Path "C:\Source\WebTimeSheetManagement\*" -Destination "C:\inetpub\wwwroot\WebTimeSheetManagement" -Recurse -Force

# Kopiowanie pakietów NuGet do bin
Copy-Item -Path "C:\Source\packages\*" -Destination "C:\inetpub\wwwroot\WebTimeSheetManagement\bin" -Recurse -Force
```

### 3. Konfiguracja Web.config

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  
  <!-- Connection Strings -->
  <connectionStrings>
    <add name="TimesheetDBEntities" 
         connectionString="Data Source=SQL_SERVER_NAME;Initial Catalog=WebTimeSheetDB;User ID=WebTimeSheetUser;Password=P@ssw0rd123!;MultipleActiveResultSets=True" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  <!-- App Settings -->
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    
    <!-- Email Configuration -->
    <add key="SMTPServer" value="smtp.company.com" />
    <add key="SMTPPort" value="587" />
    <add key="SMTPUsername" value="noreply@company.com" />
    <add key="SMTPPassword" value="EmailPassword123!" />
    <add key="SMTPEnableSSL" value="true" />
    
    <!-- File Upload Settings -->
    <add key="MaxFileSize" value="5242880" /> <!-- 5MB -->
    <add key="DocumentsPath" value="C:\WebTimeSheetDocuments\" />
    
    <!-- Security Settings -->
    <add key="PasswordMinLength" value="7" />
    <add key="SessionTimeout" value="30" />
    <add key="ForceHTTPS" value="true" />
  </appSettings>
  
  <!-- System.Web Configuration -->
  <system.web>
    <compilation targetFramework="4.8" />
    <httpRuntime targetFramework="4.8" maxRequestLength="5120" executionTimeout="600" />
    
    <!-- Session Configuration -->
    <sessionState mode="InProc" timeout="30" cookieless="false" cookieName="WebTimeSheetSession" />
    
    <!-- Authentication -->
    <authentication mode="Forms">
      <forms loginUrl="~/Login" timeout="30" name="WebTimeSheetAuth" />
    </authentication>
    
    <!-- Custom Errors -->
    <customErrors mode="RemoteOnly" defaultRedirect="~/Error">
      <error statusCode="404" redirect="~/Error/NotFound" />
      <error statusCode="500" redirect="~/Error/ServerError" />
    </customErrors>
    
    <!-- Trust Level -->
    <trust level="Full" />
  </system.web>
  
  <!-- Entity Framework -->
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
      <parameters>
        <parameter value="mssqllocaldb" />
      </parameters>
    </defaultConnectionFactory>
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
    </providers>
  </entityFramework>
  
  <!-- Elmah Configuration -->
  <elmah>
    <security allowRemoteAccess="false" />
    <errorLog type="Elmah.SqlErrorLog, Elmah" connectionStringName="TimesheetDBEntities" />
  </elmah>
</configuration>
```

## Konfiguracja IIS

### 1. Tworzenie Application Pool

```powershell
# Tworzenie Application Pool
Import-Module WebAdministration
New-WebAppPool -Name "WebTimeSheetManagement"

# Konfiguracja Application Pool
Set-ItemProperty -Path "IIS:\AppPools\WebTimeSheetManagement" -Name "processModel.identityType" -Value "SpecificUser"
Set-ItemProperty -Path "IIS:\AppPools\WebTimeSheetManagement" -Name "processModel.userName" -Value "WebTimeSheetAppPool"
Set-ItemProperty -Path "IIS:\AppPools\WebTimeSheetManagement" -Name "processModel.password" -Value "P@ssw0rd123!"
Set-ItemProperty -Path "IIS:\AppPools\WebTimeSheetManagement" -Name "managedRuntimeVersion" -Value "v4.0"
Set-ItemProperty -Path "IIS:\AppPools\WebTimeSheetManagement" -Name "enable32BitAppOnWin64" -Value $false
Set-ItemProperty -Path "IIS:\AppPools\WebTimeSheetManagement" -Name "recycling.periodicRestart.time" -Value "02:00:00"
```

### 2. Tworzenie Website

```powershell
# Tworzenie Website
New-Website -Name "WebTimeSheetManagement" -Port 80 -PhysicalPath "C:\inetpub\wwwroot\WebTimeSheetManagement" -ApplicationPool "WebTimeSheetManagement"

# Konfiguracja dodatkowych binding'ów (HTTPS)
New-WebBinding -Name "WebTimeSheetManagement" -Protocol "https" -Port 443 -SslFlags 0
```

### 3. Konfiguracja SSL Certificate

```powershell
# Importowanie certyfikatu SSL
$cert = Import-PfxCertificate -FilePath "C:\Certificates\webtimesheet.pfx" -CertStoreLocation "Cert:\LocalMachine\My" -Password (ConvertTo-SecureString "CertPassword" -AsPlainText -Force)

# Przypisanie certyfikatu do witryny
$binding = Get-WebBinding -Name "WebTimeSheetManagement" -Protocol "https"
$binding.AddSslCertificate($cert.Thumbprint, "My")
```

### 4. Konfiguracja URL Rewrite (HTTPS Redirect)

```xml
<!-- web.config - URL Rewrite rules -->
<system.webServer>
  <rewrite>
    <rules>
      <rule name="Redirect to HTTPS" stopProcessing="true">
        <match url="." />
        <conditions>
          <add input="{HTTPS}" pattern="^OFF$" ignoreCase="true" />
        </conditions>
        <action type="Redirect" url="https://{HTTP_HOST}/{R:0}" redirectType="Permanent" />
      </rule>
    </rules>
  </rewrite>
</system.webServer>
```

### 5. MIME Types i Handler Mappings

```xml
<!-- web.config - dodatkowe konfiguracje -->
<system.webServer>
  <staticContent>
    <mimeMap fileExtension=".json" mimeType="application/json" />
    <mimeMap fileExtension=".woff" mimeType="application/font-woff" />
    <mimeMap fileExtension=".woff2" mimeType="font/woff2" />
  </staticContent>
  
  <handlers>
    <remove name="ExtensionlessUrlHandler-Integrated-4.0" />
    <add name="ExtensionlessUrlHandler-Integrated-4.0" path="*." verb="*" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
  </handlers>
  
  <defaultDocument>
    <files>
      <clear />
      <add value="Default.aspx" />
      <add value="index.html" />
    </files>
  </defaultDocument>
</system.webServer>
```

## Konfiguracja Bezpieczeństwa

### 1. Firewall Settings

```powershell
# Otwieranie portów w Windows Firewall
New-NetFirewallRule -DisplayName "HTTP Inbound" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow
New-NetFirewallRule -DisplayName "HTTPS Inbound" -Direction Inbound -Protocol TCP -LocalPort 443 -Action Allow
New-NetFirewallRule -DisplayName "SQL Server" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
```

### 2. SQL Server Security

```sql
-- Konfiguracja SQL Server Security
-- 1. Wyłącz SA account jeśli nie jest używany
ALTER LOGIN sa DISABLE;

-- 2. Konfiguruj SQL Server dla Windows Authentication Only (jeśli możliwe)
-- Wykonaj przez SQL Server Configuration Manager

-- 3. Konfiguruj SQL Server Firewall
-- Windows Firewall -> SQL Server Configuration Manager

-- 4. Ustaw odpowiednie uprawnienia na folderach SQL Server
```

### 3. Application Security

```xml
<!-- web.config - Security Headers -->
<system.webServer>
  <httpProtocol>
    <customHeaders>
      <add name="X-Frame-Options" value="DENY" />
      <add name="X-Content-Type-Options" value="nosniff" />
      <add name="X-XSS-Protection" value="1; mode=block" />
      <add name="Strict-Transport-Security" value="max-age=31536000; includeSubDomains" />
      <add name="Content-Security-Policy" value="default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'" />
    </customHeaders>
  </httpProtocol>
  
  <security>
    <requestFiltering>
      <requestLimits maxAllowedContentLength="5242880" /> <!-- 5MB -->
      <fileExtensions>
        <remove fileExtension=".config" />
        <add fileExtension=".config" allowed="false" />
      </fileExtensions>
    </requestFiltering>
  </security>
</system.webServer>
```

### 4. Folder Permissions

```powershell
# Ustaw minimalne uprawnienia dla folderów aplikacji
icacls "C:\inetpub\wwwroot\WebTimeSheetManagement" /inheritance:r
icacls "C:\inetpub\wwwroot\WebTimeSheetManagement" /grant "IIS_IUSRS:(OI)(CI)RX"
icacls "C:\inetpub\wwwroot\WebTimeSheetManagement" /grant "WebTimeSheetAppPool:(OI)(CI)RX"
icacls "C:\inetpub\wwwroot\WebTimeSheetManagement\App_Data" /grant "WebTimeSheetAppPool:(OI)(CI)F"
```

## Monitoring i Logowanie

### 1. IIS Logging

```powershell
# Konfiguracja IIS Logging
Set-WebConfiguration -Filter "system.webServer/httpLogging" -Value @{dontLog=$false} -PSPath "IIS:\Sites\WebTimeSheetManagement"
Set-WebConfiguration -Filter "system.webServer/httpLogging" -Value @{directory="C:\WebTimeSheetLogs\IIS"} -PSPath "IIS:\Sites\WebTimeSheetManagement"
```

### 2. Application Insights (opcjonalnie)

```xml
<!-- ApplicationInsights.config -->
<?xml version="1.0" encoding="utf-8"?>
<ApplicationInsights xmlns="http://schemas.microsoft.com/ApplicationInsights/2013/Settings">
  <InstrumentationKey>YOUR_INSTRUMENTATION_KEY</InstrumentationKey>
  <TelemetryModules>
    <Add Type="Microsoft.ApplicationInsights.DependencyCollector.DependencyTrackingTelemetryModule, Microsoft.AI.DependencyCollector"/>
    <Add Type="Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.PerformanceCollectorModule, Microsoft.AI.PerfCounterCollector"/>
    <Add Type="Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.AdaptiveSamplingTelemetryProcessor, Microsoft.AI.ServerTelemetryChannel"/>
  </TelemetryModules>
</ApplicationInsights>
```

### 3. Elmah Configuration

```xml
<!-- web.config - Elmah dla error logging -->
<configSections>
  <sectionGroup name="elmah">
    <section name="security" requirePermission="false" type="Elmah.SecuritySectionHandler, Elmah" />
    <section name="errorLog" requirePermission="false" type="Elmah.ErrorLogSectionHandler, Elmah" />
  </sectionGroup>
</configSections>

<elmah>
  <security allowRemoteAccess="true" />
  <errorLog type="Elmah.SqlErrorLog, Elmah" connectionStringName="TimesheetDBEntities" />
</elmah>

<system.web>
  <httpModules>
    <add name="ErrorLog" type="Elmah.ErrorLogModule, Elmah" />
    <add name="ErrorMail" type="Elmah.ErrorMailModule, Elmah" />
    <add name="ErrorFilter" type="Elmah.ErrorFilterModule, Elmah" />
  </httpModules>
</system.web>

<system.webServer>
  <modules>
    <add name="ErrorLog" type="Elmah.ErrorLogModule, Elmah" preCondition="managedHandler" />
    <add name="ErrorMail" type="Elmah.ErrorMailModule, Elmah" preCondition="managedHandler" />
    <add name="ErrorFilter" type="Elmah.ErrorFilterModule, Elmah" preCondition="managedHandler" />
  </modules>
</system.webServer>
```

### 4. Performance Monitoring

```powershell
# Monitoring Performance Counters
$counters = @(
    "\Processor(_Total)\% Processor Time"
    "\Memory\Available MBytes"
    "\ASP.NET\Requests/Sec"
    "\ASP.NET\Request Execution Time"
    "\.NET CLR Memory(_Global_)\% Time in GC"
)

# Tworzenie Data Collector Set
logman create counter WebTimeSheetPerfMon -f csv -o "C:\WebTimeSheetLogs\Performance\perf.csv" -c $counters -si 00:00:30
```

## Backup i Recovery

### 1. Database Backup

```sql
-- Backup Strategy
-- Full backup codziennie o 2:00
USE master;
GO
EXEC sp_add_job
    @job_name = 'WebTimeSheetDB Full Backup',
    @enabled = 1;

EXEC sp_add_jobstep
    @job_name = 'WebTimeSheetDB Full Backup',
    @step_name = 'Full Backup',
    @command = 'BACKUP DATABASE WebTimeSheetDB TO DISK = ''C:\WebTimeSheetBackups\WebTimeSheetDB_Full_'' + CONVERT(VARCHAR, GETDATE(), 112) + ''.bak'' WITH COMPRESSION, CHECKSUM;';

EXEC sp_add_schedule
    @schedule_name = 'Daily 2AM',
    @freq_type = 4,
    @freq_interval = 1,
    @active_start_time = 020000;

EXEC sp_attach_schedule
    @job_name = 'WebTimeSheetDB Full Backup',
    @schedule_name = 'Daily 2AM';

EXEC sp_add_jobserver
    @job_name = 'WebTimeSheetDB Full Backup';

-- Transaction Log backup co 15 minut
EXEC sp_add_job
    @job_name = 'WebTimeSheetDB Log Backup',
    @enabled = 1;

EXEC sp_add_jobstep
    @job_name = 'WebTimeSheetDB Log Backup',
    @step_name = 'Log Backup',
    @command = 'BACKUP LOG WebTimeSheetDB TO DISK = ''C:\WebTimeSheetBackups\WebTimeSheetDB_Log_'' + CONVERT(VARCHAR, GETDATE(), 112) + ''_'' + REPLACE(CONVERT(VARCHAR, GETDATE(), 108), '':'', '''') + ''.trn'' WITH COMPRESSION, CHECKSUM;';

EXEC sp_add_schedule
    @schedule_name = 'Every 15 Minutes',
    @freq_type = 4,
    @freq_interval = 1,
    @freq_subday_type = 4,
    @freq_subday_interval = 15;
```

### 2. Application Files Backup

```powershell
# PowerShell script for application backup
$date = Get-Date -Format "yyyyMMdd_HHmmss"
$backupPath = "C:\WebTimeSheetBackups\Application\$date"

# Tworzenie folderu backup
New-Item -ItemType Directory -Path $backupPath -Force

# Kopiowanie plików aplikacji
Copy-Item -Path "C:\inetpub\wwwroot\WebTimeSheetManagement\*" -Destination "$backupPath\Application" -Recurse -Force

# Kopiowanie dokumentów
Copy-Item -Path "C:\WebTimeSheetDocuments\*" -Destination "$backupPath\Documents" -Recurse -Force

# Kompresja backup
Add-Type -Assembly "System.IO.Compression.FileSystem"
[System.IO.Compression.ZipFile]::CreateFromDirectory($backupPath, "$backupPath.zip")
Remove-Item -Path $backupPath -Recurse -Force

# Usuwanie starych backupów (starsze niż 30 dni)
Get-ChildItem "C:\WebTimeSheetBackups\Application\" -Filter "*.zip" | Where-Object {$_.LastWriteTime -lt (Get-Date).AddDays(-30)} | Remove-Item -Force
```

### 3. Disaster Recovery Plan

```powershell
# Skrypt odtwarzania aplikacji
param(
    [string]$BackupDate,
    [string]$DatabaseBackupPath,
    [string]$ApplicationBackupPath
)

# 1. Odtwarzanie bazy danych
sqlcmd -S "SQL_SERVER_NAME" -Q "RESTORE DATABASE WebTimeSheetDB FROM DISK = '$DatabaseBackupPath' WITH REPLACE"

# 2. Odtwarzanie plików aplikacji
Remove-Item -Path "C:\inetpub\wwwroot\WebTimeSheetManagement\*" -Recurse -Force
Expand-Archive -Path $ApplicationBackupPath -DestinationPath "C:\Temp\Restore"
Copy-Item -Path "C:\Temp\Restore\Application\*" -Destination "C:\inetpub\wwwroot\WebTimeSheetManagement" -Recurse -Force
Copy-Item -Path "C:\Temp\Restore\Documents\*" -Destination "C:\WebTimeSheetDocuments" -Recurse -Force

# 3. Restart IIS
iisreset

Write-Host "Recovery completed successfully"
```

## Rozwiązywanie Problemów

### Typowe Problemy i Rozwiązania

#### Problem 1: HTTP Error 500.19

**Symptom**: Internal Server Error z kodem 0x80070021
**Przyczyna**: Brakujące moduły IIS lub błędna konfiguracja web.config
**Rozwiązanie**:
```powershell
# Sprawdź czy są zainstalowane odpowiednie moduły IIS
Get-WindowsOptionalFeature -Online -FeatureName "IIS-ASPNET45"

# Zainstaluj brakujące moduły
Enable-WindowsOptionalFeature -Online -FeatureName "IIS-ASPNET45"
```

#### Problem 2: Database Connection Issues

**Symptom**: Nie można połączyć się z bazą danych
**Przyczyna**: Błędny connection string lub uprawnienia
**Rozwiązanie**:
```sql
-- Sprawdź connection string w web.config
-- Sprawdź czy użytkownik ma odpowiednie uprawnienia
SELECT name, create_date FROM sys.databases WHERE name = 'WebTimeSheetDB';
SELECT name FROM sys.database_principals WHERE name = 'WebTimeSheetUser';
```

#### Problem 3: Slow Performance

**Symptom**: Aplikacja działa wolno
**Przyczyna**: Brak indeksów, niewłaściwa konfiguracja IIS
**Rozwiązanie**:
```sql
-- Sprawdź brakujące indeksy
SELECT 
    dm_mid.database_id,
    dm_mid.object_id,
    dm_mid.index_handle,
    dm_migs.user_seeks,
    dm_migs.user_scans,
    dm_mid.statement AS TableName,
    dm_mid.equality_columns,
    dm_mid.inequality_columns,
    dm_mid.included_columns
FROM sys.dm_db_missing_index_details dm_mid
CROSS APPLY sys.dm_db_missing_index_groups dm_mig
CROSS APPLY sys.dm_db_missing_index_group_stats dm_migs
WHERE dm_mid.index_handle = dm_mig.index_handle
AND dm_mig.index_group_handle = dm_migs.group_handle
ORDER BY dm_migs.user_seeks DESC;
```

#### Problem 4: SignalR Connection Issues

**Symptom**: Real-time notifications nie działają
**Przyczyna**: Problemy z WebSocket lub konfiguracja proxy
**Rozwiązanie**:
```xml
<!-- web.config - włącz WebSocket -->
<system.webServer>
  <webSocket enabled="true" />
</system.webServer>
```

### Narzędzia Diagnostyczne

#### 1. Failed Request Tracing

```powershell
# Włączenie Failed Request Tracing
Set-WebConfiguration -Filter "system.webServer/tracing/traceFailedRequests" -Value @{enabled=$true} -PSPath "IIS:\Sites\WebTimeSheetManagement"
```

#### 2. Process Monitor

```powershell
# Używaj Process Monitor do śledzenia dostępu do plików
# https://docs.microsoft.com/en-us/sysinternals/downloads/procmon
```

#### 3. SQL Server Profiler

```sql
-- Użyj SQL Server Profiler lub Extended Events do monitorowania SQL
CREATE EVENT SESSION WebTimeSheetTrace ON SERVER
ADD EVENT sqlserver.sql_statement_completed,
ADD EVENT sqlserver.rpc_completed
ADD TARGET package0.event_file(SET filename=N'C:\WebTimeSheetLogs\SQLTrace.xel')
WITH (MAX_MEMORY=4096 KB, EVENT_RETENTION_MODE=ALLOW_SINGLE_EVENT_LOSS, MAX_DISPATCH_LATENCY=30 SECONDS, MAX_EVENT_SIZE=0 KB, MEMORY_PARTITION_MODE=NONE, TRACK_CAUSALITY=ON, STARTUP_STATE=OFF);

ALTER EVENT SESSION WebTimeSheetTrace ON SERVER STATE = START;
```

### Health Check Script

```powershell
# Skrypt sprawdzający stan aplikacji
function Test-WebTimeSheetHealth {
    $results = @{}
    
    # 1. Test IIS
    try {
        $site = Get-Website -Name "WebTimeSheetManagement"
        $results["IIS"] = if ($site.State -eq "Started") { "OK" } else { "FAILED: Site not started" }
    } catch {
        $results["IIS"] = "FAILED: $($_.Exception.Message)"
    }
    
    # 2. Test Database
    try {
        $connectionString = "Data Source=SQL_SERVER;Initial Catalog=WebTimeSheetDB;Integrated Security=True"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        $connection.Close()
        $results["Database"] = "OK"
    } catch {
        $results["Database"] = "FAILED: $($_.Exception.Message)"
    }
    
    # 3. Test HTTP Response
    try {
        $response = Invoke-WebRequest -Uri "https://your-domain.com/Login" -UseBasicParsing
        $results["HTTP"] = if ($response.StatusCode -eq 200) { "OK" } else { "FAILED: Status $($response.StatusCode)" }
    } catch {
        $results["HTTP"] = "FAILED: $($_.Exception.Message)"
    }
    
    # 4. Test Disk Space
    $disk = Get-WmiObject -Class Win32_LogicalDisk | Where-Object {$_.DeviceID -eq "C:"}
    $freeSpaceGB = [math]::Round($disk.FreeSpace / 1GB, 2)
    $results["DiskSpace"] = if ($freeSpaceGB -gt 5) { "OK ($freeSpaceGB GB free)" } else { "WARNING: Low disk space ($freeSpaceGB GB)" }
    
    return $results
}

# Uruchomienie testu
Test-WebTimeSheetHealth
```

### Maintenance Schedule

```powershell
# Harmonogram konserwacji
# Codziennie o 1:00 - Database maintenance
# Codziennie o 2:00 - Full database backup
# Co godzinę - Transaction log backup
# Co tydzień - Rebuild indexes
# Co miesiąc - Update statistics
# Co kwartał - Archive old data

# Przykład skryptu maintenance
sqlcmd -S "SQL_SERVER" -Q "
-- Update statistics
EXEC sp_updatestats;

-- Rebuild fragmented indexes
DECLARE @sql NVARCHAR(MAX) = '';
SELECT @sql = @sql + 'ALTER INDEX ' + i.name + ' ON ' + s.name + '.' + t.name + ' REBUILD;' + CHAR(13)
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ps ON i.object_id = ps.object_id AND i.index_id = ps.index_id
WHERE ps.avg_fragmentation_in_percent > 30 AND i.index_id > 0;

EXEC sp_executesql @sql;
"
``` 