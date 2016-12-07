using M.Document.Common;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.PdfTk.Pdfium
{
    public class DocProviderPdfium : FormProvider
    {


        public override string FillForm(string templateFilePath, DocFieldCollection values)
        {
            string outputFile = GetTempFilePath();
            string outputFileData = outputFile.ToLower().Replace( ".pdf" , ".xfdf");
            PdfCommon.Initialize();
            var forms = new PdfForms();
            PdfDocument document = PdfDocument.Load(templateFilePath, forms);
            DocFieldCollection fields = new DocFieldCollection();
            
            foreach (var field in forms.InterForm.Fields)
            {
                string name = field.FullName;
                DocField docField = values.Where(p => (p.Name == name)).FirstOrDefault();
                if (docField != null)
                    field.Value = docField.Value;
              
            }
            document.Save(outputFile, Patagames.Pdf.Enums.SaveFlags.NoIncremental);



            return outputFile;
        }



        public override IEnumerable<DocField> GetFields(string templateFilePath)
        {
            PdfCommon.Initialize();
            var forms = new PdfForms();
            PdfDocument document = PdfDocument.Load(templateFilePath, forms);
            DocFieldCollection fields = new DocFieldCollection();
            foreach (var field in forms.InterForm.Fields)
            {
             
                    fields.Add(new DocField() { Name = field.FullName, Value = field.Value, FieldType =  field.Type.ToString() });
                
            }



            return fields;
        }
    }
}
