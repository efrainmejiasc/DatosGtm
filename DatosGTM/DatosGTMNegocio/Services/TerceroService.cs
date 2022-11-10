using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using DatosGTMNegocio.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.Services
{
    public  class TerceroService:ITerceroService
    {
        private readonly  ITerceroRepository _terceroRepository;
        public TerceroService(ITerceroRepository terceroRepository)
        {
            this._terceroRepository = terceroRepository;
        }

        public List<Tercero> GetTerceros()
        {
            return this._terceroRepository.GetTerceros();
        }
    }
}
 