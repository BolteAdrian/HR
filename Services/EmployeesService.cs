using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using HR.Models;
using HR.Repository;
using HR.Utils;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace HR.Services
{
    /// <summary>
    /// Provides operations for managing employees.
    /// </summary>
    public class EmployeesService
    {
        private readonly modelContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesService"/> class.
        /// </summary>
        /// <param name="context">The database context used for employee operations.</param>
        public EmployeesService(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of employees, optionally filtered and sorted.
        /// </summary>
        /// <param name="filter">The filter to apply to employee names, teams, or company short names.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="sortExpression">The sort expression to apply.</param>
        /// <returns>A paginated list of <see cref="Employees"/> objects.</returns>
        public async Task<PagingList<Employees>> GetEmployeesAsync(string filter, int page, string sortExpression)
        {
            try
            {
                var qry = _context.Employees.AsNoTracking().OrderBy(p => p.Id).AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry = qry.Where(p => p.Name.Contains(filter) || p.Team.Contains(filter) || p.CompanyShortName.Contains(filter));
                }

                return await PagingList.CreateAsync(qry, 10, page, sortExpression, "Name");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The <see cref="Employees"/> with the specified ID, or null if not found.</returns>
        public async Task<Employees> GetEmployeeByIdAsync(int? id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
                if (employee == null)
                {
                    throw new Exception(Constants.EMPLOYEE.NotFound);
                }
                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <param name="employee">The employee to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddEmployeeAsync(Employees employee)
        {
            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.EMPLOYEE.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Updates an existing employee in the database.
        /// </summary>
        /// <param name="employee">The employee with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateEmployeeAsync(Employees employee)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.EMPLOYEE.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Deletes an employee from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    throw new Exception(Constants.EMPLOYEE.NotFound);
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.EMPLOYEE.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Checks if an employee exists in the database.
        /// </summary>
        /// <param name="id">The ID of the employee to check.</param>
        /// <returns><c>true</c> if the employee exists; otherwise, <c>false</c>.</returns>
        public bool EmployeeExists(int id)
        {
            try
            {
                return _context.Employees.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Exports all employees to an Excel file.
        /// </summary>
        /// <returns>A byte array representing the Excel file.</returns>
        public async Task<byte[]> ExportEmployeesToExcelAsync()
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Employees");
                    var currentRow = 1;

                    // Set up the header row.
                    worksheet.Cell(currentRow, 1).Value = "Id";
                    worksheet.Cell(currentRow, 2).Value = "EmployeeId";
                    worksheet.Cell(currentRow, 3).Value = "Name";
                    worksheet.Cell(currentRow, 4).Value = "OrganizationId";
                    worksheet.Cell(currentRow, 5).Value = "EmploymentDate";
                    worksheet.Cell(currentRow, 6).Value = "Email";
                    worksheet.Cell(currentRow, 7).Value = "Team";
                    worksheet.Cell(currentRow, 8).Value = "CompanyShortName";

                    // Populate the worksheet with employee data.
                    foreach (var employee in await _context.Employees.ToListAsync())
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = employee.Id;
                        worksheet.Cell(currentRow, 2).Value = employee.EmployeeId;
                        worksheet.Cell(currentRow, 3).Value = employee.Name;
                        worksheet.Cell(currentRow, 4).Value = employee.OrganizationId;
                        worksheet.Cell(currentRow, 5).Value = employee.EmploymentDate?.ToShortDateString();
                        worksheet.Cell(currentRow, 6).Value = employee.Email;
                        worksheet.Cell(currentRow, 7).Value = employee.Team;
                        worksheet.Cell(currentRow, 8).Value = employee.CompanyShortName;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.EMPLOYEE.ExportFailed, ex);
            }
        }
    }
}
