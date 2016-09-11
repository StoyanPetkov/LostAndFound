using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class ItemRepository : BaseRepository<Item>
    {
        public ItemRepository() : base()
        {
        }

        public ItemRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}