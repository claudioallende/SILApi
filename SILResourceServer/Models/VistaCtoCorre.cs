using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class VistaCtoCorre
    {
        public virtual string Id { get; set; }
        public virtual int Empresa { get; set; }
        public virtual string Contrato { get; set; }
        public virtual Int64 Compcta { get; set; }
        public virtual string Cuitcomp { get; set; }
        public virtual string Comprador { get; set; }
        public virtual Int64 Vendcta { get; set; }
        public virtual string Cuitvend { get; set; }
        public virtual string Vendedor { get; set; }
        public virtual int Grano { get; set; }
        public virtual string Producto { get; set; }
        public virtual string Cosecha { get; set; }
        public virtual Int64 Ctadestino { get; set; }
        public virtual string Destino { get; set; }
        public virtual string Zonainfluencia { get; set; }
        public virtual string Codcentro { get; set; }
        public virtual string Centro { get; set; }
        public virtual string Tipcta { get; set; }
        public virtual string Opera { get; set; }
        public virtual DateTime Entrega { get; set; }
        public virtual DateTime Vtoent { get; set; }
        public virtual DateTime Prorroga { get; set; }
        public virtual double Pactadas { get; set; }
        public virtual int Pendientes { get; set; }
        public virtual double Ittfijadas { get; set; }
        public virtual double Ittparciales { get; set; }
        public virtual string Operacion { get; set; }
        public virtual double Precio { get; set; }
        public virtual string Fecha { get; set; }
        public virtual string Contparte { get; set; }
        public virtual Int64 Fechaent { get; set; }
        public virtual int Fechavto { get; set; }
        public virtual string Negocio { get; set; }
        public virtual long Aplicadas { get; set; }
        public virtual long Liquidadas { get; set; }
    }
}