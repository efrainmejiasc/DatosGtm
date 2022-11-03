using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.IServices
{
    public interface IRequestService
    {
        Task<string> ObtenerJWTAsync(string urlAdobePdfApi);
        Task<string> ObtenerJWTPostAsync(string urlAdobePdfApiJwt);
    }
}
