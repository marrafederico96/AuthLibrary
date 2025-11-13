using AuthLibrary.Models;
using Microsoft.Data.SqlClient;

namespace AuthLibrary
{
    public class SqlDbHandler(string connectionString, TokenSettings tokenSettings)
    {
        private readonly string _connectionString = connectionString;

        public async Task<string> LoginUser(string email, string password, string role)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT PasswordHash, PasswordSalt FROM Customers WHERE LOWER(TRIM(EmailAddress)) = @Email";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email.Trim().ToLowerInvariant());

            var reader = await command.ExecuteReaderAsync();

            string passwordHash = string.Empty;
            string passwordSalt = string.Empty;

            if (!reader.HasRows)
            {
                throw new ArgumentException("Wrong credentials");
            }
            else
            {
                while (reader.Read())
                {
                    passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                    passwordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt"));
                }
            }

            var token = TokenService.GenerateJwtToken(email, role, tokenSettings);
            return token;
        }
    }
}
