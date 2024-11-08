using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

public class DatabaseService
{
    private readonly IDbConnection _connection;

    public DatabaseService(IDbConnection connection)
    {
        _connection = connection;
    }

    // Fetch company along with its sub-companies
    public async Task<CompanyLicense> GetLicenseByKeyAsync(string licenseKey)
    {
        var companyQuery = "SELECT * FROM Customer_Header WHERE LicenseKey = @LicenseKey";
        var subCompanyQuery = "SELECT * FROM Customer_details WHERE LicenseKey = @LicenseKey";

        // Fetch company info
        var company = await _connection.QueryFirstOrDefaultAsync<CompanyLicense>(companyQuery, new { LicenseKey = licenseKey });

        if (company == null)
        {
            return null;
        }

        // Fetch associated sub-companies
        var subCompanies = await _connection.QueryAsync<SubCompany>(subCompanyQuery, new { LicenseKey = licenseKey });
        company.SubCompanies = subCompanies.ToList();

        return company; // Return the company along with its sub-companies
    }

    // Fetch details of a sub-company by name
    public async Task<SubCompany> GetSubCompanyDetailsAsync(string subCompanyName)
    {
        var subCompanyQuery = @"
            SELECT * FROM Customer_details
            WHERE SubCompanyName = @SubCompanyName";

        var subCompanyDetails = await _connection.QueryFirstOrDefaultAsync<SubCompany>(subCompanyQuery, new { SubCompanyName = subCompanyName });

        return subCompanyDetails;
    }

    // Alternatively, you can fetch by SubCompanyId
    public async Task<SubCompany> GetSubCompanyDetailsByIdAsync(int subCompanyId)
    {
        var subCompanyQuery = @"
            SELECT * FROM Customer_details
            WHERE SubCompanyId = @SubCompanyId";

        var subCompanyDetails = await _connection.QueryFirstOrDefaultAsync<SubCompany>(subCompanyQuery, new { SubCompanyId = subCompanyId });

        return subCompanyDetails;
    }
}
