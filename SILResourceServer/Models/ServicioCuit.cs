using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioCuit
    {
        private ICuitStore Store;
        public ServicioCuit()
        {
            Store = new CuitStore();
        }
        public IList<string> Get(string Text)
        {
            if (EsProbableNumeroCuit(Text))
            {
                return GetNameFromCuit(Text);
            }
            else
            {
                return GetCuitFromName(Text);
            }
        }
        public IList<string> GetCuitFromName(string Name)
        {
            return Store.FindLikeNombreLimit(Name, 20).Select(x => x.Cuit).ToList();
        }
        public IList<string> GetNameFromCuit(string Cuit)
        {
            return Store.FindLikeCuitLimit(Cuit, 20).Select(x => x.Cuit).ToList();
        }
        public bool EsNumero(string Text)
        {
            int n;
            return int.TryParse(Text, out n);
        }
        public bool EsProbableNumeroCuit(string Text)
        {
            return Text.Length <= 11 && EsNumero(Text);
        }
        public bool EsNumeroCuitValido(string Text)
        {
            if (Text == null)
            {
                return false;
            }
            //Quito los guiones, el cuit resultante debe tener 11 caracteres.
            Text = Text.Replace("-", string.Empty);
            if (Text.Length != 11)
            {
                return false;
            }
            else
            {
                int calculado = CalcularDigitoCuit(Text);
                int digito = int.Parse(Text.Substring(10));
                return calculado == digito;
            }
        }
        public static int CalcularDigitoCuit(string cuit)
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuit.ToCharArray();
            int total = 0;
            for (int i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }
    }
}