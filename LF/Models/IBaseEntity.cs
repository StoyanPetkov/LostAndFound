using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF.Models
{
    public interface IBaseEntity<T> where T : class, new()
    {
        string Id { get; set; }
    }
}
