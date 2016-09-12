using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LF.DataAccess.Repositories;
using System.Threading.Tasks;
using LF.Models;

namespace LF.DataAccess
{
    public class LFDataManager
    {
        private ItemRepository _itemRepository;
        private CountryRepository _countryRepository;
        private RegionRepository _regionRepository;
        private CityRepository _cityRepository;
        private CommentsRepository _commentRepository;
        private CategoryRepository _categoryRepository;

        public LFDataManager()
        {
            _itemRepository = new ItemRepository();
            _regionRepository = new RegionRepository();
            _countryRepository = new CountryRepository();
            _cityRepository = new CityRepository();
            _commentRepository = new CommentsRepository();
            _categoryRepository = new CategoryRepository();
        }

        #region Item

        public async Task ItemAdd(Item item)
        {
            await _itemRepository.Insert(item);
        }

        public Task<List<Item>> ItemsGetForCurrentUser(string userId)
        {
            return _itemRepository.GetAll(filter: x=> x.UserId == userId);
        }

        #endregion

        #region Region

        public Task<Region> RegionGetById(Guid regionId)
        {
            return _regionRepository.GetById(regionId);
        }


        #region Comment

        #endregion

        #endregion

        #region Region

        #endregion

        #region City

        #endregion

        #region Country

        #endregion

        #region Category

        #endregion
    }
}