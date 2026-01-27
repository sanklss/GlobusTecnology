using GlobusT.Data;
using GlobusT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobusT.Services
{
    public class AuthService
    {
        public Role TryAuth(string username, string password)
        {
            using (var context = new GlobusTechnologyContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Login == username & u.Password == password);

                if (user == null)
                {
                    return null;
                }

                return context.Roles.FirstOrDefault(r => r.Id == user.IdRole);
            }
        }
    }
}
