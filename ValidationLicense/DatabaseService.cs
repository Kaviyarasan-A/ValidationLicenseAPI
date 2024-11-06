using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using Dapper;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        // Update to match the key in appsettings.json
        _connectionString = configuration.GetConnectionString("Dbconnection");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Connection string 'Dbconnection' not found.");
        }
    }

    // Method to retrieve a license by LicenseKey
    public async Task<CompanyLicense> GetLicenseByKeyAsync(string licenceKey)
    {
        using (IDbConnection dbConnection = new SqlConnection(_connectionString))
        {
            string sqlQuery = "SELECT * FROM Table_1 WHERE LicenceKey = @LicenceKey";
            return await dbConnection.QueryFirstOrDefaultAsync<CompanyLicense>(sqlQuery, new { LicenceKey = licenceKey });
        }
    }
}
