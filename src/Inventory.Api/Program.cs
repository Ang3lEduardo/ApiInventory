using System.Reflection;
using Inventory.Api.Data;
using Inventory.Application;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//if (!builder.Environment.IsDevelopment()){
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
    });
    builder.Services.AddAuthorization();
    //}
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddControllers();
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
var app = builder.Build();

using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider
    .GetRequiredService<InventoryDbContext>();

await DbInitializer.InitializeAsync(dbContext);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
//if (!builder.Environment.IsDevelopment()){
app.UseAuthentication();
app.UseAuthorization();
//}
app.MapControllers();

app.Run();
