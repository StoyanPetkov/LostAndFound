using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LF.Models.DropDownListModels
{
    public class MenuDropDownListModels
    {
        private readonly List<MenuDropDownListItem> _items;
        private readonly string _label;

        public List<MenuDropDownListItem> Items
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

        public MenuDropDownListModels()
        {
            this._items = new List<MenuDropDownListItem>();
        }

        public MenuDropDownListModels(string label)
        {
            this._items = new List<MenuDropDownListItem>();
            this._label = label;
        }

        public void AddItem(string text, string value)
        {
            this._items.Add(new MenuDropDownListItem { Text = text, Value = value });
        }

    }
    public class MenuDropDownListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}