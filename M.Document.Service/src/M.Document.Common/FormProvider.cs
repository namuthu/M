using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.Document.Common
{
    public abstract class FormProvider:IFormProvider
    {
        public string BaseDirectory { get; set; }

        public string TemplateDirectory { get; set; }
        public string OutputDirectory { get; set; }


        public string GetTemplateFilePath(string fileName)
        {
            return Path.Combine(TemplateDirectory, fileName);
        }


        public string GetTempFilePath()
        {
            string guidFile = Guid.NewGuid().ToString("N");
            return Path.Combine(OutputDirectory, $"{guidFile}.pdf");

        }


    public abstract IEnumerable<DocField> GetFields(string fileName);


    public abstract string FillForm(string id, DocFieldCollection values);
       
    }
}
