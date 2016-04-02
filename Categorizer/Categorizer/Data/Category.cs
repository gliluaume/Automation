using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Categorizer.Data
{
    public class Category
    {
        public string Name { get; set; }
        public HashSet<string> Words { get; set; }
        public Category()
        {
            this.Words = new HashSet<string>();
        }
    }
}
