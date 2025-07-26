using Microsoft.Data.SqlClient;

namespace Learnix.Commons.Application.Factories
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}