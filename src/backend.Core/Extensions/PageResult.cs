using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Core.Extensions
{
    public class PageResult<T>
    {
        public PageResult(IEnumerable<T> items, int count)
        {
            Items = items;
            Count = count;
        }

        public int Count { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
