using Dapper;
using System.Collections.Concurrent;
using System.Data;

namespace GillCleerenBlazorApi;

public static class CountryEndpointExtension
{
    public static WebApplication MapCountry(this WebApplication app)
    {
        var route = app.MapGroup("/Countries");

        route.MapGet("", async (IDbConnection connection) => {
            var sql = @"SELECT * FROM Country";

            var countries = await connection.QueryAsync<Country>(sql);

            return Results.Ok(countries);
        });

        route.MapGet("/{id:int}", async (IDbConnection connection, int id) => {
            var sql = @"SELECT * FROM Country WHERE CountryId = @id";

            var country = await connection.QuerySingleOrDefaultAsync<Country>(sql, new { id });

            if(country is null) return Results.NotFound();

            return Results.Ok(country);
        });

        return app;
    }
}
