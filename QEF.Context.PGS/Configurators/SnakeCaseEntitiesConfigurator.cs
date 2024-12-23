using Microsoft.EntityFrameworkCore;

namespace QEF.Context.Configurations
{
    /// <inheritdoc cref="IEntitiesConfigurator"/>
    public class SnakeCaseEntitiesConfigurator : IEntitiesConfigurator
    {
        /// <inheritdoc/>
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration("departments", null));
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration("employees", null));
            modelBuilder.ApplyConfiguration(new CompanyConfiguration("companies", null));
        }
    }
}
