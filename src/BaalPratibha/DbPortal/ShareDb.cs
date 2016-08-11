using BaalPratibha.Logic;
using BaalPratibha.Models;
using Dapper;

namespace BaalPratibha.DbPortal
{
    public class ShareDb : BaseDb
    {
        private readonly ContestantDb _contestantDb;
        private readonly IViewHelper _viewHelper;

        public ShareDb(IConnectionFactory connectionFactory, ContestantDb contestantDb, IViewHelper viewHelper) : base(connectionFactory)
        {
            _contestantDb = contestantDb;
            _viewHelper = viewHelper;
        }

        public void UpdateAllShares()
        {
            var contestants = _contestantDb.GetAllContestants();
            foreach (var contestantView in contestants)
            {
                var totalFbShares = _viewHelper.GetShares(contestantView.UserName).Result;
                if (GetTotalDbShares(contestantView.Id) < totalFbShares)
                {
                    Upsert(contestantView.Id, totalFbShares);
                }
            }
        }

        public void Upsert(long contestantId, long shares)
        {

            const string sql = @"INSERT INTO core.shares(contestant_id, total_shares)
                                    VALUES(@ContestantId, @TotalShares)
                                    ON CONFLICT (contestant_id)
                                    do update set total_shares = @TotalShares
                                RETURNING total_shares;";

            Connection.Execute(sql, new { ContestantId = contestantId, TotalShares = shares });
        }

        public long GetTotalDbShares(long contestantId)
        {
            var sql = "SELECT total_shares from core.shares WHERE contestant_id = @ContestantId";
            return Connection.QueryFirstOrDefault<long>(sql, new { contestantId });
        }

    }
}
