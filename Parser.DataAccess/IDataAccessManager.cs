using Parcer.Model;
using Parser.Serviсes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DataAccess
{
    public interface IDataAccessManager
    {
        public Task<int> AddCertificateAsync(Certificate certificate);
        public Task<Certificate> GetCertificateAsync(int id);
    }
}
