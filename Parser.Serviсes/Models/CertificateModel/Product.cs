﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parser.Serviсes.Models.CertificateModel
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Labeling { get; set; }
        public string? Code { get; set; }
    }
}
