using System.Data;
using System.Reflection;
using Dapper;
using FluentValidation;
using GillCleerenBlazorApi;
using Microsoft.Data.Sqlite;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));
// builder.Services.AddCors();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5135")
                                .AllowAnyMethod()
                                .WithHeaders("Content-Type");
                      });
});

var app = builder.Build();

// app.UseCors(policy => 
//     policy.WithOrigins("http://localhost:5135")
//     .AllowAnyMethod()
//     .WithHeaders(HeaderNames.ContentType));

app.UseCors("MyAllowSpecificOrigins");

app.MapDb();

app.MapGet("datetime", () => Results.Ok(new {date =DateTime.Now}));

app.MapEmployee();

app.MapCountry();

app.Run();
