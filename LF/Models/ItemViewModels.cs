using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LF.Helpers;

namespace LF.Models
{
    public class CreateItemVM : LocationModel
    {
        public Guid? ItemId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително.")]
        [MaxLength(50,ErrorMessage = "Максимална дължина 50.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Категорията е задължителна.")]
        public Guid CategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "Посочете дали предмета е загубен.")]
        public bool IsLost { get; set; }

        public string Size { get; set; }

        [DataType(DataType.Currency)]
        public string RewardValue { get; set; }

        [Required(ErrorMessage = "Електронната поща е задължителна.")]
        [MaxLength(50, ErrorMessage = "Максимална дължина 50.")]
        public string OwnerEmail { get; set; }

        [Display(Name = "Размер")]
        public IEnumerable<SelectListItem> Sizes { get; set; }

        [Display(Name = "Категория")]
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string ImageLocation { get; set; }

        [FileSize(507200)]
        [FileTypes("jpg,jpeg,png")]
        public HttpPostedFileBase file { get; set; }
    }

    public class ShowItemsVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string RewardValue { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Size { get; set; }
        public string Category { get; set; }
        public string ImageLocation { get; set; }
        public string CreatedOn { get; set; }
        public string IsLost { get; set; }
        public Guid ItemId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class FilterModel
    {
        public string CategoryId { get; set; }
        public string CityId { get; set; }
        public string FromValue { get; set; }
        public string LostFound { get; set; }
        public string RegionId { get; set; }
        public string SizeType { get; set; }
        public string ToValue { get; set; }
        public string InputValue { get; set; }
    }
}