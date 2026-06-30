using Horizon.Server.Extensions;
using Horizon.Server.Modules.ApprovalWorkflow;
using Horizon.Server.Modules.Employees;
using Horizon.Server.Modules.LeaveManagement;
using Horizon.Server.Modules.References;
using Horizon.Server.Modules.Shared;
using Horizon.Server.Modules.Users;
using Horizon.Server.Modules.Users.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
{
    throw new InvalidOperationException(
        "JWT configuration is missing. Ensure 'JwtSettings' section exists in appsettings.json " +
        "or override via environment variables: JwtSettings__SecretKey, JwtSettings__Issuer, JwtSettings__Audience.");
}

if (!builder.Environment.IsDevelopment() &&
    jwtSettings.SecretKey.Contains("development-only"))
{
    throw new InvalidOperationException(
        "Production JWT SecretKey must not use the development default. " +
        "Set the JwtSettings__SecretKey environment variable to a strong random value.");
}

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // В продакшене - true
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    // Для работы с SignalR или WebSockets 
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAuthorization(options =>
{
    // Политики на основе ролей
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("HRAndAbove", policy => policy.RequireRole("Admin", "HRManager"));
    options.AddPolicy("ManagerAndAbove", policy => policy.RequireRole("Admin", "HRManager", "DepartmentManager"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Admin", "HRManager", "DepartmentManager", "Employee"));

    // Политики на основе прав
    options.AddPolicy("CanApproveHR", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "permission" && c.Value == "request:approve-hr") ||
            context.User.IsInRole("Admin")));

    options.AddPolicy("CanApproveSubordinate", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "permission" && c.Value == "request:approve-subordinate") ||
            context.User.IsInRole("Admin") ||
            context.User.IsInRole("HRManager")));

    options.AddPolicy("CanViewAllRequests", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "permission" && c.Value == "request:view-all") ||
            context.User.IsInRole("Admin")));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();

builder.Services.AddSharedModule();
builder.Services.AddEmployeesModule();
builder.Services.AddLeaveManagementModule();
builder.Services.AddApprovalWorkflowModule();
builder.Services.AddUsersModule();
builder.Services.AddReferencesModule();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection")
    ?? builder.Configuration.GetConnectionString("PostgresConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("PostgreSQL connection string not configured. Set ConnectionStrings__PostgresConnection env var or appsettings.json.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173",
                "http://localhost:5174",
                "https://localhost:53830",
                "http://localhost:53830"
            )
            .AllowAnyMethod()                 
            .AllowAnyHeader()                 
            .AllowCredentials();             
    });

    options.AddPolicy("DevPolicy", policy =>
    {
        policy.AllowAnyOrigin()                
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwaggerWithUi();
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.Run();