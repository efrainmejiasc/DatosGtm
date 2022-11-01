using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMModelo.Repositories
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly MyAppContext db;
        public UsuarioRepository(MyAppContext _db)
        {
            this.db = _db;
        }
        public Usuario GetUserData( string userMail, string password)
        {
            return db.Usuario.Where(x => (x.Password == password || x.Password2 == password) && (x.Email == userMail || x.UserName == userMail) && x.Estado).FirstOrDefault();
        }

    }
}
