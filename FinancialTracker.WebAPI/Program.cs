using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Infrastructure;
using FinancialTracker.Infrastructure.Repositories;
using FinancialTracker.Infrastructure.Utilities;
using FinancialTracker.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinancialTracker.Middlewares; 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<TransactionsService>();
builder.Services.AddScoped<ProceduresHelper>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen();

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure middleware for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty; // Make Swagger the root page
    });
}

// General middleware


app.UseAuthentication();
app.UseAuthorization();
//Custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();