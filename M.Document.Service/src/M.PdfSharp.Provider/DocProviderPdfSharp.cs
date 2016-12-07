using M.Document.Common;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.PdfSharp.Provider
{
    public class DocProviderPdfSharp : FormProvider
    {
        public override string FillForm(string templateFilePath, DocFieldCollection values)
        {
            string outputFile = GetTempFilePath();
            File.Copy(templateFilePath, outputFile, true);
         
            // Open the file
            PdfDocument document = PdfReader.Open(outputFile, PdfDocumentOpenMode.Modify);

            PdfAcroForm form = document.AcroForm;
            foreach ( var docField in values)
            {
                try
                {
                    PdfTextField field = form.Fields[docField.Name] as PdfTextField;
                    if (field != null)
                        field.Text = docField.Value;
                }
                catch
                {

                }
            }
            document.Save(outputFile);

            ////Create a pdf document
            //PdfDocument doc = new PdfDocument();
            ////Load from file
            //doc.LoadFromFile(templateFilePath);

            ////Get form
            //PdfFormWidget formWidget = doc.Form as PdfFormWidget;


            //for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
            //{
            //    PdfField field = formWidget.FieldsWidget.List[i] as PdfField;

            //    DocField docField = values.Where(p => (p.Name == field.Name)).FirstOrDefault();
            //    if (docField != null)
            //    {
            //        PdfTextBoxFieldWidget widget = field as PdfTextBoxFieldWidget;
            //        if (widget != null)
            //            widget.Text = docField.Value;


            //    }
            //}

            // doc.SaveToFile(outputFile);
            return outputFile;
        }






        public override IEnumerable<DocField> GetFields(string templateFilePath)
        {
            DocFieldCollection fields = new DocFieldCollection();
            PdfDocument doc = PdfReader.Open(templateFilePath);
            

            PdfAcroForm form  = doc.AcroForm;
            foreach (var field in form.Elements)
            {
                
             
                string fieldName = field.Key;
                PdfItem pdfField = field.Value;

             
                fields.Add(new DocField() { Name = fieldName, Value = "" });
            }

            return fields;
        }
    }
}
