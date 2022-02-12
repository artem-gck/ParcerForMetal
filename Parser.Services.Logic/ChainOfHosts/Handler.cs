using Parser.Serviсes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Services.Logic.ChainOfHosts
{
    public abstract class Handler
    {
        public Handler Successor { get; set; }
        public abstract Task<Certificate> HandleRequestAsync(Uri link);
    }
}
