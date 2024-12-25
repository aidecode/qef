using QEF.Context.Extensions;
using System.Linq.Expressions;

namespace QEF.Context.EF7.Tests
{
    /// <summary>
    /// <see cref="QueryableExtensions"> tests. 
    /// </summary>
    public class QueryableExtensionsTests
    {
        private static readonly List<FakeDataItem<int>> FakeIntDataList =
            Enumerable.Range(1, 10).Select(x => new FakeDataItem<int> { DataValue = x }).ToList();

        /// <summary>
        /// Gets bad test data.
        /// </summary>
        public static IEnumerable<object[]> GetBadTestData()
        {
            Expression<Func<int, int>> keySelector = x => x;

            return
            [
                [new[] { 1, 2 }, null, "keySelector"],
                [null, keySelector, "valuesSet"],
            ];
        }

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

        [Theory(DisplayName = "In() filters data correctly for int")]
        [MemberData(nameof(GetTestData))]
        public void In_ForIntParameters_FiltersDataCorrectly(
            IEnumerable<int> values, IEnumerable<int> expectedValues)
        {
            // Arrange
            var query = FakeIntDataList.AsQueryable();

            // Act
            var filteredData = query.In(values.ToHashSet(), x => x.DataValue).ToList();

            // Assert
            var filteredValues = filteredData.Select(x => x.DataValue);
            Assert.Equal(expectedValues, filteredValues);
        }

        [Theory(DisplayName = "In() filters data correctly for string")]
        [MemberData(nameof(GetTestData))]
        public void In_ForStringParameters_FiltersDataCorrectly(
            IEnumerable<int> values, IEnumerable<int> expectedValues)
        {
            // Arrange
            var query = FakeIntDataList.Select(
                x => new FakeDataItem<string>() { DataValue = $"v{x.DataValue}" }).AsQueryable();
            var stringValues = values.Select(x => $"v{x}").ToHashSet();
            var expectedStringValues = expectedValues.Select(x => $"v{x}");

            // Act
            var filteredData = query.In(stringValues, x => x.DataValue).ToList();

            // Assert
            var filteredValues = filteredData.Select(x => x.DataValue);
            Assert.Equal(expectedStringValues, filteredValues);
        }

        [Theory(DisplayName = "In() throws ArgumentNullException for invalid params")]
        [MemberData(nameof(GetBadTestData))]
        public void In_ForBadParameters_ThrowsArgumentNullException(
            int[] values, Expression<Func<int, int>> keySelector, string expectedParameterName)
        {
            // Arrange
            var query = new[] { 1, 2, 3 }.AsQueryable();
            void act() => query.In(values?.ToHashSet(), keySelector);

            // Act
            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => act());
            Assert.Equal(expectedParameterName, exception.ParamName);
            Assert.Contains(expectedParameterName, exception.Message);
        }

        /// <summary>
        /// Fake data item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class FakeDataItem<T>
        {
            /// <summary>
            /// Fake data value.
            /// </summary>
            public T? DataValue { get; set; }
        }
    }
}
