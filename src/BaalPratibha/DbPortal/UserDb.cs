using System;
using BaalPratibha.Models;
using BaalPratibha.Models.ViewModels;
using Dapper;

namespace BaalPratibha.DbPortal
{
    public class UserDb : BaseDb
    {
        public UserDb(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public Models.User GetUserById(int id)
        {
            const string sql = "SELECT id, user_name as UserName, password, full_name as fullname, role  FROM core.users WHERE id=@id;";
            return Connection.QueryFirst<User>(sql, new { id = id });
        }

        public Models.User GetUserByUserName(string userName)
        {
            const string sql = "SELECT id, user_name as UserName, password, full_name as fullname, role FROM core.users WHERE user_name=@UserName;";
            return Connection.QueryFirstOrDefault<User>(sql, new { UserName = userName });
        }

        public int UpdateUser(User user)
        {
            const string sql =
                "UPDATE core.users SET user_name = @UserName, password = @Password, full_name = @FullName, role=@Role WHERE id = @Id";
            return Connection.Execute(sql, user);
        }

        public long Insert(User user)
        {
            const string sql = @"INSERT INTO core.users (user_name, password, full_name, role) 
                                                        VALUES(@UserName, @Password, @FullName, @Role) RETURNING id";
            return Connection.QueryFirstOrDefault<int>(sql, new { user.UserName, user.Password, user.FullName, Role = user.Role.ToString() });

        }

        public bool Delete(int userId)
        {
            const string sql = "DELETE FROM core.users WHERE id = @UserId";
            return Connection.Execute(sql, new { userId }) > 0;
        }

        internal int UpdateUser(string fullName, string userName)
        {
            const string sql =
                 "UPDATE core.users SET  full_name = @FullName WHERE user_name = @userName";
            return Connection.Execute(sql, new { UserName = userName, FullName = fullName });
        }
    }
}
