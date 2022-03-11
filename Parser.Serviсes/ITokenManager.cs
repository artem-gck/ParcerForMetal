using Parser.Serviсes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes
{
    public interface ITokenManager
    {
        public Task<TokenApiModel> Refresh(TokenApiModel tokenApiModel);
    }
}
