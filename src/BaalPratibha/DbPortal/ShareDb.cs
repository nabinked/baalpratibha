using BaalPratibha.Logic;
using BaalPratibha.Models;
using Dapper;
using System.Threading.Tasks;

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

        public async Task<int> UpdateAllShares()
        {
            var contestants = _contestantDb.GetAllContestants();
            foreach (var contestantView in contestants)
            {
                var totalFbShares = await _viewHelper.GetShares(contestantView.UserName);
                if (GetTotalDbShares(contestantView.Id) < totalFbShares)
                {
                    return Upsert(contestantView.Id, totalFbShares);
                }
            }
            return 0;
        }

        public async Task<int> UpdateShare(string userName)
        {
            var contestantView = _contestantDb.GetContestantViewByUserName(userName);

            var totalFbShares = await _viewHelper.GetShares(contestantView.UserName);
            if (GetTotalDbShares(contestantView.Id) < totalFbShares)
            {
                return Upsert(contestantView.Id, totalFbShares);
            }
            return 0;
        }

        public int Upsert(long contestantId, long shares)
        {

            const string sql = @"INSERT INTO core.shares(contestant_id, total_shares)
                                    VALUES(@ContestantId, @TotalShares)
                                    ON CONFLICT (contestant_id)
                                    do update set total_shares = @TotalShares
                                RETURNING total_shares;";

            return Connection.Execute(sql, new { ContestantId = contestantId, TotalShares = shares });
        }

        public long GetTotalDbShares(long contestantId)
        {
            var sql = "SELECT total_shares from core.shares WHERE contestant_id = @ContestantId";
            return Connection.QueryFirstOrDefault<long>(sql, new { contestantId });
        }

    }
}
