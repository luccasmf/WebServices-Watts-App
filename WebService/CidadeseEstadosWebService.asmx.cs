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
    /// Summary description for CidadeseEstados
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CidadeseEstadosWebService : Classes.WebServiceComum
    {
        [WebMethod]
        public string BuscaEstados()
        {
            Estado estado = new Estado();
            Estado[] estados = estado.BuscarEstados();

            return jsonSerializer.Serialize(estados);
        }

        [WebMethod]
        public string BuscaCidadesPorIdEstado(int idEstado)
        {
            Cidade c = new Cidade();
            Cidade[] cidades = c.BuscarCidadePorEstado(idEstado);

            return jsonSerializer.Serialize(cidades);
        }

        [WebMethod]
        public string BuscaCidadesPorEstado(string uf)
        {
            Cidade c = new Cidade();
            Cidade[] cidades = c.BuscarCidadePorEstado(uf);

            return jsonSerializer.Serialize(cidades);
        }
    }
}
