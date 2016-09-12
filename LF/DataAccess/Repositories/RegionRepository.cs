using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class RegionRepository : BaseRepository<Region>
    {
        public RegionRepository() : base()
        {
        }

        public RegionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}