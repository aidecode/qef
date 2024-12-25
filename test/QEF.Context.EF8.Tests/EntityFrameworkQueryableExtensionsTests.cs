using Microsoft.EntityFrameworkCore;
using QEF.Context.EF8.Tests.Fixtures;
using QEF.Context.Extensions;
using QEF.Model;

namespace QEF.Context.EF8.Tests
{
    /// <summary>
    /// <see cref="EntityFrameworkQueryableExtensions"> tests. 
    /// </summary>
    public class EntityFrameworkQueryableExtensionsTests(EntityContextFixture entityContextFixture) :
        IClassFixture<EntityContextFixture>
    {
        /// <summary>
        /// Gets valid test data.
        /// </summary>
        public static IEnumerable<object[]> GetTestData()
            =>
                [
                    [new[] { 1, 2, 3 }, new[] { 1, 2, 3 }],
                    [new[] { 3 }, new[] { 3 }],
                    [new[] { 3, 3 }, new[] { 3 }],
                    [new[] { 2, 3, 3, 3, 5, 5 }, new[] { 2, 3, 5 }],
                    [new[] { 9999 }, Array.Empty<int>()],
                    [new[] { 4, 5, 9999 }, new int[] { 4, 5 }],
                    [Array.Empty<int>(), Array.Empty<int>()],
                ];

        private readonly EntityContextFixture _entityContextFixture = entityContextFixture;

        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task ToListByPortionsAsync_ForEntity_FiltersAndGetsListCorrectlyAsync(
            IEnumerable<int> values, IEnumerable<int> expectedValues)
        {
            // Arrange
            var portionSizes = new[] { (int?)null, -1, 0, 1, 2, 3, 8, 10, 15, 1000, 10000 };

            var query = entityContextFixture.DbContext.Set<Employee>();
            var filteredResults = new List<List<Employee>>(portionSizes.Length);
            var valuesSet = values.ToHashSet();

            // Act
            foreach (var portionSize in portionSizes)
            {
                filteredResults.Add(
                    portionSize.HasValue ?
                    await query.ToListByPortionsAsync(valuesSet, x => x.Id, portionSize.Value) :
                    await query.ToListByPortionsAsync(valuesSet, x => x.Id));
            }

            // Assert
            filteredResults.ForEach(
                filteredResult =>
                {
                    var filteredValues = filteredResult.Select(x => x.Id);
                    Assert.Equal(filteredValues, expectedValues);
                });
        }

        [Fact]
        public async Task ToListByPortionsAsync_ForEmployees_FiltersAndGetsListCorrectlyAsync()
        {
            // Arrange
            var dbcontext = entityContextFixture.DbContext;
            var ids = new[] { 1, 2, 3, 4, 5, 6, 7, };

            // Act
            var employees = await dbcontext.Set<Employee>().ToListByPortionsAsync(
                [.. ids], x => x.Id, portionSize: 4);

            // Assert
            Assert.Equal(7, employees.Count);
        }

        [Fact]
        public async Task In_BeforeToListAsync_FiltersAndGetsListCorrectlyAsync()
        {
            // Arrange
            var dbcontext = entityContextFixture.DbContext;
            var ids = new[] { 1, 2, 3, 4, 5, 6, 7, };

            // Act
            var employees = await dbcontext.Set<Employee>().In(
                [.. ids], x => x.Id).ToListAsync();

            // Assert
            Assert.Equal(7, employees.Count);
        }
    }
}
