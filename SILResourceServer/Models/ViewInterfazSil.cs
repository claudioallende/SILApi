using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ViewInterfazSil
    {
        public virtual long cuposID { get; set; }
        public virtual DateTime cuposfecha { get; set; }
        public virtual long cuposgrano { get; set; }
        public virtual long cuposcompcta { get; set; }
        public virtual long cupospuertocta { get; set; }
        public virtual string cuposcuitsolicitante { get; set; }
        public virtual string cuposcuitintermediario { get; set; }
        public virtual string cuposnrocupo { get; set; }
        public virtual long cuposvendcta { get; set; }
        public virtual long cuposvendcyo { get; set; }
        public virtual string cuposnomsolicitante { get; set; }
        public virtual string cuposnomintermediario { get; set; }
        public virtual string cuposcuitrtecomercial { get; set; }
        public virtual string cuposnomrtecomercial { get; set; }
        public virtual string cuposcuitcorrcomp { get; set; }
        public virtual string cuposnomcorrcomp { get; set; }
        public virtual string cuposcuitmat { get; set; }
        public virtual string cuposnommat { get; set; }
        public virtual string cuposcuitcorrvta { get; set; }
        public virtual string cuposnomcorrvta { get; set; }
        public virtual string cuposcuitrteent { get; set; }
        public virtual string cuposnomrteent { get; set; }
        public virtual string cuposcuitdestinatario { get; set; }
        public virtual string cuposnomdestinatario { get; set; }
        public virtual string cuposcuitrtecomercialprod { get; set; }
        public virtual string cuposnomrtecomercialprod { get; set; }
        public virtual string cuposcuitrtecomercialvtaprim { get; set; }
        public virtual string cuposnomrtecomercialvtaprim { get; set; }
        public virtual string cuposcentro { get; set; }
        public virtual string cuposcentrodist { get; set; }
        public virtual string cuposcontrato { get; set; }
        public virtual string cuposcorrcta { get; set; }
        public virtual string cuposcuposotorgados { get; set; }
        public virtual string cuposcupospedidos { get; set; }
        public virtual string cuposcuposrecibidos { get; set; }
        public virtual int cuposstatus { get; set; }
        public virtual int cupostipo { get; set; }
        public virtual long cuposidorigen { get; set; }
        public virtual long cuposuvcupodist { get; set; }
        public virtual string cuposmotbaja { get; set; }
        public virtual string cuposobserva { get; set; }
        public virtual int cupospdf { get; set; }
        public virtual string cuposusuario { get; set; }
        public virtual string cuposdetallecupocnrt { get; set; }
        public virtual string cuposestadocupocnrt { get; set; }
        public virtual DateTime cuposfechayhorainformado { get; set; }
        public virtual int granograno { get; set; }
        public virtual string granonombre { get; set; }
        public virtual string compradorcuit { get; set; }
        public virtual string compradordomicilio { get; set; }
        public virtual string compradorlocalidad { get; set; }
        public virtual string compradornombre { get; set; }
        public virtual string compradorprovincia { get; set; }
        public virtual int compradorstipprovee { get; set; }
        public virtual string compradortipodecuenta { get; set; }
        public virtual string vendedorcuit { get; set; }
        public virtual string vendedordomicilio { get; set; }
        public virtual string vendedorlocalidad { get; set; }
        public virtual string vendedornombre { get; set; }
        public virtual string vendedorprovincia { get; set; }
        public virtual int vendedorstipprovee { get; set; }
        public virtual string vendedortipodecuenta { get; set; }
        public virtual int vendedorinterexter { get; set; }
        public virtual string puertocuit { get; set; }
        public virtual string puertodomicilio { get; set; }
        public virtual string puertolocalidad { get; set; }
        public virtual string puertonombre { get; set; }
        public virtual string puertoprovincia { get; set; }
        public virtual int puertostipprovee { get; set; }
        public virtual string puertotipodecuenta { get; set; }
        public virtual long puertocpostal { get; set; }
        public virtual string puertoruca { get; set; }
        public virtual string puertoidterminal { get; set; }
        public virtual string codigoestabproc { get; set; }

        public virtual Consignacion GetConsignacion()
        {
            return new Consignacion
            {
                Cuitsolicitante = this.cuposcuitsolicitante,
                Cuitintermediario = this.cuposcuitintermediario,
                Nomsolicitante = this.cuposnomsolicitante,
                Nomintermediario = this.cuposnomintermediario,
                Cuitrtecomercial = this.cuposcuitrtecomercial,
                Nomrtecomercial = this.cuposnomrtecomercial,
                Cuitcorrcomp = this.cuposcuitcorrcomp,
                Nomcorrcomp = this.cuposnomcorrcomp,
                Cuitmat = this.cuposcuitmat,
                Nommat = this.cuposnommat,
                Cuitcorrvta = this.cuposcuitcorrvta,
                Nomcorrvta = this.cuposnomcorrvta,
                Cuitrteent = this.cuposcuitrteent,
                Nomrteent = this.cuposnomrteent,
                Cuitdestinatario = this.cuposcuitdestinatario,
                Nomdestinatario = this.cuposnomdestinatario,
                CuitRteComercialProductor = this.cuposcuitrtecomercialprod,
                NomRteComercialProductor = this.cuposnomrtecomercialprod,
                CuitRteComercialVentaPrimaria = this.cuposcuitrtecomercialvtaprim,
                NomRteComercialVentaPrimaria = this.cuposnomrtecomercialvtaprim
            };
        }

        public virtual Grano GetGrano(){
            return new Grano
            {
                Id = this.granograno.ToString(),
                CodigoGrano = this.granograno,
                Nombre = this.granonombre
            };
        }

        public virtual Comprador GetComprador()
        {
            return new Comprador
            {
                Id = this.cuposcompcta,
                Cuenta = this.cuposcompcta,
                Cuit = this.compradorcuit,
                Domicilio = this.compradordomicilio,
                Localidad = this.compradorlocalidad,
                Nombre = this.compradornombre,
                Provincia = this.compradorprovincia,
                Stipprovee = this.compradorstipprovee,
                Tipodecuenta = this.compradortipodecuenta
            };
        }

        public virtual Vendedor GetVendedor()
        {
            return new Vendedor
            {
                Id = this.cuposvendcta,
                Cuenta = this.cuposvendcta,
                Cuit = this.vendedorcuit,
                Domicilio = this.vendedordomicilio,
                Localidad = this.vendedorlocalidad,
                Nombre = this.vendedornombre,
                Provincia = this.vendedorprovincia,
                Stipprovee = this.vendedorstipprovee,
                Tipodecuenta = this.vendedortipodecuenta,
                Interexter = this.vendedorinterexter
            };
        }

        public virtual Puerto GetPuerto()
        {
            return new Puerto
            {
                Id = this.cupospuertocta,
                Cuenta = this.cupospuertocta,
                Cuit = this.puertocuit,
                Domicilio = this.puertodomicilio,
                Localidad = this.puertolocalidad,
                Nombre = this.puertonombre,
                Provincia = this.puertoprovincia,
                Stipprovee = this.puertostipprovee,
                Tipodecuenta = this.puertotipodecuenta,
                Cpostal = this.puertocpostal,
                Ruca = this.puertoruca,
                IdTerminal = this.puertoidterminal
            };
        }

        public virtual ViewInterfazSil Group()
        {
            return new ViewInterfazSil
            {
                cuposcuitsolicitante = this.cuposcuitsolicitante,
                cuposcuitintermediario = this.cuposcuitintermediario,
                cuposnomsolicitante = this.cuposnomsolicitante,
                cuposnomintermediario = this.cuposnomintermediario,
                cuposcuitrtecomercial = this.cuposcuitrtecomercial,
                cuposnomrtecomercial = this.cuposnomrtecomercial,
                cuposcuitcorrcomp = this.cuposcuitcorrcomp,
                cuposnomcorrcomp = this.cuposnomcorrcomp,
                cuposcuitmat = this.cuposcuitmat,
                cuposnommat = this.cuposnommat,
                cuposcuitcorrvta = this.cuposcuitcorrvta,
                cuposnomcorrvta = this.cuposnomcorrvta,
                cuposcuitrteent = this.cuposcuitrteent,
                cuposnomrteent = this.cuposnomrteent,
                cuposcuitdestinatario = this.cuposcuitdestinatario,
                cuposnomdestinatario = this.cuposnomdestinatario,
                granograno = this.granograno,
                granonombre = this.granonombre,
                cuposcompcta = this.cuposcompcta,
                compradorcuit = this.compradorcuit,
                compradordomicilio = this.compradordomicilio,
                compradorlocalidad = this.compradorlocalidad,
                compradornombre = this.compradornombre,
                compradorprovincia = this.compradorprovincia,
                compradorstipprovee = this.compradorstipprovee,
                compradortipodecuenta = this.compradortipodecuenta,
                cuposvendcta = this.cuposvendcta,
                vendedorcuit = this.vendedorcuit,
                vendedordomicilio = this.vendedordomicilio,
                vendedorlocalidad = this.vendedorlocalidad,
                vendedornombre = this.vendedornombre,
                vendedorprovincia = this.vendedorprovincia,
                vendedorstipprovee = this.vendedorstipprovee,
                vendedortipodecuenta = this.vendedortipodecuenta,
                vendedorinterexter = this.vendedorinterexter,
                cupospuertocta = this.cupospuertocta,
                puertocuit = this.puertocuit,
                puertodomicilio = this.puertodomicilio,
                puertolocalidad = this.puertolocalidad,
                puertonombre = this.puertonombre,
                puertoprovincia = this.puertoprovincia,
                puertostipprovee = this.puertostipprovee,
                puertotipodecuenta = this.puertotipodecuenta,
                puertocpostal = this.puertocpostal,
                puertoruca = this.puertoruca,
                puertoidterminal = this.puertoidterminal,
                codigoestabproc = this.codigoestabproc
            };
        }
    }
}