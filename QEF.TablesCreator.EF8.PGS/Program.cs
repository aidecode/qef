using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QEF.Context.PGS;
using QEF.Context.SQL;
using QEF.Model;

class Programm
{
    private static IConfiguration _configuration;
    private static DbContext _dbcontext;
    static async Task Main(string[] args)
    {
        Console.WriteLine("Start Init");
        await InitAsync();
        Console.WriteLine("End Init");
    }

    static IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        return configuration.Build();
    }

    static async Task InitAsync()
    {
        _configuration = BuildConfiguration();
        _dbcontext = ContextCreator.Create(_configuration.GetConnectionString(ContextDefaults.SectionName));
        _dbcontext.Database.EnsureCreated();
        await InsertDataAsync(_dbcontext);
    }

    private static async Task InsertDataAsync(DbContext dbcontext)
    {
        var symbol = 'A';
        var companiesCount = 3;
        if (!dbcontext.Set<Company>().Any())
        {
            for (int i = 1; i <= companiesCount; i++)
            {
                var postfix = symbol < 'Z' ? symbol++.ToString() : i.ToString();
                var company = new Company
                {
                    Name = $"Company Name {postfix}",
                    Email = $"company{postfix}@email.com",
                };

                dbcontext.Set<Company>().Add(company);
            }

            await dbcontext.SaveChangesAsync();
        }

        symbol = 'A';
        var departmentsCount = 9;
        if (!dbcontext.Set<Department>().Any())
        {
            for (int i = 1; i <= departmentsCount; i++)
            {
                var postfix = symbol < 'Z' ? symbol++.ToString() : i.ToString();
                var companyId = dbcontext.Set<Company>().Skip(i % companiesCount).Select(x => (int?)x.Id).FirstOrDefault();
                var department = new Department
                {
                    // Id = i,
                    Name = $"Department Name {postfix}",
                    Description = $"Department Description {postfix}",
                    Number = i * 10,
                    CompanyId = companyId,
                };

                dbcontext.Set<Department>().Add(department);
            }

            await dbcontext.SaveChangesAsync();
        }

        symbol = 'A';
        var employeesCount = 20;
        var date = new DateTimeOffset(2000, 6, 1, 0, 0, 0, TimeSpan.Zero);
        if (!dbcontext.Set<Employee>().Any())
        {
            for (int i = 1; i <= employeesCount; i++)
            {
                var postfix = symbol < 'Z' ? symbol++.ToString() : i.ToString();

                var departmentId = dbcontext.Set<Department>().Skip(i % departmentsCount).Select(x => (int?)x.Id).FirstOrDefault() ??
                    throw new InvalidOperationException(
                        $"Error get {nameof(Department)}, " +
                        $"{nameof(i)} = {i}, " +
                        $"{nameof(employeesCount)} = {employeesCount}, " +
                        $"{nameof(departmentsCount)} = {departmentsCount}");

                var employee = new Employee
                {
                    Name = $"Employee Name {postfix}",
                    BirthDate = date.AddDays(i % 10),
                    DepartmentId = departmentId
                };

                dbcontext.Set<Employee>().Add(employee);
            }

            await dbcontext.SaveChangesAsync();
        }
    }
}