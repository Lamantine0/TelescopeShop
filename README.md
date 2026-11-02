# TelescopeShop

Интернет-магазин телескопов: витрина на Razor Pages + REST API на ASP.NET Core.

## Стек

- .NET 10 / ASP.NET Core (Razor Pages + Web API)
- Entity Framework Core 10 + SQLite
- Swagger (Swashbuckle)
- Статические файлы для изображений товаров

## Структура

```
Controllers/TelescopesController.cs   CRUD-контроллер
Models/Telescope.cs                   модель телескопа
DTOs/TelescopeDto.cs                  DTO для API
Data/TelescopeDbContext.cs            DbContext + начальные данные
Data/Migrations/                      миграции EF Core
Pages/Index.cshtml(+.cs)              страница каталога телескопов
Pages/Telescope.cshtml(+.cs)          страница товара с полным описанием
Pages/Shared/_Layout.cshtml           общий layout витрины
Telescope/telescope.db                база данных SQLite
wwwroot/Product/                      изображения товаров (telescope-1..10.png)
wwwroot/Product/CREDITS.txt           источники и лицензии фотографий
wwwroot/AstroPhoto/                   астрофото для галереи (Id=1, 2 снимка)
wwwroot/css/site.css                  стили витрины
```

## Модель Telescope

| Поле | Тип | Описание |
| --- | --- | --- |
| Id | int | первичный ключ |
| Manufacturer | string | производитель |
| TripodHeight | int | высота треноги, мм |
| Aperture | int | диаметр главного зеркала (апертура), мм |
| Seeker | string | искатель |
| BarlowLensIncluded | bool | линза Барлоу в комплекте |
| MaximumUsefulMagnification | int | максимальное полезное увеличение |
| FocalLength | int | фокусное расстояние, мм |
| Description | string | полное описание товара |

## API

| Метод | Маршрут | Описание |
| --- | --- | --- |
| GET | `/telescopes` | список всех телескопов |
| GET | `/telescopes/{id}` | телескоп по Id |
| POST | `/telescopes` | создать телескоп |
| PUT | `/telescopes/{id}` | обновить телескоп |
| DELETE | `/telescopes/{id}` | удалить телескоп |

Swagger UI доступен в режиме разработки: `/swagger`.

## База данных

- Файл: `Telescope/telescope.db`
- Строка подключения: `TelescopeDb` в `appsettings.json`
- В базе 10 тестовых телескопов (seed-данные в `TelescopeDbContext`)

Применить миграции:

```powershell
dotnet ef database update
```

## Запуск

```powershell
dotnet run
```

- Витрина каталога: `http://localhost:<port>/`
- Swagger UI: `http://localhost:<port>/swagger`

## Витрина (Razor Pages)

Страница `Pages/Index.cshtml` рендерится на сервере: `PageModel` читает
товары из `TelescopeDbContext` и выводит адаптивную сетку карточек
(изображение + характеристики) через `wwwroot/css/site.css`.
API остаётся на прежних маршрутах (`/telescopes`) и используется отдельно.

Фильтрация каталога: `GET /?<параметры>` — фильтры комбинируются между собой.
Панель фильтров сворачивается (нативный `<details>`, без JavaScript); при
активных фильтрах она автоматически раскрыта и показывает счётчик.

Страница товара: `GET /Telescope/{id}` (`Pages/Telescope.cshtml`) — фото,
галерея («Галерея»): для Celestron 130 мм · f/650 мм (Id=1) — два снимка из
`wwwroot/AstroPhoto`, клик по снимку открывает его во весь экран (lightbox на
чистом CSS `:target`, без JavaScript); для остальных моделей — два слота-заглушки
под фото, характеристики таблицей, полное описание и блок похожих моделей.
Несуществующий Id возвращает 404.

| Параметр | Значение |
| --- | --- |
| `search` | текст в названии (производителе), регистронезависимо |
| `apertureMin` / `apertureMax` | диапазон апертуры, мм |
| `focalMin` / `focalMax` | диапазон фокусного расстояния, мм |
| `barlow` | `true` — только с линзой Барлоу, `false` — без неё |

Пример: `/?search=Celestron&apertureMin=100&apertureMax=200&focalMax=1600`.
Фильтрация выполняется на сервере (`Pages/Index.cshtml.cs`), без JavaScript.

## Изображения

Фотографии лежат в `wwwroot/Product` и доступны по адресам вида
`/Product/telescope-1.png`. Источники, авторы и лицензии указаны в
`wwwroot/Product/CREDITS.txt`. Фотографии иллюстративные и могут не совпадать
с точной моделью товара.

Снимки для галереи лежат в `wwwroot/AstroPhoto` и открываются по адресу
`/AstroPhoto/<имя файла>` (пока только у модели Id=1).

## План на завтра

- [x] Добавить HTML-разметку (страницу каталога телескопов) — сделано на Razor Pages.
- [x] Вывести карточки товаров: производитель, характеристики, изображение из `/Product/telescope-{id}.png`.
- [x] Добавить базовые стили (CSS) и адаптивную сетку карточек.
- [x] Фильтрация/поиск по каталогу — по названию, апертуре, фокусному расстоянию и наличию линзы Барлоу.
- [x] Страница товара с полным описанием (`/Telescope/{id}`) и блоком похожих моделей.
- [x] Галерея на странице товара: снимки для Id=1, заглушки для остальных, lightbox без JS.
- [ ] Загрузка данных из `GET /telescopes` через JavaScript (клиентский рендеринг).
