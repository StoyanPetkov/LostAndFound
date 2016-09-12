using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class CommentsRepository : BaseRepository<Comment>
    {
        public CommentsRepository() : base()
        {
        }

        public CommentsRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}