using System.Collections.Generic;
using System.Linq;
using BaalPratibha.Models;
using BaalPratibha.Models.ViewModels;
using Dapper;

namespace BaalPratibha.DbPortal
{
    public class ContestantDb : BaseDb
    {
        public ContestantDb(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public Models.ContestantDetail GetContestantByUserName(string userName)
        {
            const string sql = "SELECT id, user_name as UserName, password, full_name as FullName, age, suburb, about_me as aboutme  FROM core.contestants WHERE user_name = @UserName";

            return Connection.QueryFirst<Models.ContestantDetail>(sql, new { UserName = userName });
        }

        public Models.ContestantDetail GetContestantById(int id)
        {
            const string sql = "SELECT id, user_name as UserName, password, full_name as FullName, age, suburb, about_me as aboutme  FROM core.contestants WHERE id = @Id";

            return Connection.QueryFirst<Models.ContestantDetail>(sql, new { Id = id });
        }

        public Models.ContestantDetail GetContestantDetailByUserId(int userId)
        {
            const string sql = "SELECT user_id as UserId, age, suburb, parents_name as ParentsName, contact, email, about_me as aboutme FROM core.contestant_details WHERE user_id = @UserId";

            return Connection.QueryFirstOrDefault<Models.ContestantDetail>(sql, new { UserId = userId });
        }

        public int Update(ContestantDetail contestantDetail)
        {
            const string sql = "UPDATE core.contestant_details SET age = @Age,suburb = @Suburb, parents_name = @ParentsName, contact = @Contact , email = @Email, about_me =@AboutMe WHERE user_id = @UserId;";

            return Connection.Execute(sql, contestantDetail);
        }

        public int Insert(ContestantDetail contestantDetail)
        {
            const string sql = @"INSERT INTO core.contestant_details (user_id, age,suburb, parents_name, contact, email, about_me) 
                                                        VALUES(@UserId, @Age, @Suburb, @ParentsName, @Contact, @Email, @AboutMe) RETURNING user_id";
            return Connection.QueryFirstOrDefault<int>(sql, contestantDetail);
        }

        public IList<ContestantView> GetAllContestants()
        {
            const string sql = "SELECT * FROM core.contestant_view;";
            return Connection.Query<ContestantView>(sql).ToList();
        }

        public ContestantView GetContestantView(long id)
        {
            const string sql = "SELECT * FROM core.contestant_view WHERE id=@Id;";
            return Connection.QueryFirstOrDefault<ContestantView>(sql, new { Id = id });
        }

        public bool DeleteByUserId(int userId)
        {
            const string sql = "DELETE FROM core.contestant_details WHERE user_id=@UserId;";
            return Connection.Execute(sql, new { userId }) > 0;
        }

        public ContestantView GetContestantViewByUserName(string userName)
        {
            const string sql = "SELECT * FROM core.contestant_view WHERE username=@UserName;";
            return Connection.QueryFirstOrDefault<ContestantView>(sql, new { UserName = userName });
        }
    }
}
