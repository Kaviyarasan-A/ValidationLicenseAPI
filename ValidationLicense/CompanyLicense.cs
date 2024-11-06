using System;

public class CompanyLicense
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string LicenseKey { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}
