using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Cliente
{
    public class ConjuntoCuposAgrupadosPorVendedor
    {
        public IList<CuposAgrupadosPorVendedor> CuposInformadosSTOPCyo { get; set; }
        public IList<CuposAgrupadosPorVendedor> CuposNoInformadosSTOPCyo { get; set; }
        public IList<CuposAgrupadosPorVendedor> CuposInformadosSTOPNoCyo { get; set; }
        public IList<CuposAgrupadosPorVendedor> CuposNoInformadosSTOPNoCyo { get; set; }
    }
}