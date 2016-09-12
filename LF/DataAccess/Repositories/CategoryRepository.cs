using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository() : base()
        {
        }

        public CategoryRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}