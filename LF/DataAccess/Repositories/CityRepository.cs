using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class CityRepository : BaseRepository<City>
    {
        public CityRepository() : base()
        {
        }

        public CityRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}