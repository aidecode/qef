using Microsoft.EntityFrameworkCore;
using QEF.Context.Dtos;
using QEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QEF.Context.Queries
{
    /// <summary>
    /// Entities queries.
    /// </summary>
    public static class EntitiesQueries
    {
        /// <summary>
        /// Select employees query.
        /// </summary>
        /// <param name="dbcontext"><see cref="DbContext"/>.</param>
        /// <param name="ids">Employees IDs.</param>
        /// <returns><see cref="IQueryable{Employee}"/>.</returns>
        public static IQueryable<Employee> SelectEmployeesQuery(
            DbContext dbcontext, IEnumerable<int> ids) => dbcontext.
            Set<Employee>().
            Where(e => ids.Contains(e.Id));

        /// <summary>
        /// Numbers of employees in departments by department IDs.
        /// </summary>
        /// <param name="dbcontext"><see cref="DbContext"/>.</param>
        /// <param name="departmentsIds">Departments IDs.</param>
        /// <returns><see cref="IQueryable{EntityCounter}"/>.</returns>
        public static IQueryable<EntityCounter> EmployeesCountInDepartmentsQuery(
            DbContext dbcontext, IEnumerable<int> departmentsIds) => dbcontext.
            Set<Employee>().
            GroupBy(g => g.DepartmentId).
            Select(e => new { Id = e.Key, Count = e.Count() }).
            Where(x => departmentsIds.Contains(x.Id)).
            Select(e => new EntityCounter(e.Id, e.Count));
    }
}
