using Microsoft.EntityFrameworkCore;
using QEF.Context.Configurations;
using System;

namespace QEF.Context
{
    /// <summary>
    /// Entity context.
    /// </summary>
    public class EntityContext : DbContext
    {
        private readonly IEntitiesConfigurator _configurator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityContext"/> class.
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions"/>.</param>
        public EntityContext(DbContextOptions options)
            : base(options)
        {
            _configurator = new DefaultEntitiesConfigurator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityContext"/> class.
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions"/>.</param>
        /// <param name="configurator"><see cref="IEntitiesConfigurator"/>.</param>
        public EntityContext(DbContextOptions options, IEntitiesConfigurator configurator)
            : base(options) =>
            _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _configurator?.Configure(modelBuilder);
        }
    }
}
