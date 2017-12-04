using System.Linq;
using System.Threading.Tasks;
using Aplication.Queries.Infrastructure;
using Aplication.Queries.ViewModels;
using MediatR;
using Dapper;

namespace Aplication.Queries
{
    public class GetSessionsFilmsQueryHandler : IAsyncRequestHandler<GetSessionsFilmsQuery, QueryResponse<SessionFilmsViewModel[]>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetSessionsFilmsQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<QueryResponse<SessionFilmsViewModel[]>> Handle(GetSessionsFilmsQuery message)
        {
            using (var conn = _connectionProvider.CreateConnection())
            {
                string sqlWhere = string.Empty;
                if (message.IsPublished != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.IsPublished = @IsPublished");
                if (message.ReferenceDate != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.[Start] >= @ReferenceDate");

                string sql = string.Format(@"
SELECT *
FROM cine.Films
WHERE cine.Films.Id IN
	(
		SELECT Sessions.FilmId
		  FROM session.Sessions
          {0} 
	)", sqlWhere);

                var films = await conn.QueryAsync<SessionFilmsViewModel>(sql, new
                {
                    IsPublished = message.IsPublished,
                    ReferenceDate = message.ReferenceDate
                });
                return new QueryResponse<SessionFilmsViewModel[]>
                {
                    Data = films.ToArray()
                };
            }
        }
    }

}
