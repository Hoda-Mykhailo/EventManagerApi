# EventManagerApi

## Опис проекту
EventManagerApi — це простий ASP.NET Core WebAPI сервіс для управління подіями та користувачами. Проект реалізує функції реєстрації, авторизації та аутентифікації користувачів, управління ролями та подіями, з використанням патернів проектування і сучасної архітектури.

---
## Використані технології та пакети
- .NET 8
- ASP.NET Core WebAPI
- Entity Framework Core (EF Core) 9
- SQL Server (або інша підтримувана база даних)
- Moq — для unit-тестування
- xUnit — для написання тестів
- FluentAssertions — для зручних перевірок у тестах
- Microsoft.Extensions.Configuration — для роботи з конфігурацією
- System.IdentityModel.Tokens.Jwt — для генерації JWT

---
## Пакети NuGet які були додані в проект для розробки:
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package Swashbuckle.AspNetCore
dotnet add package BCrypt.Net-Next
dotnet add EventManagerApi.Tests package Moq
dotnet add EventManagerApi.Tests package xunit
dotnet add EventManagerApi.Tests package FluentAssertions

--
## Логіка проекту
1.Користувачі та ролі
  -Користувачі можуть реєструватися і логінитися.
  -Використовується JWT для аутентифікації.
  -Ролі користувачів зберігаються у базі даних (Role), під час реєстрації присвоюється роль "User".
2.Події
  -Можливість створювати та отримувати події.
  -Події пов’язані з користувачами (creatorId).
3.Сервісний шар
  -Клас UserService реалізує логіку реєстрації, логіну та генерації токена.
  -Клас EventService реалізує бізнес-логіку роботи з подіями.
4.Репозиторійний шар
  -Репозиторії (IUserRepository, IRoleRepository, IEventRepository) відповідають за доступ до даних.
  -Використано патерн Repository для інкапсуляції доступу до бази.

## Патерни проектування
SOLID:
  -Single Responsibility: сервіси відповідають тільки за бізнес-логіку.
  -Dependency Inversion: контролери залежать від абстракцій (інтерфейсів).
  -Interface Segregation та Open/Closed дотримані при реалізації репозиторіїв і сервісів.
Clean Architecture:
  -Розділення шарів: контролери -> сервіси -> репозиторії -> модель даних.
Repository Pattern: інкапсуляція логіки доступу до бази даних.
Dependency Injection: сервіси та репозиторії впроваджуються через DI.


## БД
Базаданих створювалась через міграцію EntityFramework, методом code-first. Під час міграції трохи виникали труднощі, потрібно було додати один додатковий споміжний клас для ДБКонтексту щоб можна було зробити міграцію.
Також іноді потрібно зайти глибше впроект у терміналі для пропису команди міграції(використовував такі команди: dotnet ef migrations add InitialCreate,  dotnet ef database update) і з .sln розширення проекту потрібно було зайтив в .csproj здається так називається.)) Ну і було створено базу з таблицями Event, User, Role.

## Unit-тестування
В цьому пункті найбільш виникало проблем, в цілому одна так і лишилась. Виконуються тільки 4 з 5 тестів, там із тестом реєстрації склались труднощі, і мені здається що це через токени або ключі, бо воно не знаходить його і пише що видає Null. Особисто для мене так і лишилось найважчим це створення юніт тесті, хоча просто треба набити руку на більш складних а не на простих типу на повернення якогось математичного значення.
Тестування проводив на окремому проекті, але як в просторі імен основного проекту(типу того що має розширення .sln). І вже в цьому проекті для тестів створив два класи для тестів івентів та користувачів і їх дій.

Якщо ви зможете надати фідбек по проекту, то можете написати які є явні помики , як їх можна булоб виправити і, якщо зможете, підказати що не так із юніт-тестом по реєстрації.Дякую та з нетерпінням буду чекати зворотнього зв'язку.
