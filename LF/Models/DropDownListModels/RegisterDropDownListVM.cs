using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.Models.DropDownListModels
{
    public class RegisterDropDownListVM
    {
        private readonly List<DropDownListItem> _items;
        private readonly string _label;

        public List<DropDownListItem> Items
        {
            get
            {
                return _items;
            }
        }

        public string label
        {
            get
            {
                return _label != null ? _label : "";
            }
        }

        public RegisterDropDownListVM()
        {
            this._items = new List<DropDownListItem>();
        }

        public RegisterDropDownListVM(string label)
        {
            this._items = new List<DropDownListItem>();
            this._label = label;
        }

        public void AddItem(string text, Guid? value)
        {
            this._items.Add(new DropDownListItem { Text = text, Value = value });
        }
    }

    public class DropDownListItem
    {
        public string Text { get; set; }
        public Guid? Value { get; set; }
    }
}