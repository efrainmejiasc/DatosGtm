using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMModelo.Repositories
{
    public class TerceroRepository: ITerceroRepository
    {
        private readonly MyAppContext db;
        public TerceroRepository(MyAppContext _db)
        {
            this.db = _db;
        }

        public List<Tercero> GuardarRangoTercero (List<Tercero> model)
        {
            db.AddRange(model);
            db.SaveChanges();

            return model;
        }

        public List<Tercero> GetTerceros (Guid identificador)
        {
            return db.Tercero.Where(x => x.Identificador == identificador ).ToList ();
        }

        public List<Tercero> GetTerceros()
        {
            return db.Tercero.ToList();
        }
    }
}
