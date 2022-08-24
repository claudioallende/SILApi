using ResourceServer.Models.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioCuenta
    {
        private ICuentaStore Store { get; set; }
        public ServicioCuenta(ICuentaStore s)
        {
            this.Store = s;
        }
        public IList<ICuenta> Get(string Text)
        {
            if (EsNumero(Text))
            {
                return GetFromNumeroCuenta(Text);
            }
            else
            {
                return GetFromNombreCuenta(Text);
            }
        }
        //Igual a GetNameFromCuenta pero con otro nombre mas representativo
        public IList<ICuenta> GetFromNumeroCuenta(string Cuenta, int Limit = 10)
        {
            return Store.FindStartsWithCuentaLimit(Int64.Parse(Cuenta), Limit).ToList();
        }
        //Igual a GetNameFromCuenta pero con otro nombre mas representativo
        public IList<ICuenta> GetFromNumeroCuenta(string Cuenta, ISession Session, int Limit = 10)
        {
            return Store.FindStartsWithCuentaLimit(Int64.Parse(Cuenta), Session, Limit).ToList();
        }
        //Igual a GetCuentaFromName pero con otro nombre mas representativo
        public IList<ICuenta> GetFromNombreCuenta(string Nombre, int Limit = 10)
        {
            return Store.FindLikeIgnoreCaseNombreLimit(Nombre, Limit).ToList();
        }
        public IList<ICuenta> GetFromCuit(string Cuit, int Limit = 10)
        {
            return Store.FindByCuitLimit(Cuit, Limit);
        }
        public IList<ICuenta> GetAllFromCuit(string Cuit)
        {
            return Store.FindByCuit(Cuit);
        }
        [Obsolete("Nombre no representativo. Usar el método GetFromNombreCuenta")]
        public IList<ICuenta> GetCuentaFromName(string name, int limit = 10)
        {
            return Store.FindLikeIgnoreCaseNombreLimit(name, limit).ToList();
        }
        [Obsolete("Nombre no representativo. Usar el método GetFromNumeroCuenta")]
        public IList<ICuenta> GetNameFromCuenta(string cuenta)
        {
            return Store.FindStartsWithCuentaLimit(Int64.Parse(cuenta), 10).ToList();
        }
        public bool EsNumero(string Text)
        {
            long n;
            return long.TryParse(Text, out n);
        }
        public ICuenta GetNombreAndCuitFromCuenta(long cuenta)
        {
            return Store.FindNombreAndCuitByCuenta(cuenta);
        }
        public IList<ICuenta> GetLikeName(string value)
        {
            return Store.FindLikeNombreLimit(value, 10);
        }
        public IList<string> GetName(string value)
        {
            return Store.FindNombreLikeNombre(value);
        }
    }
}