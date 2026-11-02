using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TelescopeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TelescopeDb")));
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
