using MySqlConnector;

namespace exam_preparing
{
    public class DatabaseService(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public async Task SaveResultAsync(IntegrationResult result)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new MySqlCommand("INSERT INTO Results (A, B, Result) VALUES (@a, @b, @result)", connection);
            command.Parameters.AddWithValue("@a", result.A);
            command.Parameters.AddWithValue("@b", result.B);
            command.Parameters.AddWithValue("@result", result.Result);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<IntegrationResult>> GetResultsAsync()
        {
            var results = new List<IntegrationResult>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM Results", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(new IntegrationResult
                    {
                        Id = reader.GetInt32(0),
                        A = reader.GetDouble(1),
                        B = reader.GetDouble(2),
                        Result = reader.GetDouble(3)
                    });
                }
            }

            return results;
        }
    }
}
