using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QEF.Context.SQL;

namespace QEF.Context.EF7.Tests.Fixtures
{
    /// <summary>
    /// Fixture for <see cref="DbContext"/>.
    /// </summary>
    public class EntityContextFixture : IDisposable
    {
        private static readonly IConfiguration _configuration = BuildConfiguration();
        private static readonly string _connectionString = 
            _configuration.GetConnectionString(ContextDefaults.SectionName) ??
            throw new ArgumentNullException(
                "ConnectionStrings",
                $"ConnectionStrings[{ContextDefaults.SectionName}] == null");

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityContextFixture"/> class.
        /// </summary>
        public EntityContextFixture() => DbContext.Database.EnsureCreated();

        /// <summary>
        /// <see cref="DbContext"/>.
        /// </summary>
        public DbContext DbContext { get; private set; } = ContextCreator.Create(_connectionString);

        /// <inheritdoc/>
        public void Dispose() => DbContext?.Dispose();

        static IConfiguration BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json");
            return configuration.Build();
        }
    }
}
