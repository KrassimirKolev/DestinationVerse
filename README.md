# DestinationVerse - Изпитен Проект по ASP.NET Core

Добре дошли в **DestinationVerse** – уеб приложение за споделяне и разглеждане на туристически дестинации и маршрути. Проектът е разработен като изпитна работа за курса по ASP.NET Core в SoftUni, спазвайки стриктно изискванията за трислойна софтуерна архитектура, сигурност и Fluent API конфигурация.

## Архитектура на проекта (3-Tier Architecture)
Приложението е разделено на три независими и логически обособени слоя:
*   **`DestinationVerse.Data`**: Слой за достъп до данните. Съдържа Entity моделите (`Region`, `Destination`, `UserDestination`), миграциите и `ApplicationDbContext` с дефиниран съставен (композитен) ключ за сигурност.
*   **`DestinationVerse.Services`**: Бизнес слой (Services Layer). Съдържа интерфейсите (`Contracts`) и имплементациите на бизнес логиката (`DestinationService`, `RegionService`), следвайки SOLID принципите и Dependency Inversion.
*   **`DestinationVerse`**: Презентационен слой (Web Layer). ASP.NET Core MVC приложение, съдържащо контролерите, ViewViewModels за сигурност на формите и Razor изгледите за визуализация.

## Използвани технологии и версии
*   **Рамка**: .NET 8.0 (ASP.NET Core MVC)
*   **База данни**: SQL Server (чрез Entity Framework Core 8.0.32)
*   **Сигурност**: ASP.NET Core Identity за аутентификация и авторизация
*   **Интерфейс**: Bootstrap 5 за модерен и адаптивен дизайн

## 🚀 Инструкции за локално стартиране и инсталация

### 1. Клониране на репозиторито
```bash
git clone https://github.com
cd DestinationVerse
```

### 2. Настройка на Connection String (Връзка с базата данни)
Отворете файла `DestinationVerse/appsettings.json` и се уверете, че стринга за връзка съответства на вашия локален SQL Server екземпляр:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=DestinationVerse;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### 3. Нанасяне на миграциите (Създаване на базата данни)
Отворете **Package Manager Console** във Visual Studio, изберете `DestinationVerse.Data` като *Default project* и изпълнете:
```powershell
Update-Database
```

### 4. Стартиране на приложението
Натиснете **`Ctrl + F5`** във Visual Studio, за да стартирате проекта в браузъра.

## Тестови потребителски акаунт (Credentials)
За удобство на проверяващите, изискванията за пароли са олекотени до минимум 3 символа в `Program.cs`. Можете да регистрирате нов акаунт директно или да използвате следния тестов профил:
*   **Имейл**: `admin@destinationverse.com`
*   **Парола**: `123`
