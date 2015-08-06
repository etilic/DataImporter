using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Names
{
    public class NameColumn
    {
        public Int32 Index
        {
            get;
            private set;
        }

        public String Title
        {
            get;
            private set;
        }

        public NameColumn(Int32 index, String title)
        {
            this.Index = index;
            this.Title = title;
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
