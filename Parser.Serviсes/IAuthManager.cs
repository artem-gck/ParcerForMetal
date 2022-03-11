using Parser.Serviсes.Models;
using Parser.Serviсes.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes
{
    public interface IAuthManager
    {
        public Task<TokenApiModel> Login(User loginModel);
    }
}
