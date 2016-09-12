using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LF.Models.MenuModel
{
    public class MenuModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Size")]
        public IEnumerable<SelectListItem> Sizes { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Categories")]
        public IEnumerable<SelectListItem> Categories { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Cities")]
        public IEnumerable<SelectListItem> Cities { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Regions")]
        public IEnumerable<SelectListItem> Regions { get; set; }

        [Display(Name = "FromRewardValue")]
        public string FromRewardValue { get; set; }

        [Display(Name = "ToRewardValue")]
        public string ToRewardValue { get; set; }
    }
    
}