using System.Collections.Generic;

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
