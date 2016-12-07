using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using M.Document.Common;
using M.IText7.Provider;
using System.IO;
using M.Spire.Provider;
using M.PdfTk.Provider;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using M.PdfSharp.Provider;
using M.PdfTk.Pdfium;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace M.Document.Service.Controllers
{
    [Route("api/[controller]")]
    public class TemplatesController : Controller
    {

        private IHostingEnvironment _env;
        private FormProvider _provider;
        private string _providerType;

        public TemplatesController(IHostingEnvironment env, IConfiguration configuration)
        {
            _providerType = configuration["AppSettings:PdfProvider"];
            _env = env;

        }

        private FormProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    if (String.Compare(_providerType, "itext", true) == 0)
                        _provider = new DocProviderIText() { OutputDirectory = Path.Combine(_env.WebRootPath, "output"), TemplateDirectory = Path.Combine(_env.WebRootPath, "templates") };
                    else if (String.Compare(_providerType, "spire", true) == 0)
                        _provider = new DocProviderSpire() { OutputDirectory = Path.Combine(_env.WebRootPath, "output"),
                            TemplateDirectory = Path.Combine(_env.WebRootPath, "templates"),
                            BaseDirectory = _env.WebRootPath
                        };
                    else if (String.Compare(_providerType, "pdftk", true) == 0)

                        _provider = new DocProviderPdfTk();
                      
                    else if (String.Compare(_providerType, "pdfsharp", true) == 0)
                        _provider = new DocProviderPdfSharp();
                      
                    else if (String.Compare(_providerType, "pdfium", true) == 0)
                        _provider = new DocProviderPdfium();


                    _provider.OutputDirectory = Path.Combine(_env.WebRootPath, "output");
                    _provider.TemplateDirectory = Path.Combine(_env.WebRootPath, "templates");
                    _provider.BaseDirectory = _env.WebRootPath;
                        
                }
                return _provider;
            }
        }


        private string GetTemplateFileName(int templateId)
        {
            var result = GetTemplates().Where(p =>(  p.TemplateId == templateId)).FirstOrDefault();
            return result.FileName;
                     
        }

        private List<DocTemplate> GetTemplates()
        {
            return new List<DocTemplate>() {
                new DocTemplate() { TemplateId = 1, FileName = "2001e.pdf" },
                new DocTemplate() { TemplateId = 2, FileName = "i-134.pdf" } };
        }
        

        [HttpGet()]
        public List<DocTemplate> GetList()
        {
            return GetTemplates();
        }

        // GET api/values/5
        [HttpGet("{templateId}")]
        public FileStreamResult GetContent(int templateId)
        {
            return new FileStreamResult(new FileStream(Provider.GetTemplateFilePath(GetTemplateFileName(templateId)), FileMode.Open), "application/pdf");
        }

        [HttpGet("{templateId}/Fields")]
        public IEnumerable<DocField> GetFields(int templateId)
        {
            return Provider.GetFields(Provider.GetTemplateFilePath(GetTemplateFileName(templateId)));
        }

        [HttpPost("{templateId}/FillForm")]
        public string FillForm(int templateId, [FromBody] DocFieldCollection value)
        {
            return Provider.FillForm(Provider.GetTemplateFilePath(GetTemplateFileName(templateId)), value);
        }
    }
}