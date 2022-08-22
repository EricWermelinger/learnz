global using Learnz.Data;
global using Learnz.Framework;
global using Learnz.Entities;
global using Learnz.Entities.Enums;
global using Learnz.DTOs;
global using Learnz.Services;
global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter()));

builder.Services.AddSignalR()
                .AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new CustomDateTimeConverter()));

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFilePolicyChecker, FilePolicyChecker>();
builder.Services.AddScoped<IFileFinder, FileFinder>();
builder.Services.AddScoped<IFileAnonymousFinder, FileAnonymousFinder>();
builder.Services.AddScoped<ISetPolicyChecker, SetPolicyChecker>();
builder.Services.AddScoped<ILearnzFrontendFileGenerator, LearnzFrontendFileGenerator>();
builder.Services.AddScoped<ITogetherQueryService, TogetherQueryService>();
builder.Services.AddScoped<IGroupQueryService, GroupQueryService>();
builder.Services.AddScoped<ICreateQueryService, CreateQueryService>();
builder.Services.AddScoped<IChallengeQueryService, ChallengeQueryService>();
builder.Services.AddScoped<ILearnQueryService, LearnQueryService>();
builder.Services.AddScoped<ITestQueryService, TestQueryService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "http header => token im Stil: 'bearer token'",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

string corsOrigin = "corsOrigin";
string frontEnd = builder.Configuration.GetSection("Frontend:URL").Value;
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        corsOrigin,
        builder => builder.WithOrigins(frontEnd)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(corsOrigin);

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<LearnzHub>("/ws/learnzsocket");
});

app.Run();
