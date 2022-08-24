using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;

namespace ResourceServer
{
    public class ListToHtmlTable
    {
        protected string alineadoCabeceras = "center";
        protected string alineadoColumnas = "right";
        protected List<string> totales;

        public ListToHtmlTable(string alineaCabecera, string alineaColumnas, List<string> xtotales) 
        {
            this.alineadoCabeceras = alineaCabecera;
            this.alineadoColumnas = alineaColumnas;
            this.totales = xtotales;
        }

        public string GetMyTable<T>(IEnumerable<T> list, params  Expression<Func<T, object>>[] fxns)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE>\n");

            sb.Append("<TR>\n");
            foreach (var fxn in fxns)
            {
                sb.Append("<TD align='" + this.alineadoCabeceras + "'><b>");
                sb.Append(GetName(fxn));
                sb.Append("</b></TD>");
            }
            sb.Append("</TR> <!-- HEADER -->\n");


            foreach (var item in list)
            {
                sb.Append("<TR>\n");
                foreach (var fxn in fxns)
                {
                    sb.Append("<TD align='" + this.alineadoColumnas + "'>");
                    sb.Append(fxn.Compile()(item));
                    sb.Append("</TD>");
                }
                sb.Append("</TR>\n");
            }
            
                sb.Append("<TR>\n");
                foreach (var fxn in this.totales)
                {
                    sb.Append("<TD align='" + this.alineadoColumnas + "'><b>");
                    sb.Append(fxn);
                    sb.Append("<b/></TD>");
                }
                sb.Append("</TR> <!-- FOOTER -->\n");

            sb.Append("</TABLE>");

            return sb.ToString();
        }

        protected string GetName<T>(Expression<Func<T, object>> expr)
        {
            var member = expr.Body as MemberExpression;
            if (member != null)
                return GetName2(member);

            var unary = expr.Body as UnaryExpression;
            if (unary != null)
                return GetName2((MemberExpression)unary.Operand);

            return "?+?";
        }

        protected string GetName2(MemberExpression member)
        {
            var fieldInfo = member.Member as FieldInfo;
            if (fieldInfo != null)
            {
                var d = fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (d != null) return d.Description;
                return fieldInfo.Name;
            }

            var propertInfo = member.Member as PropertyInfo;
            if (propertInfo != null)
            {
                var d = propertInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (d != null) return d.Description;
                return propertInfo.Name;
            }

            return "?-?";
        }
    }
}