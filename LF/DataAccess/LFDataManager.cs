using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LF.DataAccess.Repositories;
using System.Threading.Tasks;
using LF.Models;

namespace LF.DataAccess
{
    public class LFDataManager
    {
        private ItemRepository _itemRepository;
        public LFDataManager()
        {
            _itemRepository = new ItemRepository();
        }

        #region Item

        public async Task ItemAdd(Item item)
        {
            await _itemRepository.Insert(item);
        }

        #endregion
    }
}