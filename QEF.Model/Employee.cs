using System;

namespace QEF.Model
{
    /// <summary>
    /// Employee entity.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Employee ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Indicates that the employee is working currently.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        public DateTimeOffset BirthDate { get; set; }

        /// <summary>
        /// Department ID.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Employee's department.
        /// </summary>
        public virtual Department Department { get; set; } = null!;
    }
}