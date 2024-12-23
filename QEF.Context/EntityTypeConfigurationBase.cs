using Microsoft.EntityFrameworkCore;
using System;

namespace QEF.Context
{
    /// <summary>
    /// Base Configuration for <see cref="IEntityTypeConfiguration{T}"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EntityTypeConfigurationBase"/> class.
    /// </remarks>
    /// <param name="tableName">Table name.</param>
    /// <param name="schema">Schema name.</param>
    public class EntityTypeConfigurationBase(string tableName, string? schemaName)
    {
        /// <summary>
        /// Table name.
        /// </summary>
        protected string TableName { get; } = string.IsNullOrWhiteSpace(tableName) ?
            throw new ArgumentException(null, nameof(tableName)) : tableName;

        /// <summary>
        /// Schema name.
        /// </summary>
        protected string? SchemaName { get; } = schemaName;
    }
}