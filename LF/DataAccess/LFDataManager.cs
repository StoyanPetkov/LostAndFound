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

        public async Task ItemEdit(Item item)
        {
            await _itemRepository.Update(item);
        }

        public async Task ItemAdd(Item item)
        {
            await _itemRepository.Insert(item);
        }

        public Task<List<Item>> ItemsGetForCurrentUser(string userId)
        {
            return _itemRepository.GetAll(filter: x => x.UserId == userId && !x.IsDeleted);
        }

        public Task<Item> ItemGetById(Guid itemId)
        {
            return _itemRepository.GetById(itemId);
        }

        public Task ItemDelete(Item item)
        {
            return _itemRepository.Update(item);
        }

        #endregion

        #region Region

        public Task<Region> RegionGetById(Guid regionId)
        {
            return _regionRepository.GetById(regionId);
        }

        public Task<List<Region>> RegionsGetAll()
        {
            return _regionRepository.GetAll(filter: x => !x.IsDeleted);
        }
        #endregion

        #region Comment

        #endregion

        #region City

        public Task<List<City>> CitiesByRegionGetAll(Guid regionId)
        {
            return _cityRepository.GetAll(filter: x => !x.IsDeleted && x.RegionId == regionId);
        }

        #endregion

        #region Country

        public Task<List<Country>> CountriesGetAll()
        {
            return _countryRepository.GetAll(filter: x=> !x.IsDeleted);
        }

        #endregion

        #region Category

        public Task<List<Category>> CategoriesGetAll()
        {
            return _categoryRepository.GetAll(filter: x => !x.IsDeleted);
        }


        public Task<Category> CategoryById(Guid id)
        {
            return _categoryRepository.GetById(id);
        }

        #endregion
    }
}