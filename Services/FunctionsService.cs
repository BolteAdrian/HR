using System;
using System.Collections.Generic;
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
    /// Provides operations for managing functions.
    /// </summary>
    public class FunctionsService
    {
        private readonly modelContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionsService"/> class.
        /// </summary>
        /// <param name="context">The database context used for function operations.</param>
        public FunctionsService(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of functions, optionally filtered and sorted.
        /// </summary>
        /// <param name="filter">The filter to apply to function names.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="sortExpression">The sort expression to apply.</param>
        /// <returns>A paginated list of <see cref="Functions"/> objects.</returns>
        public async Task<PagingList<Functions>> GetFunctionsAsync(string filter, int page, string sortExpression)
        {
            try
            {
                var qry = _context.Functions.AsNoTracking().OrderBy(p => p.Id).AsQueryable();

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
        /// Retrieves a function by its ID.
        /// </summary>
        /// <param name="id">The ID of the function to retrieve.</param>
        /// <returns>The <see cref="Functions"/> with the specified ID, or null if not found.</returns>
        public async Task<Functions> GetFunctionByIdAsync(long? id)
        {
            try
            {
                var function = await _context.Functions.FirstOrDefaultAsync(m => m.Id == id);
                if (function == null)
                {
                    throw new Exception(Constants.FUNCTION.NotFound);
                }
                return function;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Adds a new function to the database.
        /// </summary>
        /// <param name="function">The function to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddFunctionAsync(Functions function)
        {
            try
            {
                _context.Functions.Add(function);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.FUNCTION.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Updates an existing function in the database.
        /// </summary>
        /// <param name="function">The function with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateFunctionAsync(Functions function)
        {
            try
            {
                _context.Update(function);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.FUNCTION.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Deletes a function from the database by its ID. Before deleting, updates all references to the function 
        /// in candidates and interviews to point to a default function with ID = 1.
        /// </summary>
        /// <param name="id">The ID of the function to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteFunctionAsync(long id)
        {
            try
            {
                var function = await _context.Functions.FindAsync(id);
                if (function == null)
                {
                    throw new Exception(Constants.FUNCTION.NotFound);
                }

                // Update candidates function apply with the given function id to function id = 1
                var candidatesFunctionApplyToUpdate = _context.Candidates.Where(c => c.FunctionApply == id);
                await foreach (var candidate in candidatesFunctionApplyToUpdate.AsAsyncEnumerable())
                {
                    candidate.FunctionApply = 1;
                }

                // Update candidates function match with the given function id to function id = 1
                var candidatesFunctionMatchToUpdate = _context.Candidates.Where(c => c.FunctionMatch == id);
                await foreach (var candidate in candidatesFunctionMatchToUpdate.AsAsyncEnumerable())
                {
                    candidate.FunctionMatch = 1;
                }

                // Update interviews with the given function id to function id = 1
                var interviewsToUpdate = _context.Interviews.Where(i => i.FunctionApply == id);
                await foreach (var interview in interviewsToUpdate.AsAsyncEnumerable())
                {
                    interview.FunctionApply = 1;
                }

                // Remove the function
                _context.Functions.Remove(function);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.FUNCTION.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Checks if a function exists in the database.
        /// </summary>
        /// <param name="id">The ID of the function to check.</param>
        /// <returns><c>true</c> if the function exists; otherwise, <c>false</c>.</returns>
        public bool FunctionExists(long id)
        {
            try
            {
                return _context.Functions.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a list of all departments.
        /// </summary>
        /// <returns>A list of <see cref="Departments"/> objects.</returns>
        public List<Departments> GetDepartments()
        {
            try
            {
                return _context.Departments.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }
    }
}
