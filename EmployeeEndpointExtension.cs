using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using FluentValidation;

namespace GillCleerenBlazorApi;

public static class EmployeeEndpointExtension
{
    public static WebApplication MapEmployee(this WebApplication app)
    {
        var route = app.MapGroup("employees");

        route.MapGet("/", async (IDbConnection connection) =>
        {
            var sql = @"SELECT * FROM Employee";

            var employees = await connection.QueryAsync<Employee>(sql);

            return Results.Ok(employees);
        });

        route.MapGet("/{id:int}", async (IDbConnection connection, int id) => {
            var sql = @"SELECT * FROM Employee WHERE EmployeeId = @id";

            var employee = await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { id });
            if(employee is null) return Results.NotFound();
            return Results.Ok(employee);
        });

        route.MapPost("/", async (IDbConnection connection, IValidator<Employee> Validator, Employee employeeToCreate) =>
        {
            var result = Validator.Validate(employeeToCreate);

            if(result.IsValid == false)
            {
               var errorMessages = result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");

               return Results.BadRequest(errorMessages); 
            }
            
            var sql = @"INSERT INTO Employee (FirstName, LastName, Email, BirthDate, CountryId, Smoker, Gender)
                VALUES (@FirstName, @LastName, @Email, @BirthDate, @CountryId, @Smoker, @Gender)";

            var rowsAffected = await connection.ExecuteAsync(sql, employeeToCreate);

            return Results.Ok(rowsAffected);
        });

        route.MapPut("/", async (IDbConnection connection, IValidator<Employee> Validator, Employee employeeToUpdate) =>
        {
            var result = Validator.Validate(employeeToUpdate);

            if(result.IsValid == false)
            {
               var errorMessages = result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");

               return Results.BadRequest(errorMessages); 
            }
            
            var sql = @"UPDATE Employee
                        SET 
                            FirstName = @FirstName, 
                            LastName = @LastName, 
                            Email = @Email, 
                            BirthDate = @BirthDate, 
                            CountryId = @CountryId,
                            Smoker = @Smoker,
                            Gender = @Gender
                        WHERE EmployeeId = @EmployeeId";

            var rowsAffected = await connection.ExecuteAsync(sql, employeeToUpdate);

            return Results.Ok(rowsAffected);
        });

        route.MapDelete("/{id:int}", async (IDbConnection connection, int id) => {
            var sql = @"DELETE FROM Employee WHERE EmployeeId = @EmployeeId";

            var rowsAffected = await connection.ExecuteAsync(sql, new { id });

            return Results.Ok(rowsAffected);
        });

        return app;
    }
}
