using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class CountryRepository : BaseRepository<Country>
    {
        public CountryRepository() : base()
        {
        }

        public CountryRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}