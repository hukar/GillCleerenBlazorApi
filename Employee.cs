namespace GillCleerenBlazorApi;

public class Employee
{
    public int EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public int CountryId { get; set; }
    public bool Smocker { get; set; }
    public Gender Gender { get; set; }
}
