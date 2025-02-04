using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2025, 1, 9, 16, 3, 33, 241, DateTimeKind.Local).AddTicks(9468), new DateTime(2025, 1, 9, 16, 3, 33, 241, DateTimeKind.Local).AddTicks(9457), "", 50.0, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la villa...", new DateTime(2025, 1, 9, 16, 3, 33, 241, DateTimeKind.Local).AddTicks(9470), new DateTime(2025, 1, 9, 16, 3, 33, 241, DateTimeKind.Local).AddTicks(9470), "", 60.0, "Premium Vista a la Piscina", 3, 200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
