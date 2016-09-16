using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LF.DataAccess.Repositories;
using System.Threading.Tasks;
using LF.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;

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
        private UserRepository _userRepository;

        public LFDataManager()
        {
            _itemRepository = new ItemRepository();
            _regionRepository = new RegionRepository();
            _countryRepository = new CountryRepository();
            _cityRepository = new CityRepository();
            _commentRepository = new CommentsRepository();
            _categoryRepository = new CategoryRepository();
            _userRepository = new UserRepository();
        }

        #region User

        public Task<ApplicationUser> UserGetById(string userId)
        {
            BaseRepository<ApplicationUser> baseRepo = new BaseRepository<ApplicationUser>();
            var user = baseRepo.Context.Users.Where(x => x.Id == userId.ToString()).First();
            return Task.FromResult(user);
        }

        #endregion

        #region Item

        public async Task<List<Item>> FilterItems(FilterModel filterOptions)
        {
            List<Item> items = await ItemsGetAll();
            //filtering
            string categoryId = filterOptions.CategoryId ?? null;
            string cityId = filterOptions.CityId ?? null;
            string regionId = filterOptions.RegionId ?? null;
            string fromValue = filterOptions.FromValue ?? null;
            string toValue = filterOptions.FromValue ?? null;
            string inputValue = filterOptions.InputValue ?? null;
            string itemSize = filterOptions.SizeType ?? null;
            string isLost = filterOptions.LostFound ?? null;
            return items = items.Where(x => (inputValue != null ? x.ItemName.Contains(inputValue.Trim()) : true) &&
                                     (categoryId != null ? x.CategoryId == new Guid(categoryId) : true) &&
                                     (cityId != null ? x.CityId == new Guid(cityId) : true) &&
                                     (regionId != null ? x.City.RegionId == new Guid(regionId) : true) &&
                                     (fromValue != null ? x.RewardValue >= float.Parse(filterOptions.FromValue, CultureInfo.InvariantCulture.NumberFormat) : true) &&
                                     (toValue != null ? x.RewardValue <= float.Parse(filterOptions.ToValue, CultureInfo.InvariantCulture.NumberFormat) : true) &&
                                     (isLost != null ? x.IsLost == !Convert.ToBoolean(isLost) : true) &&
                                     (itemSize != null ? x.Size == (filterOptions.SizeType == Sizes.Малък.ToString() ? (int)Sizes.Малък :
                                                filterOptions.SizeType == Sizes.Среден.ToString() ? (int)Sizes.Среден :
                                                filterOptions.SizeType == Sizes.Голям.ToString() ? (int)Sizes.Среден : 0) : true)
                                    )
                                    .ToList();
        }

        public async Task<List<Item>> HotItemsGet(Guid? cityId)
        {
            List<Item> itemList = new List<Item>();
            List<Item> defaultItemList = new List<Item>();
            float averageRW = _itemRepository.GetAll(filter: x=> !x.IsDeleted).Result.Average(x => x.RewardValue).Value;

            if (cityId.HasValue)
            {
                itemList = _itemRepository.GetAll(filter: x => x.CityId == cityId).Result;
            }
            if (itemList.Count < 12)
            {
                //defaultItemList = await _itemRepository.GetAll(filter: x => !x.IsDeleted &&
                //                                               x.RewardValue > averageRW &&
                //                                               x.CreatedDate > DateTime.Now.AddDays(-14) &&
                //                                               x.ImagesLocation != null & x.ImagesLocation != "");
                defaultItemList = await _itemRepository.GetAll(filter: x => !x.IsDeleted);
                defaultItemList = defaultItemList.Where(x => x.RewardValue > averageRW).ToList();
                defaultItemList = defaultItemList.Where(x => x.CreatedDate > DateTime.Now.AddDays(-14)).ToList();
                defaultItemList = defaultItemList.Where(x => x.ImagesLocation != null & x.ImagesLocation != "").ToList();
                return defaultItemList;
            }
            else
            {
                itemList = itemList.Where(x => x.ImagesLocation != null &&
                                               x.RewardValue > averageRW &&
                                               x.CreatedDate > DateTime.Now.AddDays(-14)).ToList();
                if (itemList.Count() >= 4)
                {
                    return itemList;
                }
                else
                {
                    return defaultItemList;
                }
            }
        }

        public async Task<List<Item>> ItemsGetAll()
        {
            return await _itemRepository.GetAll(filter: x=> !x.IsDeleted);
        }

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

        public enum Sizes
        {
            Неопределен = 0,
            Малък = 1,
            Среден = 2,
            Голям = 3

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

        public Task<City> CityGetById(Guid cityId)
        {
            return _cityRepository.GetById(cityId);
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