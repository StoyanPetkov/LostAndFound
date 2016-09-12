using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LF.Models
{
    public class CreateItemVM : LocationModel
    {
        public Guid? ItemId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Заглавието е задължително.")]
        [MaxLength(50,ErrorMessage = "Максимална дължина 50.")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Категорията е задължителна.")]
        public Guid CategoryId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "Посочете дали предмета е загубен.")]
        public bool IsLost { get; set; }

        public byte Size { get; set; }

        [DataType(DataType.Currency)]
        public string RewardValue { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Електронната поща е задължителна.")]
        [MaxLength(50, ErrorMessage = "Максимална дължина 50.")]
        public string OwnerEmail { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Размер")]
        public IEnumerable<SelectListItem> Sizes { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Категория")]
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string ImageLocation { get; set; }
    }
}