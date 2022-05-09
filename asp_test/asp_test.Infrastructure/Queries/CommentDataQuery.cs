using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asp_test.Application.Comments;
using asp_test.Infrastructure.Database.Data;

namespace asp_test.Infrastructure.Queries
{
    internal class CommentDataQuery: ICommentDataQuery
    {
        private readonly asp_testContext _dbContext;

        public CommentDataQuery(asp_testContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
