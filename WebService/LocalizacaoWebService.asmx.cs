using ClassesCompartilhadas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace WebService
{
    /// <summary>
    /// Summary description for Localizacao
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LocalizacaoWebService : Classes.WebServiceComum
    {
        [WebMethod]
        public string Buscar(string busca)
        {
            return string.Empty;
        }
    }
}
