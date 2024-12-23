using Microsoft.EntityFrameworkCore;

namespace QEF.Context.Configurations
{
    /// <summary>
    /// Entities Configurator.
    /// </summary>
    public interface IEntitiesConfigurator
    {
        /// <summary>
        /// Configures models.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
        void Configure(ModelBuilder modelBuilder);
    }
}
