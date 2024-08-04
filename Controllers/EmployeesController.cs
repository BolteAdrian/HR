using System.Threading.Tasks;
using HR.DataModels;
using HR.Models;
using HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace HR_CV.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeesService _employeeService;

        public EmployeesController(EmployeesService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Employees
        /// <summary>
        /// Retrieves and displays a paginated list of employees with optional filtering and sorting.
        /// </summary>
        /// <param name="filter">Optional filter to apply to the employee list.</param>
        /// <param name="page">The page number to display (default is 1).</param>
        /// <param name="sortExpression">The sorting criteria (default is "EmployeeId").</param>
        /// <returns>Returns a view that displays the list of employees.</returns>
        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "EmployeeId")
        {
            var employeesViewModel = await _employeeService.GetEmployeesAsync(filter, page, sortExpression);
            employeesViewModel.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(employeesViewModel);
        }

        // GET: Employees/AddOrEdit/0 (Insert) or Employees/AddOrEdit/:id (Update)
        /// <summary>
        /// Displays a form to add a new employee or edit an existing one based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the employee to edit (0 for new employee).</param>
        /// <returns>Returns a view for adding or editing an employee.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Employees());
            }
            else
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
        }

        // POST: Employees/AddOrEdit
        /// <summary>
        /// Handles the creation or update of an employee based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the employee to update (0 for new employee).</param>
        /// <param name="employee">The employee data to be created or updated.</param>
        /// <returns>Returns a JSON result indicating whether the operation was successful and includes the updated view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,EmployeeId,Name,OrganizationId,EmploymentDate,Email,Team,CompanyShortName")] Employees employee)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _employeeService.AddEmployeeAsync(employee);
                    TempData["AlertMessage"] = "Inserted successfully";
                }
                else
                {
                    try
                    {
                        await _employeeService.UpdateEmployeeAsync(employee);
                        TempData["AlertMessage"] = "Updated successfully";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_employeeService.EmployeeExists(employee.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_ViewAll", await _employeeService.GetEmployeesAsync(null, 1, "Name"))
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AddOrEdit", employee)
            });
        }

        // GET: Employees/ExportToExcel
        /// <summary>
        /// Exports the list of employees to an Excel file.
        /// </summary>
        /// <returns>Returns a file result containing the Excel file.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportToExcel()
        {
            var excelContent = await _employeeService.ExportEmployeesToExcelAsync();
            return File(excelContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
        }

        // GET: Employees/Details/:id
        /// <summary>
        /// Displays the details of a specific employee.
        /// </summary>
        /// <param name="id">The ID of the employee to be viewed.</param>
        /// <returns>Returns a view displaying the details of the employee.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        /// <summary>
        /// Displays a form to create a new employee.
        /// </summary>
        /// <returns>Returns a view that allows creating a new employee.</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        /// <summary>
        /// Handles the creation of a new employee.
        /// </summary>
        /// <param name="employee">The employee data to be created.</param>
        /// <returns>Redirects to the index view if successful, otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,Name,OrganizationId,EmploymentDate,Email,Team,CompanyShortName")] Employees employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employee);
                TempData["AlertMessage"] = "Inserted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/:id
        /// <summary>
        /// Displays a form to edit a specific employee.
        /// </summary>
        /// <param name="id">The ID of the employee to be edited.</param>
        /// <returns>Returns a view for editing the employee.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/:id
        /// <summary>
        /// Handles the update of a specific employee.
        /// </summary>
        /// <param name="id">The ID of the employee to be updated.</param>
        /// <param name="employee">The employee data to be updated.</param>
        /// <returns>Redirects to the index view if successful, otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,Name,OrganizationId,EmploymentDate,Email,Team,CompanyShortName")] Employees employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateEmployeeAsync(employee);
                    TempData["AlertMessage"] = "Updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_employeeService.EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // POST: Employees/DeleteEmployee
        /// <summary>
        /// Deletes a specific employee by ID and returns a JSON result indicating success.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to be deleted.</param>
        /// <returns>Returns a JSON result indicating whether the deletion was successful.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            await _employeeService.DeleteEmployeeAsync(employeeId);
            TempData["AlertMessage"] = "Deleted successfully";
            return Json(new { success = true });
        }
    }
}
