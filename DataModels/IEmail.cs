﻿using Microsoft.AspNetCore.Http;

namespace HR.DataModels
{
    public class IEmail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile Attachment { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
