using LF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LF.Helpers
{
    public static class DDL
    {
        public static List<SelectListItem> ToDropDownList(List<City> cities, string selectedCity)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var city in cities)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = city.CityName,
                    Value = city.CityId.ToString()
                };

                if (city.CityName == selectedCity)
                {
                    item.Selected = true;
                }
                selectListItems.Add(item);
            }
            return selectListItems;
        }

        public static List<SelectListItem> ToDropDownList(List<Country> countries,string selectedCountry)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var country in countries)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryId.ToString()
                };

                if (country.CountryName == selectedCountry)
                {
                    item.Selected = true;
                }
                selectListItems.Add(item);
            }
            return selectListItems;
        }

        public static List<SelectListItem> ToDropDownList(List<Region> regions, string selectedRegion)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var region in regions)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = region.RegionName,
                    Value = region.RegionId.ToString()
                };
                if (region.RegionName == selectedRegion)
                {
                    item.Selected = true;
                }
                selectListItems.Add(item);
            }
            return selectListItems;
        }

        public static List<SelectListItem> ToDropDownList(List<Category> categories, string selectedCategory)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var category in categories)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = category.CategoryName,
                    Value = category.Id.ToString()
                };
                if (category.CategoryName == selectedCategory)
                {
                    item.Selected = true;
                }
                selectListItems.Add(item);
            }
            return selectListItems;
        }
    }
}