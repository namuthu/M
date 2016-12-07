using M.Document.Common;
using Spire.Pdf;
using Spire.Pdf.Fields;
using Spire.Pdf.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.Spire.Provider
{
    public class DocProviderSpire : FormProvider
    {
        public override string FillForm(string templateFilePath, DocFieldCollection values)
        {
            //Create a pdf document
            PdfDocument doc = new PdfDocument();
            //Load from file
            doc.LoadFromFile(templateFilePath);

            //Get form
            PdfFormWidget formWidget = doc.Form as PdfFormWidget;


            for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
            {
                PdfField field = formWidget.FieldsWidget.List[i] as PdfField;

                DocField docField = values.Where(p => (p.Name == field.Name)).FirstOrDefault();
                if (docField != null)
                {
                    PdfTextBoxFieldWidget widget = field as PdfTextBoxFieldWidget;
                    if (widget != null)
                        widget.Text = docField.Value;


                }
            }
            string outputFile = GetTempFilePath();
            doc.SaveToFile(outputFile);
            return outputFile;
        }






        public override IEnumerable<DocField> GetFields(string templateFilePath)
        {
            DocFieldCollection fields = new DocFieldCollection();
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(templateFilePath);

            PdfFormWidget formWidget = doc.Form as PdfFormWidget;
            for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
            {
                PdfField field = formWidget.FieldsWidget.List[i] as PdfField;
                string fieldName = field.Name;
                bool isRequired = field.Required;
                if (isRequired)
                {
                    Console.WriteLine(fieldName + " is required.");
                }
                fields.Add(new DocField() { Name = fieldName, Value = field.FullName });
            }

            return fields;
        }
    }
}
