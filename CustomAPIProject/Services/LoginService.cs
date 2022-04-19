using CustomAPIProject.ApplicationContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Services
{
    public class LoginService : _ILoginService
    {

        private readonly DBContext _dBContext;

        public LoginService(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public bool TokenValidateInDB(string token)
        {
            return _dBContext.Login.Any(x => x.Token == token);
        }

        
    }
}
