using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ResourceServer.Models.DTO;

namespace ResourceServer.Models
{
  public class NuevoCupoViewModel
  {
    public string Error { get; set; }
    public int ProductoSeleccionado { get; set; }
    public int PuertoSeleccionado { get; set; }
    public int CompctaSeleccionada { get; set; }
    public int VendctaSeleccionada { get; set; }
    public readonly int CantidadDias = 20;
    private ServicioCupo servicioCupo;
    //private CodigosAlfanumericos Codigos { get; set; }
    [Display(Name = "Producto")]
    public virtual IEnumerable<SelectListItem> Productos
    {
      get
      {
        IGranoStore store = new GranoStore();
        var granos = store.FindAll()
                    .Select(x =>
                            new SelectListItem
                            {
                              Value = x.Id.ToString(),
                              Text = x.Nombre
                            });

        return new SelectList(granos, "Value", "Text");
      }
      private set { }
    }
    [Display(Name = "Comprador")]
    [Required]
    [Range(0, long.MaxValue, ErrorMessage = "Por favor ingrese un número")]
    public string Compcta { get; set; }
    [Required]
    [Range(0, long.MaxValue, ErrorMessage = "Por favor ingrese un número")]
    public string Puerto { get; set; }
    [Display(Name = "Centro")]
    [Required]
    public virtual string Centro
    {
      get
      {
        ICentroStore store = new CentroStore();
        Centro CentroPorDefecto = store.FindById(ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroPorDefectoUsuarioLogueado());
        return CentroPorDefecto.Nombre;
      }
      private set { }
    }
    [Display(Name = "Vendedor")]
    [Range(0, long.MaxValue, ErrorMessage = "Por favor ingrese un número")]
    [NotEqual("Compcta", "Vendedor", "Comprador")]
    public string Vendcta { get; set; }
    public virtual bool VendcyoBoolValue { get; set; }
    [Display(Name = "Solicitante")]
    public virtual string Cuitsolicitante { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nomsolicitante { get; set; }
    [Display(Name = "Intermediario")]
    public virtual string Cuitintermediario { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nomintermediario { get; set; }
    [Display(Name = "Comercial")]
    public virtual string Cuitrtecomercial { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nomrtecomercial { get; set; }
    [Display(Name = "Corredor Comprador")]
    public virtual string Cuitcorrcomp { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nomcorrcomp { get; set; }
    [Display(Name = "Mercado a Termino")]
    public virtual string Cuitmat { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nommat { get; set; }
    [Display(Name = "Corredor Vendedor")]
    public virtual string Cuitcorrvta { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nomcorrvta { get; set; }
    [Display(Name = "Entregador")]
    public virtual string Cuitrteent { get; set; }
    [Display(Name = "Nombre")]
    public virtual string Nomrteent { get; set; }
    [Display(Name = "Destinatario")]
    [Required]
    public virtual string Cuitdestinatario { get; set; }
    [Display(Name = "Nombre")]
    [Required]
    public virtual string Nomdestinatario { get; set; }

    [Display(Name = "Remitente Comercial Productor")]
    public virtual string CuitRteComercialProductor { get; set; }
    [Display(Name = "Nombre")]
    public virtual string NomRteComercialProductor { get; set; }
    [Display(Name = "Remitente Comercial Venta Primaria")]
    public virtual string CuitRteComercialVentaPrimaria { get; set; }
    [Display(Name = "Nombre")]
    public virtual string NomRteComercialVentaPrimaria { get; set; }
    //public Consignacion Consignacion { get; set; }
    public virtual IList<CodigosAlfanumericos> CodigosDias { get; set; }
    public virtual string Observaciones { get; set; }
    public virtual string DetalleCupoCNRT { get; set; }
    public virtual int EstadoCupoCNRT { get; set; }
    [Display(Name = "Caratula")]
    public virtual string Caratula { get; set; }
    [Display(Name = "Contacto Comercial")]
    public virtual string ContactoComercial { get; set; }

    public NuevoCupoViewModel()
    {
      servicioCupo = new ServicioCupo();
      AppSettingConfig.SetConsignacionConfig(this);
    }

    /// <summary>
    /// Genera un objeto nuevoCupoViewModel a partir de los datos en el 
    /// nuevoCupoViewModetDTO que toma como parametro
    /// </summary>
    /// <param name="NCVMDTO"></param>
    public NuevoCupoViewModel(NuevoCupoViewModelDTO NCVMDTO)
    {
      servicioCupo = new ServicioCupo();
      this.ProductoSeleccionado = NCVMDTO.Producto;
      this.Puerto = NCVMDTO.Puerto;
      this.Compcta = NCVMDTO.Compcta;
      this.Vendcta = NCVMDTO.Vendcta;
      this.Centro = NCVMDTO.Centro;
      this.VendcyoBoolValue = NCVMDTO.VendcyoBoolValue;
      this.Cuitdestinatario = NCVMDTO.Cuitdestinatario;
      this.Nomdestinatario = NCVMDTO.Nomdestinatario;
      this.Cuitsolicitante = NCVMDTO.Cuitsolicitante;
      this.Nomsolicitante = NCVMDTO.Nomsolicitante;
      this.Cuitintermediario = NCVMDTO.Cuitintermediario;
      this.Nomintermediario = NCVMDTO.Nomintermediario;
      this.Cuitmat = NCVMDTO.Cuitmat;
      this.Nommat = NCVMDTO.Nommat;
      this.Cuitrtecomercial = NCVMDTO.Cuitrtecomercial;
      this.Nomrtecomercial = NCVMDTO.Nomrtecomercial;
      this.Cuitcorrvta = NCVMDTO.Cuitcorrvta;
      this.Nomcorrvta = NCVMDTO.Nomcorrvta;
      this.Cuitcorrcomp = NCVMDTO.Cuitcorrcomp;
      this.Nomcorrcomp = NCVMDTO.Nomcorrcomp;
      this.Cuitrteent = NCVMDTO.Cuitrteent;
      this.Nomrteent = NCVMDTO.Nomrteent;
      this.CuitRteComercialProductor = NCVMDTO.CuitRteComercialProductor;
      this.NomRteComercialProductor = NCVMDTO.NomRteComercialProductor;
      this.CuitRteComercialVentaPrimaria = NCVMDTO.CuitRteComercialVentaPrimaria;
      this.NomRteComercialVentaPrimaria = NCVMDTO.NomRteComercialVentaPrimaria;
      this.DetalleCupoCNRT = NCVMDTO.DetalleCupoCNRT;
      this.EstadoCupoCNRT = NCVMDTO.EstadoCupoCNRT;
      this.VendcyoBoolValue = NCVMDTO.VendcyoBoolValue;
      this.Observaciones = NCVMDTO.Observaciones;
      this.Caratula = NCVMDTO.Caratula;
      this.ContactoComercial = NCVMDTO.ContactoComercial;
      this.CodigosDias = new List<CodigosAlfanumericos>();


      foreach (CuposPorDiaDTO cuposxDia in NCVMDTO.CodigosDias)
      {
        foreach (string alfa in cuposxDia.Turnos)
        {
          CodigosAlfanumericos codigoAlfa = new CodigosAlfanumericos
          {
            Dia = cuposxDia.Fecha,
            Alfanumerico = alfa
          };
          this.CodigosDias.Add(codigoAlfa);
        }
      }
    }

    /// <summary>
    /// Genera el encabezado de los cupos en caso que no exista, si no, modifica el campo Cuposrecibidos del mismo
    /// Crea y almacena en BD los cupos que se encuentran en las listas CodigosDia.
    /// Si el nuevo cupo tiene fecha de hoy y la hora actual es mayor a la hora limite configurada en Web.config 
    /// esos cupos no se crean ni se informan.
    /// </summary>       
    public void Save(ISession mySession = null, ITransaction myTrans = null)
    {
      CuposStore Store = new CuposStore();
      ISession session = mySession ?? HibernateUtil.OpenSession("");
      ITransaction tx = myTrans ?? session.BeginTransaction();
      try
      {
        Cupos cupo = CrearObjetoCupoEncabezado();
        IList<CodigosAlfanumericos> CodigosNoVacios = CodigosDias.Where(x => !x.CodigosIsNullOrEmpty()).ToList();
        IList<Cupos> AlfasRepetidos = GetAlfasRepetidos(cupo, CodigosNoVacios, session);
        if (AlfasRepetidos.Count == 0)
        {
          Cupos encabezadoIgual = ObtenerOCrearEncabezado(cupo, session);
          encabezadoIgual = SumarCuposRecibidosEncabezado(encabezadoIgual, CodigosNoVacios);
          Store.Update(encabezadoIgual, session);
          foreach (CodigosAlfanumericos CodigosDia in CodigosNoVacios)
          {
            this.SaveBody(cupo, CodigosDia, encabezadoIgual, true, session);
          }
          if (myTrans == null) tx.Commit();
        }
        else
        {
          if (AlfasRepetidos.Count > 0)
          {
            string alfas = "";
            foreach (Cupos c in AlfasRepetidos)
            {
              alfas += (alfas.Length > 0) ? "|" + c.Nrocupo : c.Nrocupo;
            }
            throw new ResourceServer.Models.Error.Exceptions.CodigosAlfanumericoDuplicadosException(alfas);
          }
        }
      }
      catch (Exception e)
      {
        tx.Rollback();
        throw e;
      }
      if (myTrans == null) session.Transaction.Dispose();
    }

    /// <summary>
    /// Busca alfas ya registrados en BD que coincidan con puerto, grano y fecha
    /// </summary>
    /// <param name="Encabezado"></param>
    /// <param name="Codigos"></param>
    /// <param name="Session"></param>
    /// <returns></returns>
    private IList<Cupos> GetAlfasRepetidos(Cupos Encabezado, IList<CodigosAlfanumericos> Codigos, ISession Session)
    {
      return servicioCupo.GetCuposRepetidos(Encabezado.Puerto, Encabezado.Grano, Codigos, Session);
    }

    /// <summary>
    /// Retorna la consignacion del Nuevo Cupo View Model
    /// </summary>
    /// <returns></returns>
    public virtual Consignacion GetConsignacionTrim()
    {
      return new Consignacion
      {
        Cuitsolicitante = string.IsNullOrEmpty(this.Cuitsolicitante) ? null : this.Cuitsolicitante.Trim(),
        Nomsolicitante = string.IsNullOrEmpty(this.Nomsolicitante) ? null : this.Nomsolicitante.Trim(),
        Cuitintermediario = string.IsNullOrEmpty(this.Cuitintermediario) ? null : this.Cuitintermediario.Trim(),
        Nomintermediario = string.IsNullOrEmpty(this.Nomintermediario) ? null : this.Nomintermediario.Trim(),
        Cuitrtecomercial = string.IsNullOrEmpty(this.Cuitrtecomercial) ? null : this.Cuitrtecomercial.Trim(),
        Nomrtecomercial = string.IsNullOrEmpty(this.Nomrtecomercial) ? null : this.Nomrtecomercial.Trim(),
        Cuitcorrcomp = string.IsNullOrEmpty(this.Cuitcorrcomp) ? null : this.Cuitcorrcomp.Trim(),
        Nomcorrcomp = string.IsNullOrEmpty(this.Nomcorrcomp) ? null : this.Nomcorrcomp.Trim(),
        Cuitmat = string.IsNullOrEmpty(this.Cuitmat) ? null : this.Cuitmat.Trim(),
        Nommat = string.IsNullOrEmpty(this.Nommat) ? null : this.Nommat.Trim(),
        Cuitcorrvta = string.IsNullOrEmpty(this.Cuitcorrvta) ? null : this.Cuitcorrvta.Trim(),
        Nomcorrvta = string.IsNullOrEmpty(this.Nomcorrvta) ? null : this.Nomcorrvta.Trim(),
        Cuitrteent = string.IsNullOrEmpty(this.Cuitrteent) ? null : this.Cuitrteent.Trim(),
        Nomrteent = string.IsNullOrEmpty(this.Nomrteent) ? null : this.Nomrteent.Trim(),
        Cuitdestinatario = string.IsNullOrEmpty(this.Cuitdestinatario) ? null : this.Cuitdestinatario.Trim(),
        Nomdestinatario = string.IsNullOrEmpty(this.Nomdestinatario) ? null : this.Nomdestinatario.Trim(),
        CuitRteComercialProductor = string.IsNullOrEmpty(this.CuitRteComercialProductor) ? null : this.CuitRteComercialProductor.Trim(),
        NomRteComercialProductor = string.IsNullOrEmpty(this.NomRteComercialProductor) ? null : this.NomRteComercialProductor.Trim(),
        CuitRteComercialVentaPrimaria = string.IsNullOrEmpty(this.CuitRteComercialVentaPrimaria) ? null : this.CuitRteComercialVentaPrimaria.Trim(),
        NomRteComercialVentaPrimaria = string.IsNullOrEmpty(this.NomRteComercialVentaPrimaria) ? null : this.NomRteComercialVentaPrimaria.Trim()
      };
    }

    /// <summary>
    /// Genera el Objeto Cupo que representa el encabezado.
    /// esto aun no es guardado en BD
    /// </summary>
    /// <returns></returns>
    private Cupos CrearObjetoCupoEncabezado()
    {
      Cupos cupo = new Cupos
      {
        Grano = this.ProductoSeleccionado,
        Puerto = Int64.Parse(this.Puerto),
        Compcta = Int64.Parse(this.Compcta),
        Vendcta = (this.Vendcta == null ? 0 : Int64.Parse(this.Vendcta)),
        Centro = ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroPorDefectoUsuarioLogueado(),
        Centrodist = ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroPorDefectoUsuarioLogueado()
      };
      if (this.VendcyoBoolValue)
      {
        cupo.Vendcyo = Int64.Parse(this.Vendcta);
      }
      else
      {
        cupo.Vendcyo = 0;
      }
      cupo.VendcyoBoolValue = this.VendcyoBoolValue;
      cupo.SetConsignacion(this.GetConsignacionTrim());
      cupo.Dia0 = DateTime.Now.AddDays(0).Date;
      cupo.Dia1 = DateTime.Now.AddDays(1).Date;
      cupo.Dia2 = DateTime.Now.AddDays(2).Date;
      cupo.Dia3 = DateTime.Now.AddDays(3).Date;
      cupo.Dia4 = DateTime.Now.AddDays(4).Date;
      cupo.Dia5 = DateTime.Now.AddDays(5).Date;
      cupo.Dia6 = DateTime.Now.AddDays(6).Date;
      cupo.Dia7 = DateTime.Now.AddDays(7).Date;
      cupo.Dia8 = DateTime.Now.AddDays(8).Date;
      cupo.Dia9 = DateTime.Now.AddDays(9).Date;
      cupo.Dia10 = DateTime.Now.AddDays(10).Date;
      cupo.Dia11 = DateTime.Now.AddDays(11).Date;
      cupo.Dia12 = DateTime.Now.AddDays(12).Date;
      cupo.Dia13 = DateTime.Now.AddDays(13).Date;
      cupo.Dia14 = DateTime.Now.AddDays(14).Date;
      cupo.Dia15 = DateTime.Now.AddDays(15).Date;
      cupo.Dia16 = DateTime.Now.AddDays(16).Date;
      cupo.Dia17 = DateTime.Now.AddDays(17).Date;
      cupo.Dia18 = DateTime.Now.AddDays(18).Date;
      cupo.Dia19 = DateTime.Now.AddDays(19).Date;
      cupo.Dia20 = DateTime.Now.AddDays(20).Date;
      cupo.Observa = this.Observaciones;
      cupo.Fecha = DateTime.Now;
      cupo.EstadoCupoCNRT = this.EstadoCupoCNRT;
      cupo.DetalleCupoCNRT = this.DetalleCupoCNRT;
      cupo.Usuario = ClaimsUtil.GetClaim("Usuario");
      cupo.Caratula = this.Caratula;
      cupo.ContactoComercial = this.ContactoComercial;
      return cupo;
    }

    private Cupos CrearCupo(Cupos Encabezado, DateTime Fecha, string Codigo, Cupos CupoPadreExistente)
    {
      Cupos cupoCuerpoNuevo = new Cupos();
      cupoCuerpoNuevo.Grano = Encabezado.Grano;
      cupoCuerpoNuevo.Puerto = Encabezado.Puerto;
      cupoCuerpoNuevo.Fecha = Fecha;
      cupoCuerpoNuevo.Compcta = Encabezado.Compcta;
      cupoCuerpoNuevo.Vendcta = Encabezado.Vendcta;
      cupoCuerpoNuevo.Vendcyo = Encabezado.Vendcyo;
      cupoCuerpoNuevo.Centro = Encabezado.Centro;
      cupoCuerpoNuevo.Centrodist = Encabezado.Centro;
      cupoCuerpoNuevo.SetConsignacion(Encabezado.GetConsignacion());
      cupoCuerpoNuevo.Nrocupo = Codigo;
      cupoCuerpoNuevo.Status = 0;
      cupoCuerpoNuevo.Tipo = 1;
      cupoCuerpoNuevo.Observa = Encabezado.Observa;
      if (CupoPadreExistente != null && CupoPadreExistente.Id != 0)
      {
        cupoCuerpoNuevo.Idorigen = CupoPadreExistente.Id;
      }
      else
      {
        throw new Exception(string.Format("Falta Id de cupo Encabezado. Encabezado = {0}", CupoPadreExistente.ToString()));
      }
      cupoCuerpoNuevo.Usuario = ClaimsUtil.GetClaim("Usuario");
      cupoCuerpoNuevo.EstadoCupoCNRT = Encabezado.EstadoCupoCNRT;
      cupoCuerpoNuevo.DetalleCupoCNRT = Encabezado.DetalleCupoCNRT;
      cupoCuerpoNuevo.Caratula = Encabezado.Caratula;
      cupoCuerpoNuevo.ContactoComercial = Encabezado.ContactoComercial;
      return cupoCuerpoNuevo;
    }
    /// <summary>
    /// crea el tantos cupos hijos como alfanumericos retorne el obtenerCodigosAlfanumericos. Ya que puede estar dando
    /// de alta un alfa o un numero que indica cuantos alfas debe generar y lo almacena en BD        
    /// </summary>
    /// <param name="CupoEncabezado"></param>
    /// <param name="codigo"></param>
    /// <param name="xcupoPadreAModificar"></param>
    /// <param name="esElUltimoDia"></param>
    /// <param name="Session"></param>
    public void SaveBody(Cupos CupoEncabezado, CodigosAlfanumericos codigo, Cupos xcupoPadreAModificar, bool esElUltimoDia, ISession Session)
    {
      string[] codigos = codigo.obtenerCodigosAlfanumericos();
      /*veamos si las consignaciones de estos cuerpos coinciden con otras*/
      string Pdia = "Dia" + codigo.NumeroDia;
      DateTime fechaDia = (DateTime)CupoEncabezado.GetType().GetProperty(Pdia).GetValue(CupoEncabezado, null);
      Cupos datosdeconsignacion = (Cupos)CupoEncabezado.Clone();
      datosdeconsignacion.Fecha = fechaDia.Date;
      CuposStore storecupo = new CuposStore();
      foreach (string codigoAlfa in codigos)
      {
        Cupos cupoCuerpoNuevo = CrearCupo(CupoEncabezado, datosdeconsignacion.Fecha, codigoAlfa, xcupoPadreAModificar);
        storecupo.Save(cupoCuerpoNuevo, Session);
      }

    }

    /// <summary>
    /// Busca en Base de Datos un encabezado con los mismos datos.
    /// </summary>
    /// <param name="nuevoPadre"></param>
    /// <param name="Session"></param>
    /// <returns></returns>
    public Cupos ExisteEncabezadoIgual(Cupos nuevoPadre, ISession Session)
    {
      CuposStore store = new CuposStore();
      IList<Cupos> listaCupo = store.FindEncabezado(nuevoPadre.Compcta, nuevoPadre.Vendcta, nuevoPadre.Puerto, nuevoPadre.Grano, nuevoPadre.Fecha.Date, nuevoPadre.GetConsignacion(), nuevoPadre.Vendcyo, Session);
      if (listaCupo != null && listaCupo.Count() != 0)
      {
        return listaCupo.ElementAt(0);
      }
      return null;
    }
    /// <summary>
    /// Obtiene, de BD, el Encabezado. En caso de que no exista crea uno con los datos pasados como parámetro.
    /// y le setea en el campo ID al valor retornado por el proceso de alta del registro en BD
    /// </summary>
    /// <param name="Encabezado"></param>
    /// <param name="Session"></param>
    /// <returns></returns>
    public Cupos ObtenerOCrearEncabezado(Cupos Encabezado, ISession Session)
    {
      Cupos CupoEncabezado = ExisteEncabezadoIgual(Encabezado, Session);
      if (CupoEncabezado != null && CupoEncabezado.Id != 0)
      {
        return CupoEncabezado;
      }
      else
      {
        CuposStore Store = new CuposStore();
        Encabezado.Id = (Int64)Store.SaveAndReturn(Encabezado, Session);
        return Encabezado;
      }
    }

    /// <summary>
    /// incrementa los cupos recibidos en el encabezado
    /// y lo retorna
    /// </summary>
    /// <param name="Encabezado"></param>
    /// <param name="ListaAlfanumericos"></param>
    /// <returns></returns>
    public Cupos SumarCuposRecibidosEncabezado(Cupos Encabezado, IList<CodigosAlfanumericos> ListaAlfanumericos)
    {
      Encabezado.Cuposrecibidos += ListaAlfanumericos.Sum(x => x.GetCantidadCupos());
      return Encabezado;
    }

    /// <summary>
    /// Dado un codigo alfanumerico pasado como parametro, lo elimina de la lista CodigosDias
    /// </summary>
    /// <param name="CodigoAlfanumerico"></param>
    /// <param name="fecha"></param>
    public void EliminarAlfa(string CodigoAlfanumerico, DateTime fecha)
    {
      CodigosAlfanumericos alfanumerico = (CodigosAlfanumericos)this.CodigosDias.Where(x => x.Dia == fecha && x.Alfanumerico == CodigoAlfanumerico).FirstOrDefault();
      this.CodigosDias.Remove(alfanumerico);
    }

    /// <summary>
    /// Retorna una lista de todos los alfas contenidos en la lista CodigosDias. Sin distinguir por Fecha
    /// </summary>
    /// <returns></returns>
    public List<string> GetAlfas()
    {
      var alfas = this.CodigosDias.Select(x => x.Alfanumerico).ToList();
      return (List<string>)alfas;
    }
  }
}