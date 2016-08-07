using System.Security.Cryptography.X509Certificates;
using BaalPratibha.Models;
using Dapper;

namespace BaalPratibha.DbPortal
{
    public class VoteDb : BaseDb
    {
        public VoteDb(IConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }

        public long Insert(Vote vote)
        {
            const string sql = @"INSERT INTO core.votes(contestant_id, voter_name, voter_email)
                                    VALUES(@ContestantId, @VoterName, @VoterEmail) RETURNING id;";

            return Connection.Execute(sql, vote);
        }

        public bool HasVotedBefore(string email)
        {
            const string sql = @"SELECT * core.votes WHERE voter_email = @Email;";

            return Connection.QueryFirstOrDefault<Vote>(sql, new { email }) != null;
        }

        public Vote GetVoteByEmail(string email)
        {
            const string sql = @"SELECT id, contestant_id as ContestantId, voter_name as VoterName, voter_email as VoterEmail FROM core.votes WHERE voter_email = @Email;";

            return Connection.QueryFirstOrDefault<Vote>(sql, new { email });
        }

        public bool Update(Vote vote)
        {
            const string sql =
                @"UPDATE core.votes SET contestant_id = @ContestantId, voter_name = @VoterName WHERE voter_email = @VoterEmail;";

            return Connection.Execute(sql, vote) > 0;
        }
    }
}
