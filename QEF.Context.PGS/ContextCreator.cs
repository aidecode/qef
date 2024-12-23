using Microsoft.EntityFrameworkCore;
using QEF.Context.Configurations;

namespace QEF.Context.PGS
{
    /// <summary>
    /// Context creator.
    /// </summary>
    public static class ContextCreator
    {
        /// <summary>
        /// Creates <see cref="EntityContext"/>.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns><see cref="EntityContext"/>.</returns>
        public static EntityContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<EntityContext>()
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging()
                .Options;

            return new EntityContext(options, new SnakeCaseEntitiesConfigurator());
        }
    }
}
