using System.Collections.Generic;

namespace QEF.Model
{
    /// <summary>
    /// Company entity.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Company ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Email address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Company departments.
        /// </summary>
        public virtual List<Department> Departments { get; } = new List<Department>();
    }
}
