using System.Collections.Generic;

namespace HR.Utils
{
    public class Constants
    {
        public static readonly List<string> OfferStatus = new List<string> { "Refused", "Signed" };

        public static readonly Dictionary<int, string> ModeApply = new Dictionary<int, string>
        {
            { 1, "Paper" },
            { 2, "Email" }
        };

        public static readonly Dictionary<int, string> Status = new Dictionary<int, string>
        {
            { 1, "Active" },
            { 2, "Inactive" }
        };

        //Universal messages
        public const string InternalServerError = "Internal server error";
        public static class CANDIDATE
        {
            public const string rootFolder = @"wwwroot\CVs\";
            public const string NoFilesUploaded = "No files uploaded";
            public const string PersonCvNotFound = "Candidate not found";
            public const string FileSizeExceedsLimit = "File size exceeds the limit for file";
            public const string DeletedWithSuccess = "Deleted with success";
            public const string AddCvBeforePerson = "You need to add a CV before adding this person.";
            public const string PersonAddedSuccessfully = "Person added successfully";
            public const string PersonUpdatedSuccessfully = "Person updated successfully";
            public const string FileNotFound = "File not found";
            public const string DocumentNotFound = "Document not found";
            public const string DocumentDeletionError = "Error deleting document";
            public const string FetchingDocumentsError = "Error fetching documents";
            public const string FetchingInterviewsError = "Error fetching interviews";
            public const string CreatingPersonCvError = "Error creating the candidate";
            public const string UpdatingPersonCvError = "Error updating the candidate";
        }
        public static class INTERVIEW
        {
            public const string NotFound = "The requested resource was not found.";
            public const string DeleteSuccess = "Deleted with success";
            public const string InvalidModel = "The model state is invalid.";
            public const string OperationFailed = "The operation failed.";
            public const string OperationSuccess = "Operation completed successfully.";
        }
        public static class EMAIL
        {
            public const string EmailSentSuccess = "Email sent successfully.";
            public const string EmailSendFailed = "Failed to send the email. Please try again later.";
        }
        public static class PREDICTION
        {
            public const string NotFound = "The requested resource was not found.";
            public const string OperationFailed = "The operation failed.";
            public const string OperationSuccess = "Operation completed successfully.";
            public const string InvalidModel = "The model state is invalid.";
            public const string NoDataFound = "No data found.";
        }
        public static class DEPARTMENT
        {
            public const string NotFound = "The requested department was not found.";
            public const string OperationSuccess = "Operation completed successfully.";
            public const string OperationFailed = "The operation failed.";
        }
        public static class DOCUMENT
        {
            public const string NotFound = "The requested document was not found.";
            public const string OperationSuccess = "Operation completed successfully.";
            public const string OperationFailed = "The operation failed.";
        }
        public static class EMPLOYEE
        {
            public const string NotFound = "The requested employee was not found.";
            public const string OperationSuccess = "Operation completed successfully.";
            public const string OperationFailed = "The operation failed.";
            public const string ExportFailed = "Failed to export data to Excel.";
        }
        public static class FUNCTION
        {
            public const string NotFound = "The requested function was not found.";
            public const string OperationSuccess = "Operation completed successfully.";
            public const string OperationFailed = "The operation failed.";
        }
        public static class JOB_APPLICATION
        {
            public const string NotFound = "The requested resource was not found.";
            public const string OperationSuccess = "Operation completed successfully.";
            public const string OperationFailed = "The operation failed.";
        }
    }
}
