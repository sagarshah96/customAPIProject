using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Services
{
    public interface _ILoginService
    {
        bool TokenValidateInDB(string token);
    }
}
