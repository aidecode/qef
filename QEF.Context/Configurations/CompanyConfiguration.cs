using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QEF.Model;

namespace QEF.Context.Configurations
{
    /// <summary>
    /// Configuration for <see cref="Company"/>.
    /// </summary>
    public class CompanyConfiguration(string tableName, string? schema) :
        EntityTypeConfigurationBase(tableName, schema), IEntityTypeConfiguration<Company>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.HasKey(e => e.Id);

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired(false);
        }
    }
}
