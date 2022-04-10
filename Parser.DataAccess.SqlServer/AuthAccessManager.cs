using Microsoft.EntityFrameworkCore;
using Parser.Serviсes.Models.Context;
using Parser.Serviсes.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DataAccess.SqlServer
{
    public class AuthAccessManager : IAuthAccessManager
    {
        private readonly MetalContext _context;

        public AuthAccessManager(MetalContext metalContext)
            => _context = metalContext;

        public async Task<User> AuthUserAsync(string login, string password)
        {
            login = login is not null ? login : throw new ArgumentNullException(nameof(login));

            return await _context.User.FirstOrDefaultAsync(user => user.Login == login && user.Password == password);
        }

        public async Task<User> GetUserAsync(string login)
        {
            login = login is not null ? login : throw new ArgumentNullException(nameof(login));

            return await _context.User.Include(us => us.UserInfo)
                                      .FirstOrDefaultAsync(user => user.Login == login);
        }

        public async Task<bool> SetNewRefreshKeyAsync(User user)
        {
            user = user is not null ? user : throw new ArgumentNullException(nameof(user));

            var userData = await _context.User.FirstOrDefaultAsync(userData => userData.Login == user.Login);

            userData.RefreshToken = user.RefreshToken;
            userData.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
