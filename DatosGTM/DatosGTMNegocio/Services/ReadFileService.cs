using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using DatosGTMNegocio.DTOs;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.Services
{

    public class ReadFileService: IReadFileService 
    {
        private readonly ITerceroRepository _terceroRepository;
        public ReadFileService(ITerceroRepository terceroRepository)
        {
            this._terceroRepository = terceroRepository;
        }

        public List<Tercero>  LeerArchivo(string path, string identificador)
        {
            var infoPdf = new ExtractPdfInfoModel();
            var elements = new List<Element>();
            var infoOrdenada = new InformacionPdfOrdenada();
            var terceros = new List<Tercero>();
            var tercero = new Tercero();
            var texto = Helper.ReadFile(path);
            var fecha = DateTime.Now;

            if (!string.IsNullOrEmpty(texto))
            {
                infoPdf = JsonConvert.DeserializeObject<ExtractPdfInfoModel>(texto);
                elements = infoPdf.elements.Where(x => x.Text != null && (x.Text.Trim() != "No." && x.Text.Trim() != "NIT" && x.Text.Trim() != "NOMBRE" && x.Text.Trim() != "FECHA INICIO" && !x.Text.Contains("LISTADO DE AGENTES DE RETENCIÓN DEL IVA"))).ToList();
                infoOrdenada.Numero = elements.Where(x => x.Path.Substring(x.Path.Length - 4, 4).Trim() == "TD/P").Select(x => x.Text).ToList();
                infoOrdenada.Nit = elements.Where(x => x.Path.Substring(x.Path.Length - 7, 7).Trim() == "TD[2]/P").Select(x => x.Text).ToList();
                infoOrdenada.Nombre = elements.Where(x => x.Path.Substring(x.Path.Length - 7, 7).Trim() == "TD[3]/P").Select(x => x.Text.Replace("\"", "")).ToList();
                infoOrdenada.FechaInicio = elements.Where(x => x.Path.Substring(x.Path.Length - 7, 7).Trim() == "TD[4]/P").Select(x => x.Text).ToList();

                for (int i = 0; i < infoOrdenada.Numero .Count (); i++)
                {
                    try
                    {
                        tercero.Numero = Convert.ToInt32(infoOrdenada.Numero[i]);
                        tercero.Nit = infoOrdenada.Nit[i];
                        tercero.Nombre = infoOrdenada.Nombre[i];
                        tercero.FechaInicio = infoOrdenada.FechaInicio[i];
                        tercero.FechaRegistro = fecha;
                        tercero.Identificador = identificador;
                        terceros.Add(tercero);
                        tercero = new Tercero();
                    }
                    catch(Exception ex)
                    {
                        var err = ex.Message;
                    }
                  
                }

                terceros = this._terceroRepository.GuardarRangoTercero(terceros);
            }

            return terceros;
        }
    }
}
