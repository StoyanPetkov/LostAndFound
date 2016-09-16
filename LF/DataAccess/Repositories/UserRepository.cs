using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>
    {
        public UserRepository() : base()
        {
        }

        public UserRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}