using DashboardVentas.API.Data;
using DashboardVentas.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ventas.db"));

builder.Services.AddScoped<DashboardService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("react", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Backend Dashboard Ventas funcionando en Render");

//app.UseHttpsRedirection();
app.UseCors("react");
app.UseAuthorization();
app.MapControllers();
app.Run();