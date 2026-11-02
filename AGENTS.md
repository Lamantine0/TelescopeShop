# TelescopeShop — правила проекта

Интернет-магазин телескопов: витрина на Razor Pages + REST API на ASP.NET Core.
Общие правила агента (читать доки первой, язык, коммиты) — в глобальном
`~/.config/opencode/AGENTS.md`; при противоречии побеждает этот файл.

Подробная документация — в `README.md` (структура, модель, API, фильтры).
Сначала читай `README.md`, затем действуй.

## Стек и команды

- .NET 10 / ASP.NET Core (Razor Pages + Web API)
- EF Core 10 + SQLite (`Microsoft.EntityFrameworkCore.Sqlite` 10.0.11, `Design` 10.0.11)
- Swagger (Swashbuckle 10.3.1)
- `dotnet ef` tool установлен (версия 10.0.11)

```powershell
dotnet build --nologo -v q          # сборка
dotnet run                          # запуск
dotnet ef database update           # применить миграции
dotnet ef migrations add <Name> --output-dir Data/Migrations  # новая миграция
```

## Важные особенности (неочевидные)

- **Namespace `TelescopeShop`** — совпадает с именем проекта (раньше был
  `TelescopeShop_` из-за двойной точки в имени csproj; файлы переименованы).
- **Имя exe — `TelescopeShop.exe`** (без двойной точки).
  Путь к exe для ручного запуска: `bin\Debug\net10.0\TelescopeShop.exe`.
- **SQLite-БД** — `Telescope/telescope.db` (строка подключения `TelescopeDb` в `appsettings.json`).
  Файлы `*.db`, `*.db-shm`, `*.db-wal` в `.gitignore`; `Telescope/.gitkeep` держит папку.
  БД восстанавливается через `dotnet ef database update` (seed в `TelescopeDbContext.OnModelCreating`).
- **Seed-данные** — 10 телескопов заданы через `HasData` в `Data/TelescopeDbContext.cs`.
  Правка seed-данных требует новой миграции (EF сгенерирует `UpdateData`).
- **Изображения товаров** — `wwwroot/Product/telescope-1.png` … `telescope-10.png`,
  отдаются через `UseStaticFiles()` по `/Product/telescope-{id}.png`.
  Источники/лицензии — `wwwroot/Product/CREDITS.txt`. Иллюстративные, не точные модели.

## Архитектура

- `Program.cs` — регистрирует DbContext (SQLite), `AddControllers`, `AddRazorPages`,
  Swagger (только в Development), `UseStaticFiles`, `MapControllers` + `MapRazorPages`.
- `Controllers/TelescopesController.cs` — CRUD API, маршрут `[Route("[controller]")]` → `/telescopes`
  (GET/GET by id/POST 201/PUT 204/DELETE 204). `Apply()` — фильтры, `ToDto()` — проекция в DTO.
- `Models/Telescope.cs` + `DTOs/TelescopeDto.cs` — модель и DTO (оба содержат `Description`).
- `Pages/Index.cshtml(.cs)` — каталог; фильтрация на сервере (QueryString: `search`,
  `apertureMin/Max`, `focalMin/Max`, `barlow`); панель фильтров — нативный `<details>` без JS.
- `Pages/Telescope.cshtml(.cs)` — страница товара, маршрут `@page "{id:int}"` → `/Telescope/{id}`;
  `OnGetAsync(int id)` возвращает `NotFound()` при отсутствии; секция «Галерея»
  (`#gallery`) в `.product-media`: для Id=1 (Celestron 130/650) — 2 снимка из
  `wwwroot/AstroPhoto` (свойство `Gallery` PageModel), для остальных моделей —
  2 слота-заглушки; клик по снимку открывает lightbox во весь экран
  (чистый CSS на `:target`, без JS); блок «Похожие телескопы».
- `wwwroot/css/site.css` — все стили витрины.

### Конвенции маршрутов

- Каталог — на `/` (`Pages/Index`), **не** на `/Telescopes`, чтобы не конфликтовать с API `/telescopes`.
- Страница товара — `/Telescope/{id}` (конфликтует только сама с собой; API остаётся на `/telescopes`).

## Язык и стиль

- Весь UI, README, seed-тексты и описания — на **русском**.
- Отвечай пользователю на языке, на котором он пишет.
- Витрина — серверный рендеринг Razor; API не трогай без нужды.

## Git

- Репозиторий: `https://github.com/Lamantine0/TelescopeShop.git` (ветка `main`).
- Коммитить/пушить **только по явной просьбе** пользователя.
- Коммиты короткие, на английском, в повелительном натяжении (`Add ...`, `Remove ...`).
- Никогда не коммитить секреты/ключи; `*.db*` уже игнорируются.

## Проверка после изменений

```powershell
# остановить зависшие процессы приложения перед сборкой (иначе MSB3026 file-lock)
Stop-Process -Name "TelescopeShop" -Force -ErrorAction SilentlyContinue
dotnet build --nologo -v q
```

Smoke-тест (вручную или скриптом): запустить exe на `--urls http://127.0.0.1:5199`
**из корня проекта** (путь к SQLite относительный; другой cwd → 500 на всех запросах),
проверить `GET /` (10 карточек), `GET /Telescope/{1..10}` (200 + описание),
`GET /Telescope/999` (404), `GET /telescopes` (JSON + `description`).
После теста убить процесс по имени `TelescopeShop`.

Ловушка PowerShell: присваивание после `&&` даёт ParserError — используй `;`.
