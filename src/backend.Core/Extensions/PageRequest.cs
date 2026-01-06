using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Core.Extensions
{
    public class PageRequest : IPageable
    {
        [FromHeader]
        [Range(1, int.MaxValue, MinimumIsExclusive = false)]
        public required int Page { get; set; }

        [FromHeader]
        [Range(1, int.MaxValue, MinimumIsExclusive = false)]
        public required int PageSize { get; set; }
    }
}
