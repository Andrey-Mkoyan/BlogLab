using BlogLab.Models.Account;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Repasitory
{
    internal class AccountRepository : IAccountRepository
    {

        private readonly IConfiguration _config;

        public AccountRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
           cancellationToken.ThrowIfCancellationRequested();

            var dataTabel = new DataTable();
            dataTabel.Columns.Add("Username",  typeof(string));
            dataTabel.Columns.Add("NormalizedUsername", typeof(string));
            dataTabel.Columns.Add("Email", typeof(string));
            dataTabel.Columns.Add("NormalizedEmail", typeof(string));
            dataTabel.Columns.Add("Fullname", typeof(string));
            dataTabel.Columns.Add("PasswordHash", typeof(string));

            dataTabel.Rows.Add(
                user.Username,
                user.NormalizedUsername,
                user.Email,
                user.NormalizedEmail,
                user.Fullname,
                user.PasswordHash
                );

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(cancellationToken);

                await connection.ExecuteAsync("Account_Insert",
                    new {Account = dataTabel.AsTableValuedParameter("dbo.AccountType")}, commandType : CommandType.StoredProcedure);
            }

            return IdentityResult.Success;
        }

        public async Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUserIdentity applicationUser;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync(cancellationToken);

                applicationUser = await connection.QuerySingleOrDefaultAsync<ApplicationUserIdentity>(
                    "Account_GetByUsername", new { NormalizedUsername = normalizedUsername },commandType: CommandType.StoredProcedure);
            }

            return applicationUser;
        }
    }

   
}
