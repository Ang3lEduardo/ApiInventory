using System.Data;
using Microsoft.Data.SqlClient;

namespace Inventory.Infrastructure.Persistence
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}