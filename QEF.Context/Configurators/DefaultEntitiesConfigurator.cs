using Microsoft.EntityFrameworkCore;

namespace QEF.Context.Configurations
{
    /// <inheritdoc cref="IEntitiesConfigurator"/>
    public class DefaultEntitiesConfigurator : IEntitiesConfigurator
    {
        /// <inheritdoc/>
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration("Departments", null));
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration("Employees", null));
            modelBuilder.ApplyConfiguration(new CompanyConfiguration("Companies", null));
        }
    }
}
