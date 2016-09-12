﻿using LF.Models.DropDownListModels;
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

namespace LF.Controllers
{
    public class ItemController : AsyncController
    {
        private LFDataManager _dataManager;

        public ItemController()
        {
            _dataManager = new LFDataManager();
        }

        public JsonResult GetSideMenu()
        {
            MenuModel model = new MenuModel();
            model.Categories = GetCategories();
            model.Regions = GetRegions();
            model.Cities = GetCities();
            model.Sizes = GetSizes();
            return Json(RenderHelper.PartialView(this, "ItemSideMenu", model),JsonRequestBehavior.AllowGet);
        }

        #region CRUD
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Article/Create
        public ActionResult CreateItem()
        {
            CreateItemVM model = new CreateItemVM();
            model.UserId = Guid.Parse(User.Identity.GetUserId());
            model.OwnerEmail = User.Identity.GetUserName();
            model.Countries = GetCountries();
            model.Regions = GetRegions();
            model.Cities = GetCities();
            model.Categories = GetCategories();
            model.Sizes = GetSizes();

            return View(model);
        }

        // POST: Article/Create
        [HttpPost]
        public async Task<ActionResult> CreateItem(CreateItemVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model = PopulateDropDownLists(model);
                    return View(model);
                }

                Item item = new Item();
                item.UserId = model.UserId.ToString();
                item.CityId = model.CityId;
                item.ImagesLocation = model.ImageLocation ?? "";
                item.IsDeleted = false;
                item.IsLost = model.IsLost;
                item.ItemName = model.Title;
                item.Description = model.Description;
                item.RewardValue = (float)Convert.ToDouble(model.RewardValue);
                item.CategoryId = model.CategoryId;
                if (model.ItemId != null)
                {
                    item.ModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    item.CreatedDate = DateTime.UtcNow;
                }

                await _dataManager.ItemAdd(item);

            }
            catch (Exception ex)
            {
                //log ex
                //throw err msg
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> MyItems()
        {
            CreateItemVM model = new CreateItemVM();
            return View(model);

        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region supportive
        private List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();

            SelectListItem country = new SelectListItem()
            {
                Value = new Guid("99bb22a8-c40c-43a6-8861-f92568a3268c").ToString(),
                Text = "България",
                Selected = true
            };
            SelectListItem country2 = new SelectListItem()
            {
                Value = new Guid("ee792f95-941c-4509-8e24-bd552b5a0f99").ToString(),
                Text = "Сърбия"
            };

            countries.Add(country);
            countries.Add(country2);
            return countries;
        }

        private List<SelectListItem> GetRegions()
        {
            List<SelectListItem> regions = new List<SelectListItem>();

            SelectListItem region = new SelectListItem()
            {
                Value = new Guid("43261ad1-fd8b-4fd5-93d7-d06f54fb859f").ToString(),
                Text = "София"
            };
            SelectListItem region2 = new SelectListItem()
            {
                Value = new Guid("d978ecb7-6d93-4f61-b879-d21d0e680656").ToString(),
                Text = "Пловдив"
            };
            SelectListItem region3 = new SelectListItem()
            {
                Value = new Guid("43261ad1-fd8b-4fd5-93d7-d06f54fb859f").ToString(),
                Text = "Варна"
            };
            SelectListItem region4 = new SelectListItem()
            {
                Value = new Guid("d978ecb7-6d93-4f61-b879-d21d0e680656").ToString(),
                Text = "Бургас"
            };
            regions.Add(region);
            regions.Add(region2);
            regions.Add(region3);
            regions.Add(region4);
            return regions;
        }

        private List<SelectListItem> GetCities()
        {
            List<SelectListItem> cities = new List<SelectListItem>();

            SelectListItem city = new SelectListItem()
            {
                Value = new Guid("5b2bb2a6-35a8-4ad6-b77f-0ea34e466cab").ToString(),
                Text = "Пловдив"
            };
            SelectListItem city2 = new SelectListItem()
            {
                Value = new Guid("36269bb9-9e79-48c0-a45d-9d451be4f916").ToString(),
                Text = "София"
            };
            SelectListItem city3 = new SelectListItem()
            {
                Value = new Guid("900526f3-2f4c-4848-a773-90bbee3ae753").ToString(),
                Text = "Варна"
            };
            SelectListItem city4 = new SelectListItem()
            {
                Value = new Guid("900526f3-2f4c-4848-a773-90bbee3ae753").ToString(),
                Text = "Бургас"
            };
            SelectListItem city5 = new SelectListItem()
            {
                Value = new Guid("900526f3-2f4c-4848-a773-90bbee3ae753").ToString(),
                Text = "Стара Загора"
            };

            cities.Add(city);
            cities.Add(city2);
            cities.Add(city3);
            cities.Add(city4);
            cities.Add(city5);
            return cities;
        }

        private List<SelectListItem> GetCategories()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            SelectListItem category = new SelectListItem()
            {
                Value = new Guid("a2d62bbe-8d32-4d4e-80c5-3ac3658664c2").ToString(),
                Text = "Животни"
            };
            SelectListItem category2 = new SelectListItem()
            {
                Value = new Guid("7b118ea5-6a92-4b5d-bf93-91cda8848fc1").ToString(),
                Text = "Бижута"
            };
            SelectListItem category3 = new SelectListItem()
            {
                Value = new Guid("41c3e50f-6c9e-4e04-9f2f-13bb1029ea64").ToString(),
                Text = "Документи"
            };
            SelectListItem category4 = new SelectListItem()
            {
                Value = new Guid("41c3e50f-6c9e-4e04-9f2f-13bb1029ea64").ToString(),
                Text = "Багаж"
            };
            SelectListItem category5 = new SelectListItem()
            {
                Value = new Guid("41c3e50f-6c9e-4e04-9f2f-13bb1029ea64").ToString(),
                Text = "Ел. устрйство"
            };

            categories.Add(category);
            categories.Add(category2);
            categories.Add(category3);
            categories.Add(category4);
            categories.Add(category5);
            return categories;
        }

        private List<SelectListItem> GetSizes()
        {
            List<SelectListItem> sizes = new List<SelectListItem>();

            SelectListItem size = new SelectListItem()
            {
                Value = new Guid("a2d62bbe-8d32-4d4e-80c5-3ac3658664c2").ToString(),
                Text = "Малко"
            };
            SelectListItem size2 = new SelectListItem()
            {
                Value = new Guid("7b118ea5-6a92-4b5d-bf93-91cda8848fc1").ToString(),
                Text = "Средно"
            };
            SelectListItem size3 = new SelectListItem()
            {
                Value = new Guid("41c3e50f-6c9e-4e04-9f2f-13bb1029ea64").ToString(),
                Text = "Голямо"
            };

            sizes.Add(size);
            sizes.Add(size2);
            sizes.Add(size3);
            return sizes;
        }

        private CreateItemVM PopulateDropDownLists(CreateItemVM model)
        {
            model.Countries = GetCountries();
            model.Regions = GetRegions();
            model.Cities = GetCities();
            model.Categories = GetCategories();
            model.Sizes = GetSizes();
            return model;
        }

        public enum Sizes
        {
            Small = 1,
            Medium = 2,
            Large = 3

        }
        #endregion
    }
}