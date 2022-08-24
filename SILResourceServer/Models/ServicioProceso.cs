using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioProceso
    {
        public void GuardarProceso(EstadoProceso Estado)
        {
            EstadoProcesoStore store = new EstadoProcesoStore();
            store.Save(Estado);
        }

        /// <summary>
        /// Construye un objeto proceso que indica que nunca se ejecuto.
        /// </summary>
        /// <param name="IdProceso"></param>
        /// <returns></returns>
        public EstadoProceso BuildEstadoProcesoNuncaSeEjecuto(long IdProceso)
        {
            return new EstadoProceso
            {
                IdProceso = IdProceso,
                Estado = 0,
                FechaExec = DateTime.Now
            };
        }

        /// <summary>
        /// Construye un objeto proceso que indica que esta en ejecucion.
        /// </summary>
        /// <param name="IdProceso"></param>
        /// <returns></returns>
        public EstadoProceso BuildEstadoProcesoEnEjecucion(long IdProceso)
        {
            return new EstadoProceso
            {
                IdProceso = IdProceso,
                Estado = 1,
                FechaExec = DateTime.Now
            };
        }

        /// <summary>
        /// Construye un objeto proceso que indica que termino de manera correcta sin errores.
        /// </summary>
        /// <param name="IdProceso"></param>
        /// <returns></returns>
        public EstadoProceso BuildEstadoProcesoTerminadoOk(long IdProceso)
        {
            return new EstadoProceso
            {
                IdProceso = IdProceso,
                Estado = 2,
                FechaExec = DateTime.Now
            };
        }

        /// <summary>
        /// Construye un objeto proceso que indica que se empezo el proceso y se interrumpio en la mitad.
        /// </summary>
        /// <param name="IdProceso"></param>
        /// <param name="CodigoError"></param>
        /// <param name="DetalleError"></param>
        /// <returns></returns>
        public EstadoProceso BuildEstadoProcesoErrorBloqueante(long IdProceso, string CodigoError, string DetalleError)
        {
            return new EstadoProceso
            {
                IdProceso = IdProceso,
                Estado = 3,
                FechaExec = DateTime.Now,
                ErrorDetalExec = CodigoError,
                ErrorDetalle = DetalleError
            };
        }

        /// <summary>
        /// Construye un objeto proceso que indica que se empezo el proceso y en el medio ocurrio un error.
        /// </summary>
        /// <param name="IdProceso"></param>
        /// <param name="CodigoError"></param>
        /// <param name="DetalleError"></param>
        /// <returns></returns>
        public EstadoProceso BuildEstadoProcesoErrorNoBloqueante(long IdProceso, string CodigoError, string DetalleError)
        {
            return new EstadoProceso
            {
                IdProceso = IdProceso,
                Estado = 4,
                FechaExec = DateTime.Now,
                ErrorDetalExec = CodigoError,
                ErrorDetalle = DetalleError
            };
        }
    }
}