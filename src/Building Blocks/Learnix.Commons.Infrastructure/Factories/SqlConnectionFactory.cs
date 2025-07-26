using Learnix.Commons.Application.Factories;
using Microsoft.Data.SqlClient;

namespace Learnix.Commons.Infrastructure.Factories
{
    public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public SqlConnection Create() => new(connectionString);
    }
}