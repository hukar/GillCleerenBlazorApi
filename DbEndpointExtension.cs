using System.Data;
using Dapper;

namespace GillCleerenBlazorApi;

public static class DbEndpointExtension
{
    public static WebApplication MapDb(this WebApplication app)
    {
        app.MapGet("createcountries", async (IDbConnection connection) =>
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Country (
                            CountryId INTEGER PRIMARY KEY,
                            Name TEXT
                        );
                        DELETE FROM Country;
                        INSERT INTO Country (Name)
                        VALUES ('Italia'), ('Belgium'), ('Roumania'), ('France'), ('Spain')";

            var rowsAffected = await connection.ExecuteAsync(sql);

            return Results.Ok(rowsAffected);
        });

        app.MapGet("createdb", async (IDbConnection connection) =>
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Employee (
                            EmployeeId INTEGER PRIMARY KEY,
                            FirstName TEXT,
                            LastName TEXT,
                            Email TEXT,
                            BirthDate TEXT,
                            CountryId INTEGER
                        );
                        DELETE FROM Employee;";

            var rowsAffected = await connection.ExecuteAsync(sql);

            return Results.Ok(new { rowsAffected });
        });


        return app;
    }
}
