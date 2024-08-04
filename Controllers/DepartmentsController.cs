using System.Threading.Tasks;
using HR.DataModels;
using HR.Models;
using HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace HR.Controllers
{
    /// <summary>
    /// Handles CRUD operations and other functionalities related to departments.
    /// </summary>
    public class DepartmentsController : Controller
    {
        private readonly DepartmentsService _departmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentsController"/> class.
        /// </summary>
        /// <param name="departmentService">The service used for department-related operations.</param>
        public DepartmentsController(DepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Displays a list of departments with optional filtering, pagination, and sorting.
        /// </summary>
        /// <param name="filter">A filter string to apply to the list of departments.</param>
        /// <param name="page">The page number to display.</param>
        /// <param name="sortExpression">The sort expression for the list.</param>
        /// <returns>A view displaying the list of departments.</returns>
        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "Id")
        {
            var model = await _departmentService.GetDepartmentsAsync(filter, page, sortExpression);

            if (model == null)
            {
                return NotFound();
            }

            model.RouteValue = model.RouteValue ?? new RouteValueDictionary();
            model.RouteValue["filter"] = filter;

            return View(model);
        }

        /// <summary>
        /// Displays the form for creating or editing a department.
        /// </summary>
        /// <param name="id">The ID of the department to edit (if applicable).</param>
        /// <returns>A view with the department form.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                // New department, return empty form
                return View(new Departments());
            }
            else
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                if (department == null)
                {
                    // Department not found, return NotFound
                    return NotFound();
                }
                // Existing department, return form pre-filled with department data
                return View(department);
            }
        }

        /// <summary>
        /// Handles the creation or update of a department.
        /// </summary>
        /// <param name="id">The ID of the department to update (if applicable).</param>
        /// <param name="department">The department details to be saved.</param>
        /// <returns>A JSON result indicating success or failure of the operation, along with the rendered view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,Name")] Departments department)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    // Add new department
                    await _departmentService.AddDepartmentAsync(department);
                    TempData["AlertMessage"] = "Inserted with success";
                }
                else
                {
                    try
                    {
                        // Update existing department
                        await _departmentService.UpdateDepartmentAsync(department);
                        TempData["AlertMessage"] = "Updated with success";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        // Handle concurrency issues
                        if (!_departmentService.DepartmentExists(department.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                // Return JSON result with updated view
                var updatedDepartments = await _departmentService.GetDepartmentsAsync("", 1, "Id");
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", updatedDepartments) });
            }
            // Return JSON result with form errors
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", department) });
        }

        /// <summary>
        /// Deletes a department by its ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department to be deleted.</param>
        /// <returns>A JSON result indicating success or failure of the operation.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DeleteDepartment(int departmentId)
        {
            await _departmentService.DeleteDepartmentAsync(departmentId);
            TempData["AlertMessage"] = "Deleted with success";
            return Json(true);
        }
    }
}
