using Microsoft.EntityFrameworkCore;

namespace QEF.Context.SQL
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
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .Options;

            return new EntityContext(options);
        }
    }
}
