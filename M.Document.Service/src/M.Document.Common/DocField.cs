using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.Document.Common
{
    public class DocField
    {

        public string Name { get; set; }
        public string Value { get; set; }

        public string FieldType { get; set; }
    }

    public class DocFieldCollection : List<DocField>
    {

    }
}
