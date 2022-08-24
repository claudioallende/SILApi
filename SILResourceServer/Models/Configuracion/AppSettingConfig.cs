using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Configuracion
{
    public class AppSettingConfig
    {
        public static string HoraLimite = ConfigurationManager.AppSettings["horaLimite"];

        public static DateTime GetHoraLimite(DateTime dia)
        {
            TimeSpan hora_limite = TimeSpan.ParseExact(HoraLimite, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime limite = dia.Date + hora_limite;
            return limite;
        }

        public static string GetFechaDesdeDistribucion()
        {
            string Fecha = ConfigurationManager.AppSettings["fechaDesdeFiltroDistribucion"];
            if (!string.IsNullOrEmpty(Fecha))
            {
                return Fecha;
            }
            else
            {
                return "";
            }
        }

        public static string GetFechaHastaDistribucion()
        {
            string Fecha = ConfigurationManager.AppSettings["fechaHastaFiltroDistribucion"];
            if (!string.IsNullOrEmpty(Fecha))
            {
                return Fecha;
            }
            else
            {
                return "";
            }
        }

        public static string GetCosechaDesdeDistribucion()
        {
            string Fecha = ConfigurationManager.AppSettings["cosechaDesdeFiltroDistribucion"];
            if (!string.IsNullOrEmpty(Fecha))
            {
                return Fecha;
            }
            else
            {
                return "";
            }
        }

        public static string GetCosechaHastaDistribucion()
        {
            string Fecha = ConfigurationManager.AppSettings["cosechaHastaFiltroDistribucion"];
            if (!string.IsNullOrEmpty(Fecha))
            {
                return Fecha;
            }
            else
            {
                return "";
            }
        }

        public static string GetHoraLimite()
        {
            string HoraLimite = ConfigurationManager.AppSettings["horaLimite"];
            if (!string.IsNullOrEmpty(HoraLimite))
            {
                TimeSpan hora_limite = TimeSpan.ParseExact(HoraLimite, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture);
                return hora_limite.ToString(@"hh\:mm");
            }
            else
            {
                return "";
            }
        }

        public static string GetHoraEnvioEmailInformadosSTOP()
        {
            string HoraLimite = ConfigurationManager.AppSettings["horaEnvioEmailInformadosSTOP"];
            if (!string.IsNullOrEmpty(HoraLimite))
            {
                TimeSpan hora_limite = TimeSpan.ParseExact(HoraLimite, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture);
                return hora_limite.ToString(@"hh\:mm");
            }
            else
            {
                return "";
            }
        }

        public static string GetHoraEnvioEmailNoCumplimientoInformadosSTOP()
        {
            string HoraLimite = ConfigurationManager.AppSettings["horaEnvioEmailNoCumplimientoInformadosSTOP"];
            if (!string.IsNullOrEmpty(HoraLimite))
            {
                TimeSpan hora_limite = TimeSpan.ParseExact(HoraLimite, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture);
                return hora_limite.ToString(@"hh\:mm");
            }
            else
            {
                return "";
            }
        }

    /// <summary>
    /// Pongo el objeto NuevoCupoViewModel porque solo ahí se setean por defecto, si en algun otro lado se necesita lo mismo va a requerir usar interface.
    /// </summary>
    /// <param name="model"></param>
    public static void SetConsignacionConfig(NuevoCupoViewModel model)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreSolicitante"])) model.Nomsolicitante = ConfigurationManager.AppSettings["nombreSolicitante"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitSolicitante"])) model.Cuitsolicitante = ConfigurationManager.AppSettings["cuitSolicitante"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreIntermediario"])) model.Nomintermediario = ConfigurationManager.AppSettings["nombreIntermediario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitIntermediario"])) model.Cuitintermediario = ConfigurationManager.AppSettings["cuitIntermediario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRemitenteComercial"])) model.Nomrtecomercial = ConfigurationManager.AppSettings["nombreRemitenteComercial"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRemitenteComercial"])) model.Cuitrtecomercial = ConfigurationManager.AppSettings["cuitRemitenteComercial"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreCorredorComprador"])) model.Nomcorrcomp = ConfigurationManager.AppSettings["nombreCorredorComprador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitCorredorComprador"])) model.Cuitcorrcomp = ConfigurationManager.AppSettings["cuitCorredorComprador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreMercadoATermino"])) model.Nommat = ConfigurationManager.AppSettings["nombreMercadoATermino"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitMercadoATermino"])) model.Cuitmat = ConfigurationManager.AppSettings["cuitMercadoATermino"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreCorredorVendedor"])) model.Nomcorrvta = ConfigurationManager.AppSettings["nombreCorredorVendedor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitCorredorVendedor"])) model.Cuitcorrvta = ConfigurationManager.AppSettings["cuitCorredorVendedor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRepresentanteEntregador"])) model.Nomrteent = ConfigurationManager.AppSettings["nombreRepresentanteEntregador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRepresentanteEntregador"])) model.Cuitrteent = ConfigurationManager.AppSettings["cuitRepresentanteEntregador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreDestinatario"])) model.Nomdestinatario = ConfigurationManager.AppSettings["nombreDestinatario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitDestinatario"])) model.Cuitdestinatario = ConfigurationManager.AppSettings["cuitDestinatario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRemitenteComercialProductor"])) model.NomRteComercialProductor = ConfigurationManager.AppSettings["nombreRemitenteComercialProductor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRemitenteComercialProductor"])) model.CuitRteComercialProductor = ConfigurationManager.AppSettings["cuitRemitenteComercialProductor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRemitenteComercialVentaPrimaria"])) model.NomRteComercialVentaPrimaria = ConfigurationManager.AppSettings["nombreRemitenteComercialVentaPrimaria"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRemitenteComercialVentaPrimaria"])) model.CuitRteComercialVentaPrimaria = ConfigurationManager.AppSettings["cuitRemitenteComercialVentaPrimaria"];

    }

        public static void SetConsignacionConfigurada(Consignacion model)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreSolicitante"])) model.Nomsolicitante = ConfigurationManager.AppSettings["nombreSolicitante"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitSolicitante"])) model.Cuitsolicitante = ConfigurationManager.AppSettings["cuitSolicitante"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreIntermediario"])) model.Nomintermediario = ConfigurationManager.AppSettings["nombreIntermediario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitIntermediario"])) model.Cuitintermediario = ConfigurationManager.AppSettings["cuitIntermediario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRemitenteComercial"])) model.Nomrtecomercial = ConfigurationManager.AppSettings["nombreRemitenteComercial"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRemitenteComercial"])) model.Cuitrtecomercial = ConfigurationManager.AppSettings["cuitRemitenteComercial"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreCorredorComprador"])) model.Nomcorrcomp = ConfigurationManager.AppSettings["nombreCorredorComprador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitCorredorComprador"])) model.Cuitcorrcomp = ConfigurationManager.AppSettings["cuitCorredorComprador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreMercadoATermino"])) model.Nommat = ConfigurationManager.AppSettings["nombreMercadoATermino"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitMercadoATermino"])) model.Cuitmat = ConfigurationManager.AppSettings["cuitMercadoATermino"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreCorredorVendedor"])) model.Nomcorrvta = ConfigurationManager.AppSettings["nombreCorredorVendedor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitCorredorVendedor"])) model.Cuitcorrvta = ConfigurationManager.AppSettings["cuitCorredorVendedor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRepresentanteEntregador"])) model.Nomrteent = ConfigurationManager.AppSettings["nombreRepresentanteEntregador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRepresentanteEntregador"])) model.Cuitrteent = ConfigurationManager.AppSettings["cuitRepresentanteEntregador"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreDestinatario"])) model.Nomdestinatario = ConfigurationManager.AppSettings["nombreDestinatario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitDestinatario"])) model.Cuitdestinatario = ConfigurationManager.AppSettings["cuitDestinatario"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRemitenteComercialProductor"])) model.NomRteComercialProductor = ConfigurationManager.AppSettings["nombreRemitenteComercialProductor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRemitenteComercialProductor"])) model.CuitRteComercialProductor = ConfigurationManager.AppSettings["cuitRemitenteComercialProductor"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["nombreRemitenteComercialVentaPrimaria"])) model.NomRteComercialVentaPrimaria = ConfigurationManager.AppSettings["nombreRemitenteComercialVentaPrimaria"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["cuitRemitenteComercialVentaPrimaria"])) model.CuitRteComercialVentaPrimaria = ConfigurationManager.AppSettings["cuitRemitenteComercialVentaPrimaria"];

    }

    public void Save(PanelViewModel Configuracion)
        {
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);  
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            SaveItem(config, "nombreSolicitante", Configuracion.Consignacion.Nomsolicitante);
            SaveItem(config, "cuitSolicitante", Configuracion.Consignacion.Cuitsolicitante);
            SaveItem(config, "nombreIntermediario", Configuracion.Consignacion.Nomintermediario);
            SaveItem(config, "cuitIntermediario", Configuracion.Consignacion.Cuitintermediario);
            SaveItem(config, "nombreRemitenteComercial", Configuracion.Consignacion.Nomrtecomercial);
            SaveItem(config, "cuitRemitenteComercial", Configuracion.Consignacion.Cuitrtecomercial);
            SaveItem(config, "nombreCorredorComprador", Configuracion.Consignacion.Nomcorrcomp);
            SaveItem(config, "cuitCorredorComprador", Configuracion.Consignacion.Cuitcorrcomp);
            SaveItem(config, "nombreMercadoATermino", Configuracion.Consignacion.Nommat);
            SaveItem(config, "cuitMercadoATermino", Configuracion.Consignacion.Cuitmat);
            SaveItem(config, "nombreCorredorVendedor", Configuracion.Consignacion.Nomcorrvta);
            SaveItem(config, "cuitCorredorVendedor", Configuracion.Consignacion.Cuitcorrvta);
            SaveItem(config, "nombreRepresentanteEntregador", Configuracion.Consignacion.Nomrteent);
            SaveItem(config, "cuitRepresentanteEntregador", Configuracion.Consignacion.Cuitrteent);
            SaveItem(config, "nombreDestinatario", Configuracion.Consignacion.Nomdestinatario);
            SaveItem(config, "cuitDestinatario", Configuracion.Consignacion.Cuitdestinatario);
            SaveItem(config, "nombreRemitenteComercialProductor", Configuracion.Consignacion.NomRteComercialProductor);
            SaveItem(config, "cuitRemitenteComercialProductor", Configuracion.Consignacion.CuitRteComercialProductor);
            SaveItem(config, "nombreRemitenteComercialVentaPrimaria", Configuracion.Consignacion.NomRteComercialVentaPrimaria);
            SaveItem(config, "cuitRemitenteComercialVentaPrimaria", Configuracion.Consignacion.CuitRteComercialVentaPrimaria);
            SaveItem(config, "horaLimite", Configuracion.HoraLimiteDistribucion);
            SaveItem(config, "fechaDesdeFiltroDistribucion", Configuracion.FechaDesdeDistribucion);
            SaveItem(config, "fechaHastaFiltroDistribucion", Configuracion.FechaHastaDistribucion);
            SaveItem(config, "cosechaDesdeFiltroDistribucion", Configuracion.CosechaDesdeDistribucion);
            SaveItem(config, "cosechaHastaFiltroDistribucion", Configuracion.CosechaHastaDistribucion);
            SaveItem(config, "horaEnvioEmailInformadosSTOP", Configuracion.HoraEnvioMailSTOP);
            SaveItem(config, "horaEnvioEmailNoCumplimientoInformadosSTOP", Configuracion.HoraEnvioMailNoCumplimiento);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void SaveItem(System.Configuration.Configuration config, string key, string value)
        {
            if (config.AppSettings.Settings[key] != null && !string.IsNullOrEmpty(value)) config.AppSettings.Settings[key].Value = value;
        }
    }
}