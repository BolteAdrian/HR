using System.Threading.Tasks;
using HR.Models;
using HR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly DocumentsService _documentService;

        public DocumentsController(DocumentsService documentService)
        {
            _documentService = documentService;
        }

        // GET: Documents
        /// <summary>
        /// Retrieves and displays all documents.
        /// </summary>
        /// <returns>Returns the view displaying the list of all documents.</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _documentService.GetAllDocumentsAsync());
        }

        // GET: Documents/Details/:id
        /// <summary>
        /// Displays the details of a document with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the document to be viewed.</param>
        /// <returns>Returns the view displaying the details of the document, or NotFound if the document does not exist.</returns>
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        /// <summary>
        /// Displays a form to create a new document.
        /// </summary>
        /// <returns>Returns the view for creating a new document.</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        /// <summary>
        /// Handles the creation of a new document.
        /// </summary>
        /// <param name="document">The document model containing the new document data.</param>
        /// <returns>Redirects to the index action after creating the document, or redisplays the form if an error occurs.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateAdded,Candidate,Observation")] Documents document)
        {
            if (ModelState.IsValid)
            {
                await _documentService.AddDocumentAsync(document);
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/:id
        /// <summary>
        /// Displays a form to edit an existing document with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the document to be edited.</param>
        /// <returns>Returns the view for editing the document, or NotFound if the document does not exist.</returns>
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/:id
        /// <summary>
        /// Handles the update of an existing document with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the document to be updated.</param>
        /// <param name="document">The document model containing the updated document data.</param>
        /// <returns>Redirects to the index action after updating the document, or redisplays the form if an error occurs.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,DateAdded,Candidate,Observation")] Documents document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _documentService.UpdateDocumentAsync(document);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_documentService.DocumentExists(document.Id))
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
            return View(document);
        }

        // GET: Documents/Delete/:id
        /// <summary>
        /// Displays a confirmation form to delete a document with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>Returns the view for confirming the deletion of the document, or NotFound if the document does not exist.</returns>
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/:id
        /// <summary>
        /// Handles the deletion of a document with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>Redirects to the index action after deleting the document.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
