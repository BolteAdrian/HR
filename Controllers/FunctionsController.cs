using System.Threading.Tasks;
using HR.DataModels;
using HR.Models;
using HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace HR.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly FunctionsService _functionService;

        public FunctionsController(FunctionsService functionService)
        {
            _functionService = functionService;
        }

        // GET: Functions
        /// <summary>
        /// Retrieves and displays a paginated list of functions with optional filtering and sorting.
        /// </summary>
        /// <param name="filter">Optional filter to apply to the function list.</param>
        /// <param name="page">The page number to display (default is 1).</param>
        /// <param name="sortExpression">The sorting criteria (default is "Id").</param>
        /// <returns>Returns a view that displays the list of functions.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "Id")
        {
            var functionsViewModel = await _functionService.GetFunctionsAsync(filter, page, sortExpression);
            functionsViewModel.RouteValue = new RouteValueDictionary { { "filter", filter } };
            ViewData["DepartmentId"] = _functionService.GetDepartments();
            return View(functionsViewModel);
        }

        // POST: Functions/DeleteFunction
        /// <summary>
        /// Deletes a specific function by ID and returns a JSON result indicating success.
        /// </summary>
        /// <param name="functionId">The ID of the function to be deleted.</param>
        /// <returns>Returns a JSON result indicating whether the deletion was successful.</returns>
        public async Task<JsonResult> DeleteFunction(long functionId)
        {
            await _functionService.DeleteFunctionAsync(functionId);
            TempData["AlertMessage"] = "Deleted successfully";
            return Json(new { success = true });
        }

        // GET: Functions/Create
        /// <summary>
        /// Displays a form to create a new function.
        /// </summary>
        /// <returns>Returns a view that allows creating a new function.</returns>
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_functionService.GetDepartments(), "Id", "Name");
            return View();
        }

        // GET: Functions/AddOrEdit
        /// <summary>
        /// Displays a form to add or edit a function based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the function to edit (0 for new function).</param>
        /// <returns>Returns a view for adding or editing a function.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewData["DepartmentId"] = new SelectList(_functionService.GetDepartments(), "Id", "Name");
                return View(new Functions());
            }
            else
            {
                var function = await _functionService.GetFunctionByIdAsync(id);
                ViewData["DepartmentId"] = new SelectList(_functionService.GetDepartments(), "Id", "Name");
                if (function == null)
                {
                    return NotFound();
                }
                return View(function);
            }
        }

        // POST: Functions/AddOrEdit
        /// <summary>
        /// Handles the creation or update of a function based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the function to update (0 for new function).</param>
        /// <param name="function">The function data to be created or updated.</param>
        /// <returns>Returns a JSON result indicating whether the operation was successful and includes the updated view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id, Name, DepartmentId")] Functions function)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _functionService.AddFunctionAsync(function);
                    TempData["AlertMessage"] = "Inserted successfully";
                }
                else
                {
                    try
                    {
                        await _functionService.UpdateFunctionAsync(function);
                        TempData["AlertMessage"] = "Updated successfully";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_functionService.FunctionExists(function.Id))
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
                    html = Helper.RenderRazorViewToString(this, "_ViewAll", await _functionService.GetFunctionsAsync(null, 1, "Name"))
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AddOrEdit", function)
            });
        }

        // POST: Functions/Create
        /// <summary>
        /// Handles the creation of a new function.
        /// </summary>
        /// <param name="function">The function data to be created.</param>
        /// <returns>Redirects to the index view if successful, otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentId")] Functions function)
        {
            if (ModelState.IsValid)
            {
                await _functionService.AddFunctionAsync(function);
                TempData["AlertMessage"] = "Inserted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(function);
        }

        // GET: Functions/Edit/:id
        /// <summary>
        /// Displays a form to edit a specific function.
        /// </summary>
        /// <param name="id">The ID of the function to be edited.</param>
        /// <returns>Returns a view for editing the function.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _functionService.GetFunctionByIdAsync(id);
            if (function == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_functionService.GetDepartments(), "Id", "Name");
            return View(function);
        }

        // POST: Functions/Edit/:id
        /// <summary>
        /// Handles the update of a specific function.
        /// </summary>
        /// <param name="id">The ID of the function to be updated.</param>
        /// <param name="function">The function data to be updated.</param>
        /// <returns>Redirects to the index view if successful, otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,DepartmentId")] Functions function)
        {
            if (id != function.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _functionService.UpdateFunctionAsync(function);
                    TempData["AlertMessage"] = "Updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_functionService.FunctionExists(function.Id))
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
            return View(function);
        }

        // Private method to check if a function exists by ID
        /// <summary>
        /// Checks if a function exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the function to check.</param>
        /// <returns>Returns true if the function exists, otherwise false.</returns>
        private bool FunctionExists(long id)
        {
            return _functionService.FunctionExists(id);
        }
    }
}
