using iText.Forms;
using iText.Kernel.Pdf;
using M.Document.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.IText7.Provider
{
    public class DocProviderIText : FormProvider
    {
        public override string FillForm(string templateFilePath, DocFieldCollection values)
        {
            string tempFile = GetTempFilePath();
            using (PdfReader reader = new PdfReader(templateFilePath))
            {
                reader.SetUnethicalReading(true);
                using (PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(tempFile)))
                {
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                    form.SetGenerateAppearance(true);
                    foreach (var field in values)
                    {
                        try
                        {
                            var pdfField = form.GetField(field.Name);
                            if( pdfField != null )
                                pdfField.SetValue(field.Value);
                        }
                        catch
                        {

                        }
                    }
                    pdfDoc.Close();
                }
            }
            return tempFile;
        }

        public override IEnumerable<DocField> GetFields(string templateFilePath)
        { 
            DocFieldCollection fields = new DocFieldCollection();

            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(templateFilePath)))
            {
                PdfAcroForm pdfForm = PdfAcroForm.GetAcroForm(pdfDoc, false);

                foreach (var de in pdfForm.GetFormFields())
                {
                    var formField = de.Value;
                    fields.Add(new DocField() { Name = de.Key, Value = formField.GetValueAsString() });
                }

            }

            return fields;
        }

    }
}
