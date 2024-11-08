using System;
using System.Collections.Generic;

public class CompanyLicense
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string LicenseKey { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    // Add a list to store the sub-companies associated with the license
    public List<SubCompany> SubCompanies { get; set; }
}


    public class SubCompany
    {
        public int SubCompanyId { get; set; }
        public string SubCompanyName { get; set; }
        public string LicenseKey { get; set; }
        public string ConnectionStringOnline { get; set; }
        public string ConnectionStringOffline { get; set; }
    }


