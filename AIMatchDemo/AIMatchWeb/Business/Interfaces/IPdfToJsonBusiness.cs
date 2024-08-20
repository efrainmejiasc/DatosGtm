using Microsoft.AspNetCore.Http;

namespace AIMatchWeb.Business.Interfaces
{
    public interface IPdfToJsonBusiness
    {
        string ExtractTextFromPdf(IFormFile file);
        string ConvertPDFToJson(string filePathn,string fileName);

    }
}
