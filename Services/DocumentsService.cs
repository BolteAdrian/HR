using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR.Models;
using HR.Repository;
using HR.Utils;
using Microsoft.EntityFrameworkCore;

namespace HR.Services
{
    /// <summary>
    /// Provides operations for managing documents.
    /// </summary>
    public class DocumentsService
    {
        private readonly modelContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsService"/> class.
        /// </summary>
        /// <param name="context">The database context used for document operations.</param>
        public DocumentsService(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all documents from the database.
        /// </summary>
        /// <returns>A list of <see cref="Documents"/> objects.</returns>
        public async Task<List<Documents>> GetAllDocumentsAsync()
        {
            try
            {
                return await _context.Documents.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a document by its ID.
        /// </summary>
        /// <param name="id">The ID of the document to retrieve.</param>
        /// <returns>The <see cref="Documents"/> with the specified ID, or null if not found.</returns>
        public async Task<Documents> GetDocumentByIdAsync(long? id)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(m => m.Id == id);
                if (document == null)
                {
                    throw new Exception(Constants.DOCUMENT.NotFound);
                }
                return document;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Adds a new document to the database.
        /// </summary>
        /// <param name="document">The document to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddDocumentAsync(Documents document)
        {
            try
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.DOCUMENT.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Updates an existing document in the database.
        /// </summary>
        /// <param name="document">The document with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateDocumentAsync(Documents document)
        {
            try
            {
                _context.Update(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.DOCUMENT.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Deletes a document from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the document to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteDocumentAsync(long id)
        {
            try
            {
                var document = await _context.Documents.FindAsync(id);
                if (document == null)
                {
                    throw new Exception(Constants.DOCUMENT.NotFound);
                }

                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.DOCUMENT.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Checks if a document exists in the database.
        /// </summary>
        /// <param name="id">The ID of the document to check.</param>
        /// <returns><c>true</c> if the document exists; otherwise, <c>false</c>.</returns>
        public bool DocumentExists(long id)
        {
            try
            {
                return _context.Documents.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Constants.InternalServerError, ex.Message));
            }
        }
    }
}
