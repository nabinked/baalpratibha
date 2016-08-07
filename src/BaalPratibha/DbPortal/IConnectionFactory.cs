using System.Data;

namespace BaalPratibha.DbPortal
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
