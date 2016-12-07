using M.Document.Common;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.PdfTk.Provider
{
    public class DocProviderPdfTk : FormProvider
    {
        public override string FillForm(string templateFilePath, DocFieldCollection values)
        {
            string outputFile = GetTempFilePath();
            string outputFileData = outputFile.ToLower().Replace( ".pdf" , ".xfdf");
            
            StringBuilder sb = new StringBuilder();

            /*
            outputFileData = outputFile.ToLower().Replace( ".pdf" , ".xfdf");
            sb.AppendLine("<xfdf xmlns=\"http://ns.adobe.com/xfdf/\" xml:space= \"preserve\">");
            sb.AppendLine("<fields>");
            foreach (var field in values)
            {
                if (String.IsNullOrEmpty(field.Value) == false)
                {
                    sb.AppendFormat("<field name=\"{0}\">", field.Name);
                    sb.AppendFormat("<value>{0}</value>", field.Value);
                        sb.AppendLine("</field>");

                }


            }
            sb.AppendLine("</fields>");
            */

            outputFileData = outputFile.ToLower().Replace(".pdf", ".dat");
            foreach (var field in values)
            {
                if (String.IsNullOrEmpty(field.Value) == false)
                {
                    sb.AppendLine(string.Format("<</ T({0}) / V({1})>>", field.Name, field.Value));
                }
            }

            File.WriteAllText(outputFileData, sb.ToString());

            ProcessStartInfo startInfo = GetStartInfo();
            startInfo.Arguments = string.Format("\"{0}\" fill_form \"{1}\" output \"{2}\"", templateFilePath, outputFileData, outputFile);

            Process p = new Process() { StartInfo = startInfo };

            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();

            p.WaitForExit();
            if (error != null)
                throw new Exception(error);

            return outputFile;
        }


        private ProcessStartInfo GetStartInfo()
        {


            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.LoadUserProfile = false;
           // startInfo.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            startInfo.FileName = "pdftk.exe";
            startInfo.WorkingDirectory = Path.Combine(BaseDirectory, "tools");
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            return startInfo;
        }



        public override IEnumerable<DocField> GetFields(string templateFilePath)
        {


            DocFieldCollection fields = new DocFieldCollection();

            ProcessStartInfo startInfo = GetStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = string.Format("\"{0}\" dump_data_fields", templateFilePath);


            Process p = new Process() { StartInfo = startInfo };

            p.Start();

            string output = p.StandardOutput.ReadToEnd();

            p.WaitForExit();

            using (StringReader sr = new StringReader(output))
            {
                string line;
                DocField docField = null;
                while ((line = sr.ReadLine()) != null)
                {

                    if (line == "---")
                    {
                         if (docField != null)
                        {
                            fields.Add(docField);
                            
                        }
                        
                    }
                    else if (line.StartsWith("FieldName: "))
                    {
                        docField = new DocField();
                        docField.Name = line.Replace("FieldName: ", "");
                    }
                }
            }



            return fields;
        }
    }
}
