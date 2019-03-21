using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employee_API.Models
{
    public class PagingPaameterModel
    {
        const int maxPageSize = 20;

        public int pageNumber { get; set; } = 1;

        public int _pageSize { get; set; } = 4;

        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}