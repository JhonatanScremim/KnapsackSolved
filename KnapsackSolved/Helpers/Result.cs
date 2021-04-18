using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnapsackSolved.Helpers
{
    public class Result
    {
        public int Generation { get; set; }
        public int TotalCost { get; set; }
        public int TotalValue { get; set; }
        public IList<Item> Items{ get; set; }
    }
}
