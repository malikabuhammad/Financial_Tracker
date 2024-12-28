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
using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Infrastructure.BackgroundServices;
using FinancialTracker.WebAPI.Hubs;
using FinancialTracker.WebAPI.Services;
using FinancialTracker.Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGoals, GoalsRepository>();
builder.Services.AddScoped<ICategories, CategoriesRepository>();
builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionsRepository>();

builder.Services.AddScoped<RecurringTransactionsService>();
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<TransactionsService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GoalsService>();
builder.Services.AddScoped<ProceduresHelper>();
 builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddSingleton<IJobRepository, JobRepository>(); // Singleton repository
builder.Services.AddHostedService<TransactionsJobService>(); // Hosted service
builder.Services.AddScoped<INotificationService, FinancialTracker.WebAPI.Services.NotificationService>();
builder.Services.AddSingleton<MachineLearningService>();
builder.Services.AddScoped<SpendingDataExportService>();
builder.Services.AddScoped<SpendingPredictionService>();
builder.Services.AddHostedService<CsvUpdateAndRetrainService>();

builder.Services.AddScoped<IMachineLearningService, MachineLearningService>();

//builder.Services.AddSingleton < TransactionsJobService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen();

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

// Add Authorization
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
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

// Custom middleware 
app.UseMiddleware<ExceptionHandlingMiddleware>();

 
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
 
app.UseHttpsRedirection();

 
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/Notifications");
app.MapControllers();



app.Run();