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
    /// Summary description for DicasWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DicasWebService : Classes.WebServiceComum
    {

        [WebMethod]
        public string RetornaDica()
        {
            Random r = new Random();
            Dicas d = new Dicas();
            int quant = QuantidadeDeDicas();
            while (true)
            {
                int id = r.Next(1, quant);

                try
                {
                    string retorno = jsonSerializer.Serialize(d.BuscarDicasPorId(id));
                    if (retorno == "null")
                    {
                        throw new Exception("Dica veio nula, outro numero será sortead");
                    }
                    else
                    {
                        return retorno;
                    }
                }
                catch(Exception e)
                {

                }
            }
        }

        public int QuantidadeDeDicas()
        {
            Dicas d = new Dicas();
            return d.QuantidadeDeDicas();
        }

    }
}
