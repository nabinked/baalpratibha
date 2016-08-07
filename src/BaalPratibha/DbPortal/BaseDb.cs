using System.Data;

namespace BaalPratibha.DbPortal
{
    public class BaseDb
    {
        protected readonly IDbConnection Connection;

        public BaseDb(IConnectionFactory connectionFactory)
        {
            Connection = connectionFactory.CreateConnection();
        }
    }
}
