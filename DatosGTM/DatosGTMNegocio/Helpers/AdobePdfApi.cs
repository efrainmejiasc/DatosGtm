using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.Helpers
{
    public  class AdobePdfApi
    {
        public static string client_id { get; set; }
        public static string client_secret { get; set; }
        public static string organization_id { get; set; }
        public static string account_id { get; set; }
        public static string private_key_file { get; set; }
        public static string pdf_filesToRead { get; set; }
        public static string pdf_filesToWrite{ get; set; }
        public static string pdf_filesExtract{ get; set; }
        public static string file_credentials { get; set; }
        public static string log_excepcion { get; set; }
        public static int split_number_pages { get; set; }

    }
}
