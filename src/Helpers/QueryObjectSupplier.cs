using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlDeVenta_Proy.src.Helpers
{
    public class QueryObjectSupplier
    {
        public string? textFilter { get; set; } = string.Empty;
        public string? orderBy { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}