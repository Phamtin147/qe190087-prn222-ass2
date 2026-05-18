using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace FUNewsManagementSystem.Services.Services;

public sealed class AuthService
{
    private readonly AccountRepository _accounts;
    private readonly IConfiguration _configuration;

    public AuthService(AccountRepository accounts, IConfiguration configuration)
    {
        _accounts = accounts;
        _configuration = configuration;
    }

    public SystemAccount? Authenticate(string email, string password)
    {
        var adminEmail = _configuration["AdminAccount:Email"];
        var adminPassword = _configuration["AdminAccount:Password"];
        if (string.Equals(email, adminEmail, StringComparison.OrdinalIgnoreCase) && password == adminPassword)
        {
            return new SystemAccount
            {
                AccountId = 0,
                AccountName = "Administrator",
                AccountEmail = adminEmail ?? string.Empty,
                AccountPassword = adminPassword ?? string.Empty,
                AccountRole = AccountRole.Admin
            };
        }

        var account = _accounts.GetByEmail(email);
        return account is not null && account.AccountPassword == password ? account : null;
    }
}
