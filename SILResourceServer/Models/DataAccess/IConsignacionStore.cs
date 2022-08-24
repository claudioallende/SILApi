using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IConsignacionStore
    {
        /// <summary>
        /// Busca consignaciones por los parámetros desde la fecha del día de hoy.
        /// </summary>
        /// <param name="grano"></param>
        /// <param name="comp"></param>
        /// <param name="vend"></param>
        /// <param name="puerto"></param>
        /// <returns></returns>
        IList<Consignacion> FindByGranoAndCompAndVendAndPuerto(int grano, long comp, long vend, long puerto);
        /// <summary>
        /// Busca consignaciones por los parámetros desde la fecha del día de hoy.
        /// </summary>
        /// <param name="grano"></param>
        /// <param name="comp"></param>
        /// <param name="vend"></param>
        /// <param name="puerto"></param>
        /// <param name="vendcyo">Conjunto de valores posibles de Vendcyo</param>
        /// <returns></returns>
        IList<Consignacion> FindByGranoAndCompAndVendAndPuertoAndVendcyo(int grano, long comp, long vend, long puerto, long[] vendcyo);
        IList<Consignacion> FindByGranoAndCompAndVendAndPuertoAndVendcyoAndCentroAndCentrodist(int grano, long comp, long vend, long puerto, FiltroCentro Filtro, long[] vendcyo);
        IList<Consignacion> FindByGranoAndCompAndVendAndPuertoInStatus(int grano, long comp, long vend, long puerto, int[] status);
        /// <summary>
        /// Busca consignaciones por los parámetros desde la fecha del día de hoy.
        /// </summary>
        /// <param name="grano"></param>
        /// <param name="comp"></param>
        /// <param name="vend"></param>
        /// <param name="puerto"></param>
        /// <param name="notstatus">Conjunto de status del cupo que no debe tener</param>
        /// <returns></returns>
        IList<Consignacion> FindByGranoAndCompAndVendAndPuertoNotInStatus(int grano, long comp, long vend, long puerto, int[] notstatus);
        /// <summary>
        /// Busca consignaciones por los parámetros desde la fecha del día de hoy.
        /// </summary>
        /// <param name="grano"></param>
        /// <param name="comp"></param>
        /// <param name="vend"></param>
        /// <param name="puerto"></param>
        /// <param name="vendcyo">Conjunto de valores posibles de Vendcyo</param>
        /// <param name="notstatus">Conjunto de status del cupo que no debe tener</param>
        /// <returns></returns>
        IList<Consignacion> FindByGranoAndCompAndVendAndPuertoAndVendcyoNotInStatus(int grano, long comp, long vend, long puerto, long[] vendcyo, int[] notstatus);
    }
}
