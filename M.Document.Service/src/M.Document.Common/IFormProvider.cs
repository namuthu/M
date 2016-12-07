using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.Document.Common
{
    public interface IFormProvider
    {

        string GetTemplateFilePath(string fileName);

        IEnumerable<DocField> GetFields(string templateFilePath);


        string FillForm(string templateFilePath, DocFieldCollection values);


    }
}
