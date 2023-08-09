using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class VistaCuposDistribuidos
    {
        public virtual Int64 Compcta { get; set; }
        public virtual string Cuitcomp { get; set; }
        public virtual string Comprador { get; set; }
        public virtual Int64 Vendcta { get; set; }
        public virtual string Cuitvend { get; set; }
        public virtual string Vendedor { get; set; }
        public virtual int Codproducto { get; set; }
        public virtual string Producto { get; set; }
        public virtual string Cosecha { get; set; }
        public virtual Int64 Ctadestino { get; set; }
        public virtual string Destino { get; set; }
        public virtual string Zonainfluencia { get; set; }
        public virtual string Codcentro { get; set; }
        public virtual string Centro { get; set; }
        public virtual string Tipcta { get; set; }
        public virtual int Pactado { get; set; }
        public virtual int Pendentrega { get; set; }
        public virtual int Fijado { get; set; }
        public virtual int Liquidado { get; set; }
        public virtual int Pendaplicar { get; set; }
        public virtual int Cuposadist { get; set; }
        public virtual int Cuposotorgados { get; set; }
        public virtual int Cupostotalesadist { get; set; }
        public virtual int Fechaent { get; set; }
        public virtual int Fechavto { get; set; }
        public virtual int Ayer { get; set; }
        public virtual int Hoy { get; set; }
        public virtual int Dia1 { get; set; }
        public virtual int Dia2 { get; set; }
        public virtual int Dia3 { get; set; }
        public virtual int Dia4 { get; set; }
        public virtual int Dia5 { get; set; }
        public virtual int Dia6 { get; set; }
        public virtual int Dia7 { get; set; }
        public virtual int Dia8 { get; set; }
        public virtual int Dia9 { get; set; }
        public virtual int Dia10 { get; set; }
        public virtual int Dia11 { get; set; }
        public virtual int Dia12 { get; set; }
        public virtual int Dia13 { get; set; }
        public virtual int Dia14 { get; set; }
        public virtual int Dia15 { get; set; }
        public virtual int Dia16 { get; set; }
        public virtual int Dia17 { get; set; }
        public virtual int Dia18 { get; set; }
        public virtual int Dia19 { get; set; }
        public virtual int Dia20 { get; set; }
        public virtual int Pedidosayer { get; set; }
        public virtual int Pedidoshoy { get; set; }
        public virtual int Pedidosdia1 { get; set; }
        public virtual int Pedidosdia2 { get; set; }
        public virtual int Pedidosdia3 { get; set; }
        public virtual int Pedidosdia4 { get; set; }
        public virtual int Pedidosdia5 { get; set; }
        public virtual int Pedidosdia6 { get; set; }
        public virtual int Pedidosdia7 { get; set; }
        public virtual int Pedidosdia8 { get; set; }
        public virtual int Pedidosdia9 { get; set; }
        public virtual int Pedidosdia10 { get; set; }
        public virtual int Pedidosdia11 { get; set; }
        public virtual int Pedidosdia12 { get; set; }
        public virtual int Pedidosdia13 { get; set; }
        public virtual int Pedidosdia14 { get; set; }
        public virtual int Pedidosdia15 { get; set; }
        public virtual int Pedidosdia16 { get; set; }
        public virtual int Pedidosdia17 { get; set; }
        public virtual int Pedidosdia18 { get; set; }
        public virtual int Pedidosdia19 { get; set; }
        public virtual int Pedidosdia20 { get; set; }
        public virtual string Cuitsolicitante { get; set; }
        public virtual string Cuitintermediario { get; set; }
        public virtual string Cuitrtecomercial { get; set; }
        public virtual string Cuitcorrcomp { get; set; }
        public virtual string Cuitmat { get; set; }
        public virtual string Cuitcorrvta { get; set; }
        public virtual string Cuitrteent { get; set; }
        public virtual string Cuitdestinatario { get; set; }
        public virtual string CuitRteComercialProductor { get; set; }
        public virtual string CuitRteComercialVentaPrimaria { get; set; }
        //public virtual string Caratula { get; set; }
        //public virtual string ContactoComercial { get; set; }

    public virtual int Vendcyo { get; set; }

        public virtual Consignacion GetConsignacion()
        {
            return new Consignacion
            {
                Cuitsolicitante = this.Cuitsolicitante,
                Cuitintermediario = this.Cuitintermediario,
                Cuitrtecomercial = this.Cuitrtecomercial,
                Cuitcorrcomp = this.Cuitcorrcomp,
                Cuitmat = this.Cuitmat,
                Cuitcorrvta = this.Cuitcorrvta,
                Cuitrteent = this.Cuitrteent,
                Cuitdestinatario = this.Cuitdestinatario,
                CuitRteComercialProductor = this.CuitRteComercialProductor,
                CuitRteComercialVentaPrimaria = this.CuitRteComercialVentaPrimaria
            };
        }

        public virtual void SetConsignacion(Consignacion consignacion)
        {
            this.Cuitsolicitante = consignacion.Cuitsolicitante;
            this.Cuitintermediario = consignacion.Cuitintermediario;
            this.Cuitrtecomercial = consignacion.Cuitrtecomercial;
            this.Cuitcorrcomp = consignacion.Cuitcorrcomp;
            this.Cuitmat = consignacion.Cuitmat;
            this.Cuitcorrvta = consignacion.Cuitcorrvta;
            this.Cuitrteent = consignacion.Cuitrteent;
            this.Cuitdestinatario = consignacion.Cuitdestinatario;
            this.CuitRteComercialProductor = consignacion.CuitRteComercialProductor;
            this.CuitRteComercialVentaPrimaria = consignacion.CuitRteComercialVentaPrimaria;
        }

        public override bool Equals(object obj)
        {
            VistaCuposDistribuidos recievedObject = (VistaCuposDistribuidos)obj;

            if (ReferenceEquals(recievedObject, null)) return false;
            if (ReferenceEquals(recievedObject, this)) return true;

            if ((this.Compcta == recievedObject.Compcta)
                && (this.Codproducto == recievedObject.Codproducto)
                //&& (this.Cosecha == recievedObject.Cosecha)
                && (this.Vendcta == recievedObject.Vendcta)
                //&& (this.Ctadestino == recievedObject.Ctadestino)
                && (this.Codcentro == recievedObject.Codcentro)
                )
            {
                return (true);
            }
            return (false);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual int GetAcumuladoDias()
        {
            int acumulado = this.Hoy + this.Dia1 + this.Dia2 + this.Dia3 + this.Dia4 + this.Dia5 + this.Dia6 + this.Dia7 + this.Dia8 + this.Dia9;
            acumulado += +this.Dia10 + this.Dia11 + this.Dia12 + this.Dia13 + this.Dia14 + this.Dia15 + this.Dia16 + this.Dia17 + this.Dia18 + this.Dia19 + this.Dia20;
            return acumulado;
        }

        public virtual bool SuperaCuposADist()
        {
            return this.Cuposadist >= GetAcumuladoDias();
        }
    }
}