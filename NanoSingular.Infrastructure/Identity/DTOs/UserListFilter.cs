using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoSingular.Infrastructure.Identity.DTOs
{
    public class UserListFilter : PaginationFilter
    {
        public string Keyword { get; set; }

    }
}
