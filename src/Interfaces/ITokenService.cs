using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}