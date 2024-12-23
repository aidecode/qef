using Microsoft.EntityFrameworkCore;
using QEF.Context.EF6.Tests.Fixtures;
using QEF.Context.Queries;
using Xunit;

namespace QEF.Context.EF6.Tests
{
    /// <summary>
    /// Contains() in Where() tests. 
    /// </summary>
    public class ContainsQueryTests(EntityContextFixture entityContextFixture) :
        IClassFixture<EntityContextFixture>
    {
        private readonly EntityContextFixture _entityContextFixture = entityContextFixture;

        [Fact]
        public async Task SelectEmployeesQuery_ForEmployeesIds_ReturnsEmployeesAsync()
        {
            // Arrange
            var query = EntitiesQueries.SelectEmployeesQuery(
                _entityContextFixture.DbContext, [1, 2, 3, 4]);

            // Act
            var employees = await query.ToListAsync();

            // Assert
            Assert.NotEmpty(employees);
        }

        [Fact]
        public async Task SelectEmployeesQuery_ForOneEmployeeId_ReturnsOneEmployeeAsync()
        {
            // Arrange
            var query = EntitiesQueries.SelectEmployeesQuery(
                _entityContextFixture.DbContext, [1]);

            // Act
            var employees = await query.ToListAsync();

            // Assert
            Assert.Single(employees);
            Assert.Collection(employees, e => Assert.Equal(1, e.Id));
        }

        [Fact]
        public async Task EmployeesCountInDepartmentsQuery_ForDepartmentsIds_ReturnsCountersAsync()
        {
            // Arrange
            var query = EntitiesQueries.EmployeesCountInDepartmentsQuery(
                _entityContextFixture.DbContext, [1, 3]);

            // Act
            var counters = await query.ToListAsync();

            // Assert
            Assert.NotEmpty(counters);
        }
    }
}