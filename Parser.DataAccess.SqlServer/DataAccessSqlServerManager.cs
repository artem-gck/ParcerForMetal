using Parser.Serviсes.Models;
using Parser.Serviсes.Models.Certificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DataAccess.SqlServer
{
    public class DataAccessSqlServerManager : IDataAccessManager
    {
        public async Task<int> AddCertificateAsync(Certificate certificate)
        {
            throw new NotImplementedException();
        }

        public async Task<Certificate> GetCertificateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
