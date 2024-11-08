using System;

namespace Forms.Models
{
    public class LicenseValidationResult
    {
        public string Message { get; set; }
        public string CompanyName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
