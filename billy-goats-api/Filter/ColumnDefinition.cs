using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bolstra.Api.Filter
{
    public class ColumnDefinition
    {
        public string DataKey { get; set; }
        public string DisplayType { get; set; }
        public string HeaderText { get; set; }
        public int? Precision { get; set; }
        public bool? Required { get; set; } = null;
        public bool Sortable { get; set; } = true;
    }
}
