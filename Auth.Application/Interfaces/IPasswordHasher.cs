using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Interfaces
{
    public interface IPasswordHasher
    {
        public string Hash(string password); // Para crear hash al registrar usuario
        public bool Verify(string password, string passwordHash); // Para validar contraseña al hacer login
    }
}
