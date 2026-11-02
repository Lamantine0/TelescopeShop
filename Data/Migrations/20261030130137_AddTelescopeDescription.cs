using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelescopeShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTelescopeDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Telescopes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Зеркальный телескоп на экваториальной монтировке. Подходит для наблюдений Луны, планет и ярких туманностей. Компактная тренога с регулируемой высотой и плавным ходом по обоим осям.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Настольный телескоп-ньютона в корпусе Гершеля. Благодаря короткому фокусу даёт широкое поле зрения — удобен для обзорных наблюдений Млечного Пути и рассеянных скоплений.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Классический вращающийся добсониан с трубой на лямбда-креплении. Апертура 203 мм собирает достаточно света для наблюдений слабых галактик и глобулярных скоплений.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Катадиоптрический телескоп с оптикой ACF на автоматизированной вилковой монтировке. Длинный фокус обеспечивает высокое увеличение — хорош для планетных наблюдений и лунной съёмки.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Компактный катадиоптрик на одновилковой монтировке с компьютерным наведением. Модель со встроенной базой небесных объектов позволяет быстро найти нужную цель.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Добсониан с алюминиевой трубой и крепким основанием. Оптимальный выбор для начинающих: большая апертура без сложной настройки и дорогой монтировки.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Лёгкий рефрактор на азимутальной треноге — первый телескоп для детей и путешествий. В комплекте линза Барлоу, удваивающая увеличение.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Настольный телескоп-ньютона на поворотном основании. Короткий фокус даёт широкое поле — подходит для наблюдений с балкона в условиях городской засветки.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Мощный добсониан с апертурой 10 дюймов на трёхопорном основании. Серьёзный инструмент для глубокого космоса: туманности, галактики и шаровые скопления.");

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Компактный максутов-кассегрейн с Wi-Fi-управлением. Лёгкий корпус и короткая тренога делают его удобным для выездных наблюдений и астрофотографии Луны.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Telescopes");
        }
    }
}
