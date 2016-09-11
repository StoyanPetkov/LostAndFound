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
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(50,ErrorMessage = "Maximum length 50.")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Guid CategoryId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "You have to specify is item lost or found.")]
        public bool IsLost { get; set; }

        public byte Size { get; set; }

        [DataType(DataType.Currency)]
        public float RewardValue { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "E-mail is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length 50.")]
        public string OwnerEmail { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Size")]
        public IEnumerable<SelectListItem> Sizes { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Category")]
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string ImageLocation { get; set; }
    }
}