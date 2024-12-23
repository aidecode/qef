using System.Collections.Generic;

namespace QEF.Model
{
    /// <summary>
    /// Department entity.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Department ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Department number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Department name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Department description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Company ID.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Company of department.
        /// </summary>
        public virtual Company? Company { get; set; }

        /// <summary>
        /// Employees of department.
        /// </summary>
        public virtual List<Employee> Employees { get; } = new List<Employee>();
    }
}
