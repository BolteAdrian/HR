using System;
using System.Linq;
using System.Threading.Tasks;
using HR.Models;
using HR.Repository;
using HR.Utils;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace HR.Services
{
    /// <summary>
    /// Provides operations for managing departments.
    /// </summary>
    public class DepartmentsService
    {
        private readonly modelContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentsService"/> class.
        /// </summary>
        /// <param name="context">The database context used for department operations.</param>
        public DepartmentsService(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of departments with optional filtering and sorting.
        /// </summary>
        /// <param name="filter">A filter string to apply to the list of departments.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="sortExpression">The sort expression to apply to the list.</param>
        /// <returns>A <see cref="PagingList{Department}"/> containing the filtered and sorted list of departments.</returns>
        public async Task<PagingList<Departments>> GetDepartmentsAsync(string filter, int page, string sortExpression)
        {
            try
            {
                var qry = _context.Departments.AsNoTracking().OrderBy(p => p.Id).AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry = qry.Where(p => p.Name.Contains(filter));
                }

                return await PagingList.CreateAsync(qry, 10, page, sortExpression, "Name");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to retrieve.</param>
        /// <returns>The <see cref="Departments"/> with the specified ID, or null if not found.</returns>
        public async Task<Departments> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var department = await _context.Departments.SingleOrDefaultAsync(x => x.Id == id);
                if (department == null)
                {
                    throw new Exception(Constants.DEPARTMENT.NotFound);
                }
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Adds a new department to the database.
        /// </summary>
        /// <param name="department">The department to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddDepartmentAsync(Departments department)
        {
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.DEPARTMENT.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Updates an existing department in the database.
        /// </summary>
        /// <param name="department">The department with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateDepartmentAsync(Departments department)
        {
            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.DEPARTMENT.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Deletes a department from the database by its ID. Before deleting, updates all references to the department 
        /// in interviews and functions to point to a default department with ID = 1.
        /// </summary>
        /// <param name="id">The ID of the department to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteDepartmentAsync(int id)
        {
            try
            {
                var department = await _context.Departments.SingleOrDefaultAsync(x => x.Id == id);
                if (department == null)
                {
                    throw new Exception(Constants.DEPARTMENT.NotFound);
                }

                // Update interviews with the given department id to department id = 1
                var interviewsToUpdate = _context.Interviews.Where(i => i.DepartamentApply == id);
                await foreach (var interview in interviewsToUpdate.AsAsyncEnumerable())
                {
                    interview.DepartamentApply = 1;
                }

                // Update functions with the given department id to department id = 1
                var functionsToUpdate = _context.Functions.Where(i => i.DepartmentId == id);
                await foreach (var interview in functionsToUpdate.AsAsyncEnumerable())
                {
                    interview.DepartmentId = 1;
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.DEPARTMENT.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Checks if a department exists in the database.
        /// </summary>
        /// <param name="id">The ID of the department to check.</param>
        /// <returns><c>true</c> if the department exists; otherwise, <c>false</c>.</returns>
        public bool DepartmentExists(long id)
        {
            try
            {
                return _context.Departments.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }
    }
}
