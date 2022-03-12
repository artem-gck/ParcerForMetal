using Parser.Serviсes.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DataAccess
{
    public interface IAuthAccessManager
    {
        public Task<User> AuthUserAsync(string login, string password);
        public Task<User> GetUserAsync(string login);
        public Task<bool> SetNewRefreshKeyAsync(User user); 
    }
}
