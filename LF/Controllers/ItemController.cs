using LF.Models.DropDownListModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LF.Models;
using Microsoft.AspNet.Identity;
using LF.DataAccess.Repositories;
using LF.DataAccess;
using System.Threading.Tasks;
using LF.Helpers;
using LF.Models.MenuModel;
using System.Text;
using System.IO;
using System.Globalization;
using System.Reflection;

namespace LF.Controllers
{
    public class ItemController : AsyncController
    {
        private LFDataManager _dataManager;

        public ItemController()
        {
            _dataManager = new LFDataManager();
        }

        #region Ajax

        public Task<JsonResult> GetSideMenu()
        {
            MenuModel model = new MenuModel();
            model.Categories = GetCategories(null);
            model.Regions = GetRegions(null);
            model.Cities = new List<SelectListItem>();//GetCities();
            model.Sizes = GetSizes(0);
            return Task.FromResult(Json(RenderHelper.PartialView(this, "ItemSideMenu", model), JsonRequestBehavior.AllowGet));
        }

        public Task<JsonResult> getCities(Guid regionId)
        {
            var cities = GetCities(regionId, null);
            return Task.FromResult(Json(cities, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> GetItemGrid()
        {
            var userId = User.Identity.GetUserId();
            List<ShowItemsVM> models = new List<ShowItemsVM>();
            List<Item> items =  await _dataManager.ItemsGetForCurrentUser(userId);
            foreach (var item in items)
            {
                ShowItemsVM model = new ShowItemsVM();
                model.Category = item.Category.CategoryName;
                model.City = item.City.CityName;
                model.CreatedOn = item.CreatedDate.ToString("MMMM dd, yyyy");
                model.Description = item.Description;
                model.ImageLocation = item.ImagesLocation;
                model.IsLost = item.IsLost.ToString();
                model.ItemId = item.Id;
                model.Region = _dataManager.RegionGetById(item.City.RegionId).Result.RegionName;
                model.RewardValue = item.RewardValue.ToString();
                switch (item.Size)
                {
                    case 1:
                        { model.Size = "Малък"; }
                        break;
                    case 2:
                        { { model.Size = "Среден"; } }
                        break;
                    case 3:
                        { { model.Size = "Голям"; } }
                        break;
                    default:
                        { model.Size = "-"; }
                        break;
                }
                model.Title = item.ItemName;
                model.UserId = item.UserId;
                model.UserName = item.User.FirstName + " " + item.User.LastName;
                models.Add(model);
            }

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> HotItems()
        {
            List<ShowItemsVM> models = new List<ShowItemsVM>();
            List<Item> items = await _dataManager.HotItemsGet();

            if (items.Count < 2)
            {
                items = _dataManager.ItemsGetAll().Result.OrderBy(x => x.CreatedDate).Take(4).ToList();
            }
            else if (items.Count > 6)
            {
                items = items.OrderBy(x=> x.CreatedDate).Take(4).ToList();
            }

            foreach (var item in items)
            {
                ShowItemsVM model = new ShowItemsVM();
                model.Category = item.Category.CategoryName;
                model.City = item.City.CityName;
                model.CreatedOn = item.CreatedDate.ToString("MMMM dd, yyyy");
                model.Description = item.Description;
                model.ImageLocation = item.ImagesLocation;
                model.IsLost = item.IsLost.ToString();
                model.ItemId = item.Id;
                model.Region = _dataManager.RegionGetById(item.City.RegionId).Result.RegionName;
                model.RewardValue = item.RewardValue.ToString();
                model.Title = item.ItemName;
                model.UserId = item.UserId;
                model.UserName = item.User.FirstName + " " + item.User.LastName;
                model.Size = "";

                models.Add(model);
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Filter(FilterModel filterOptions)
        {
            List<ShowItemsVM> models = new List<ShowItemsVM>();
            List<Item> items = await _dataManager.ItemsGetAll();

            //filtering
            if (!IsAnyNullOrEmpty(filterOptions))
            {
                string categoryId = filterOptions.CategoryId ?? null;
                string cityId = filterOptions.CityId ?? null;
                string regionId = filterOptions.RegionId ?? null;
                string fromValue = filterOptions.FromValue ?? null;
                string toValue = filterOptions.FromValue ?? null;
                string inputValue = filterOptions.InputValue ?? null;
                string itemSize = filterOptions.SizeType ?? null;
                items = items.Where(x => (inputValue != null ? x.ItemName.Contains(inputValue.Trim()) : true) &&
                                         (categoryId != null ? x.CategoryId == new Guid(categoryId) : true) &&
                                         (cityId != null ? x.CityId == new Guid(cityId) : true) &&
                                         (regionId != null ? x.City.RegionId == new Guid(regionId) : true) &&
                                         (fromValue != null ? x.RewardValue >= float.Parse(filterOptions.FromValue, CultureInfo.InvariantCulture.NumberFormat) : true) &&
                                         (toValue != null ? x.RewardValue <= float.Parse(filterOptions.ToValue, CultureInfo.InvariantCulture.NumberFormat) : true) &&
                                         x.IsLost == !Convert.ToBoolean(filterOptions.LostFound) &&
                                         (itemSize != null ? x.Size == (filterOptions.SizeType == Sizes.Малък.ToString() ? (int)Sizes.Малък :
                                                    filterOptions.SizeType == Sizes.Среден.ToString() ? (int)Sizes.Среден :
                                                    filterOptions.SizeType == Sizes.Голям.ToString() ? (int)Sizes.Среден : 0) : true)
                                        )
                                        .ToList();

                //items = items.Where(x => (inputValue != null ? x.ItemName.Contains(inputValue.Trim()) : true)).ToList();

                //items = items.Where(x => (categoryId != null ? x.CategoryId == new Guid(categoryId) : true)).ToList();

                //items = items.Where(x => (cityId != null ? x.CityId == new Guid(cityId) : true)).ToList();

                //items = items.Where(x => (regionId != null ? x.City.RegionId == new Guid(regionId) : true)).ToList();

                //items = items.Where(x => (fromValue != null ? x.RewardValue >= float.Parse(filterOptions.FromValue, CultureInfo.InvariantCulture.NumberFormat) : true)).ToList();

                //items = items.Where(x => (toValue != null ? x.RewardValue <= float.Parse(filterOptions.ToValue, CultureInfo.InvariantCulture.NumberFormat) : true)).ToList();

                //bool lost = !Convert.ToBoolean(filterOptions.LostFound);

                //items = items.Where(x => (x.IsLost == lost)).ToList();

                //items = items.Where(x => (itemSize != null ? x.Size == (filterOptions.SizeType == Sizes.Малък.ToString() ? (int)Sizes.Малък :
                //                                    filterOptions.SizeType == Sizes.Среден.ToString() ? (int)Sizes.Среден :
                //                                    filterOptions.SizeType == Sizes.Голям.ToString() ? (int)Sizes.Среден : 0) : true)).ToList();


            }

            foreach (var item in items.OrderBy(x=> x.CreatedDate))
            {
                ShowItemsVM model = new ShowItemsVM();
                model.Category = item.Category.CategoryName;
                model.City = item.City.CityName;
                model.CreatedOn = item.CreatedDate.ToString("MMMM dd, yyyy");
                model.Description = item.Description;
                model.ImageLocation = item.ImagesLocation;
                model.IsLost = item.IsLost.ToString();
                model.ItemId = item.Id;
                model.Region = _dataManager.RegionGetById(item.City.RegionId).Result.RegionName;
                model.RewardValue = item.RewardValue.ToString();
                model.Title = item.ItemName;
                model.UserId = item.UserId;
                model.UserName = item.User.FirstName + " " + item.User.LastName;
                model.Size = "";

                models.Add(model);
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public async Task<ActionResult> ShowItem(Guid itemId)
        {
            var item = await _dataManager.ItemGetById(itemId);
            ShowItemsVM model = PopulateShowItemVM(item);
            return View(model);
        }

        public ActionResult MyItems()
        {
            CreateItemVM model = new CreateItemVM();
            return View(model);
        }

        #region CRUD
        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        // GET: Item/Create
        public ActionResult CreateItem()
        {
            CreateItemVM model = new CreateItemVM();
            model.UserId = Guid.Parse(User.Identity.GetUserId());
            model.OwnerEmail = User.Identity.GetUserName();
            model.Countries = GetCountries(null);
            model.Regions = GetRegions(null);
            model.Cities = new List<SelectListItem>();//GetCities();
            model.Categories = GetCategories(null);
            model.Sizes = GetSizes(0);

            return View(model);
        }

        // POST: Item/Create
        [HttpPost]
        public async Task<ActionResult> CreateItem(CreateItemVM model)
        {
            try
            {
                TryUpdateModel(model);

                if (!ModelState.IsValid)
                {
                    model = PopulateDropDownLists(model);
                    return View(model);
                }

                string directory = null;
                string userDirectory = null;
                string fileLocation = null;
                StringBuilder trailingPath = null;
                string newDirectory = null;
                Item item = new Item();

                if (!model.ItemId.HasValue)
                {
                    item.UserId = User.Identity.GetUserId();
                    item.CityId = model.CityId;
                    item.ImagesLocation = model.ImageLocation ?? "";
                    item.IsDeleted = false;
                    item.IsLost = model.IsLost;
                    item.ItemName = model.Title;
                    item.Description = model.Description;
                    item.RewardValue = (float)Convert.ToDouble(model.RewardValue);
                    item.CategoryId = model.CategoryId;
                    item.CreatedDate = DateTime.UtcNow;
                    Sizes sizeType = (Sizes)Enum.Parse(typeof(Sizes), model.Size);
                    switch (sizeType)
                    {
                        case Sizes.Голям:
                            {
                                item.Size = (int)Sizes.Голям;
                            };
                            break;
                        case Sizes.Малък:
                            {
                                item.Size = (int)Sizes.Малък;
                            };
                            break;
                        case Sizes.Среден:
                            {
                                item.Size = (int)Sizes.Среден;
                            };
                            break;
                        default:
                            item.Size = 0;
                            break;
                    }
                    if (model.file != null)
                    {
                        directory = Server.MapPath(@"~/images/");
                        userDirectory = User.Identity.Name;
                        trailingPath = new StringBuilder(Path.GetExtension(model.file.FileName));
                        trailingPath.Insert(0, Guid.NewGuid());
                        fileLocation = Path.Combine(directory, userDirectory, trailingPath.ToString());
                        if (!Directory.Exists(directory + userDirectory))
                        {
                            Directory.CreateDirectory(directory + userDirectory);
                        }
                        model.file.SaveAs(fileLocation);
                        newDirectory = @"/images/" + userDirectory + "/" + trailingPath;
                        item.ImagesLocation = newDirectory;
                        await _dataManager.ItemAdd(item);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //log ex
                //throw err msg
            }
            return RedirectToAction("Index");
        }

        // GET: Item/Edit
        public ActionResult Edit(Guid id)
        {
            CreateItemVM model = new CreateItemVM();
            if (id != null)
            {
                model = PopulateEditItemVM(id);
            }
            return View(model);
        }

        // POST: Item/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(CreateItemVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (model.ItemId.HasValue)
                    {
                        model = PopulateEditItemVM(model.ItemId.Value);
                    }
                    return View(Task.FromResult(model));
                }
                string directory = null;
                string userDirectory = null;
                string fileLocation = null;
                StringBuilder trailingPath = null;
                string newDirectory = null;
                string oldDirectory = null;

                if (model.ItemId.HasValue)
                {
                    Item item = _dataManager.ItemGetById(model.ItemId.Value).Result;
                    item.CityId = model.CityId;
                    item.ImagesLocation = model.ImageLocation ?? "";
                    item.IsLost = model.IsLost;
                    item.ItemName = model.Title;
                    item.Description = model.Description;
                    item.RewardValue = (float)Convert.ToDouble(model.RewardValue);
                    item.CategoryId = model.CategoryId;
                    item.ModifiedDate = DateTime.UtcNow;
                    Sizes sizeType = (Sizes)Enum.Parse(typeof(Sizes), model.Size);
                    switch (sizeType)
                    {
                        case Sizes.Голям:
                            {
                                item.Size = (int)Sizes.Голям;
                            };
                            break;
                        case Sizes.Малък:
                            {
                                item.Size = (int)Sizes.Малък;
                            };
                            break;
                        case Sizes.Среден:
                            {
                                item.Size = (int)Sizes.Среден;
                            };
                            break;
                        default:
                            item.Size = 0;
                            break;
                    }
                    if (model.file != null)
                    {
                        if (item.ImagesLocation != null)
                        {
                            int index = model.ImageLocation.LastIndexOf(@"/");
                            string str = model.ImageLocation.Substring(index + 1);
                            oldDirectory = Path.Combine(directory + userDirectory + @"\" + str);
                        }
                        directory = Server.MapPath(@"~/images/");
                        userDirectory = User.Identity.Name;
                        trailingPath = new StringBuilder(Path.GetExtension(model.file.FileName));
                        trailingPath.Insert(0, User.Identity.GetUserId());
                        fileLocation = Path.Combine(directory, userDirectory, trailingPath.ToString());
                        if (!Directory.Exists(directory + userDirectory))
                        {
                            Directory.CreateDirectory(directory + userDirectory);
                        }
                        model.file.SaveAs(fileLocation);

                        newDirectory = @"/images/" + userDirectory + "/" + trailingPath;

                        if (item.ImagesLocation == null)
                        {
                            item.ImagesLocation = newDirectory;
                        }

                        if (item.ImagesLocation != newDirectory)
                        {
                            System.IO.File.Delete(oldDirectory);
                            item.ImagesLocation = newDirectory;
                        }
                    }
                    await _dataManager.ItemEdit(item);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var item = _dataManager.ItemGetById(id).Result;
                item.IsDeleted = true;
                await _dataManager.ItemDelete(item);
                return RedirectToAction("MyItems");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region supportive

        private List<SelectListItem> GetCountries(string selectedCountry)
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            var countris = _dataManager.CountriesGetAll().Result;
            return countries = DDL.ToDropDownList(countris, selectedCountry);
        }

        private List<SelectListItem> GetRegions(string selectedRegion)
        {
            List<SelectListItem> regions = new List<SelectListItem>();
            var regionss = _dataManager.RegionsGetAll().Result;
            return regions = DDL.ToDropDownList(regionss, selectedRegion);
        }

        private List<SelectListItem> GetCities(Guid regionId, string selectedCity)
        {
            List<SelectListItem> cities = new List<SelectListItem>();
            var citis = _dataManager.CitiesByRegionGetAll(regionId).Result;
            cities = DDL.ToDropDownList(citis, selectedCity);
            return cities;
        }

        private List<SelectListItem> GetCategories(string selectedCategory)
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            var categoris = _dataManager.CategoriesGetAll().Result;
            return categories = DDL.ToDropDownList(categoris, selectedCategory);
        }

        private List<SelectListItem> GetSizes(int sizeType)
        {
            List<SelectListItem> sizes = new List<SelectListItem>();
            SelectListItem size = new SelectListItem()
            {
                Value = Sizes.Малък.ToString(),
                Text = Sizes.Малък.ToString()
            };
            SelectListItem size2 = new SelectListItem()
            {
                Value = Sizes.Среден.ToString(),
                Text = Sizes.Среден.ToString()
            };
            SelectListItem size3 = new SelectListItem()
            {
                Value = Sizes.Голям.ToString(),
                Text = Sizes.Голям.ToString()
            };
            SelectListItem size0 = new SelectListItem()
            {
                Value = Sizes.Неопределен.ToString(),
                Text = Sizes.Неопределен.ToString()
            };

            sizes.Add(size);
            sizes.Add(size2);
            sizes.Add(size3);
            sizes.Add(size0);

            Sizes razmer = (Sizes)sizeType;

            if (sizeType >= 0 && sizeType < 4)
            {
                foreach (var type in sizes)
                {
                    if (type.Value == razmer.ToString())
                    {
                        type.Selected = true;
                    }
                }
            }
            return sizes;
        }

        private CreateItemVM PopulateDropDownLists(CreateItemVM model)
        {
            model.Countries = GetCountries(null);
            model.Regions = GetRegions(null);
            model.Cities = new List<SelectListItem>();//GetCities();
            model.Categories = GetCategories(null);
            model.Sizes = GetSizes(0);
            return model;
        }

        private ShowItemsVM PopulateShowItemVM(Item item)
        {
            ShowItemsVM model = new ShowItemsVM();
            model.Category = item.Category.CategoryName;
            model.City = item.City.CityName;
            model.CreatedOn = item.CreatedDate.ToString("MMMM dd, yyyy");
            model.Description = item.Description;
            model.ImageLocation = item.ImagesLocation;
            model.IsLost = item.IsLost.ToString();
            model.ItemId = item.Id;
            model.Region = _dataManager.RegionGetById(item.City.RegionId).Result.RegionName;
            model.RewardValue = item.RewardValue.ToString();
            switch (item.Size)
            {
                case 1:
                    { model.Size = "Малък"; }
                    break;
                case 2:
                    { { model.Size = "Среден"; } }
                    break;
                case 3:
                    { { model.Size = "Голям"; } }
                    break;
                default:
                    { model.Size = "-"; }
                    break;
            }
            model.Title = item.ItemName;
            model.UserId = item.UserId;
            model.UserName = item.User.FirstName + " " + item.User.LastName;
            return model;
        }

        private CreateItemVM PopulateEditItemVM(Guid itemId)
        {
            var item = _dataManager.ItemGetById(itemId).Result;
            CreateItemVM model = new CreateItemVM();
            model.ItemId = item.Id;
            model.UserId = Guid.Parse(User.Identity.GetUserId());
            model.OwnerEmail = User.Identity.GetUserName();
            var countries = GetCountries(_dataManager.CountriesGetAll().Result.FirstOrDefault().CountryName);
            model.Countries = countries;
            model.CountryId = new Guid(countries.Where(x => x.Selected == true).FirstOrDefault().Value);
            var regions = GetRegions(_dataManager.RegionGetById(item.City.RegionId).Result.RegionName);
            model.Regions = regions;
            model.RegionId = new Guid(regions.Where(x => x.Selected == true).FirstOrDefault().Value);
            var cities = GetCities(item.City.RegionId, item.City.CityName);
            model.Cities = cities;
            model.CityId = new Guid(cities.Where(x => x.Selected == true).FirstOrDefault().Value);
            var categories = GetCategories(_dataManager.CategoryById(item.CategoryId).Result.CategoryName);
            model.Categories = categories;
            model.CategoryId = new Guid(categories.Where(x => x.Selected == true).FirstOrDefault().Value);
            var sizes = GetSizes((int)item.Size);
            model.Sizes = sizes;
            model.Size = sizes.Where(x => x.Selected).FirstOrDefault().Value;
            model.Description = item.Description;
            model.Title = item.ItemName;
            model.RewardValue = item.RewardValue.ToString();
            model.ImageLocation = item.ImagesLocation;
            model.CreatedOn = item.CreatedDate;
            model.IsLost = item.IsLost;
            return model;
        }

        private string SetSize(byte size)
        {
            string sizeName = ""; ;

            //Sizes sizeType = (Sizes)Enum.Parse(typeof(Sizes), size);
            switch (size)
            {
                case (int)Sizes.Голям:
                    {
                        sizeName = Sizes.Голям.ToString();
                    };
                    break;
                case (int)Sizes.Малък:
                    {
                        sizeName = Sizes.Малък.ToString();
                    };
                    break;
                case (int)Sizes.Среден:
                    {
                        sizeName = Sizes.Среден.ToString();
                    };
                    break;
                default:
                    return sizeName;
            }
            return sizeName;
        }

        private bool IsAnyNullOrEmpty(object Object)
        {
            foreach (PropertyInfo property in Object.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(Object);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public enum Sizes
        {
            Неопределен = 0,
            Малък = 1,
            Среден = 2,
            Голям = 3

        }

        #endregion
    }
}
