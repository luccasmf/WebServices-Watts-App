using ClassesCompartilhadas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebService
{
    /// <summary>
    /// Summary description for RetornosSimplesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RetornosSimplesWebService : Classes.WebServiceComum
    {
        [WebMethod]
        public string RetornaComodos()
        {
            Comodo c = new Comodo();
            Comodo[] comodos = c.BuscarComodos();

            return jsonSerializer.Serialize(comodos);
            //return "Hello World";
        }

        [WebMethod]
        public string RetornaCategorias()
        {
            Categoria c = new Categoria();

            return jsonSerializer.Serialize(c.BuscarCategorias());
            //return "Hello World";
        }
    }
}
