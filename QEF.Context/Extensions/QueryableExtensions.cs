using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QEF.Context.Extensions
{
    /// <summary>
    /// IQueryable related extension methods.
    /// </summary>
    public static class QueryableExtensions
    {
        private const int SqlServerParametersLimit = 2100;

        /// <summary>
        /// Filters data using the Where method by values of the HashSet.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <typeparam name="TKey">Type of values.</typeparam>
        /// <param name="source">Data source.</param>
        /// <param name="valuesSet">Values.</param>
        /// <param name="keySelector">Key field selector.</param>
        /// <returns>Filtered query.</returns>
        public static IQueryable<TSource> In<TSource, TKey>(
            this IQueryable<TSource> source,
            HashSet<TKey> valuesSet,
            Expression<Func<TSource, TKey>> keySelector)
        {
            if (valuesSet == null)
            {
                throw new ArgumentNullException(nameof(valuesSet));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (valuesSet.Count == 0)
            {
                return source.Take(0);
            }

            var bucketSize = GetBucketSize(valuesSet.Count);

            if (bucketSize > SqlServerParametersLimit)
            {
                var body = Expression.Call(
                    typeof(Enumerable),
                    nameof(Enumerable.Contains),
                    [typeof(TKey)],
                    Expression.Constant(valuesSet),
                    keySelector.Body);

                var predicate = Expression.Lambda<Func<TSource, bool>>(body, keySelector.Parameters);
                return source.Where(predicate);
            }

            var distinctValues = Bucketize(valuesSet, bucketSize);
            var expression = CreateBalancedORExpression(
                distinctValues, keySelector.Body, 0, distinctValues.Length - 1);

            var clause = Expression.Lambda<Func<TSource, bool>>(
                expression, keySelector.Parameters);

            return source.Where(clause);
        }

        /// <summary>
        /// Creates OR expression.
        /// </summary>
        private static BinaryExpression CreateBalancedORExpression<TKey>(
            TKey[] values, Expression keySelectorBody, int start, int end)
        {
            if (start == end)
            {
                var v1 = values[start];
                return Expression.Equal(keySelectorBody, ((Expression<Func<TKey>>)(() => v1)).Body);
            }
            else if (start + 1 == end)
            {
                var v1 = values[start];
                var v2 = values[end];

                return Expression.OrElse(
                    Expression.Equal(keySelectorBody, ((Expression<Func<TKey>>)(() => v1)).Body),
                    Expression.Equal(keySelectorBody, ((Expression<Func<TKey>>)(() => v2)).Body));
            }
            else
            {
                int mid = (start + end) / 2;
                return Expression.OrElse(
                    CreateBalancedORExpression(values, keySelectorBody, start, mid),
                    CreateBalancedORExpression(values, keySelectorBody, mid + 1, end));
            }
        }

        /// <summary>
        /// Fills the array tail with the last value.
        /// </summary>
        private static TKey[] Bucketize<TKey>(HashSet<TKey> values, int bucketSize)
        {
            var distinctValues = values.ToArray();
            var originalLength = distinctValues.Length;

            if (originalLength == bucketSize)
            {
                return distinctValues;
            }

            var lastValue = distinctValues[originalLength - 1];
            Array.Resize(ref distinctValues, bucketSize);
            distinctValues.AsSpan()[originalLength..].Fill(lastValue);

            return distinctValues;
        }

        /// <summary>
        /// Gets bucket size:
        /// 1, 2, 3, 4, 5, 6
        /// 10, 25, 50, 75, 100
        /// 128, 256, 512, 1024, 2048, ...
        /// </summary>
        /// <param name="originalLength">Original length.</param>
        /// <returns>Bucket size.</returns>
        private static int GetBucketSize(int originalLength) => originalLength
            switch
            {
                <= 6 => originalLength,
                <= 10 => 10,
                <= 25 => 25,
                <= 50 => 50,
                <= 75 => 75,
                <= 100 => 100,
                _ => (int)Math.Pow(2, Math.Ceiling(Math.Log(originalLength, 2)))
            };
    }
}
