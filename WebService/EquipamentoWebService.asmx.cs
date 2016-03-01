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
    /// Summary description for Equipamento
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EquipamentoWebService : Classes.WebServiceComum
    {       

        [WebMethod]
        public string Salvar(string entidade)
        {
            Equipamento eqp = jsonSerializer.Deserialize<Equipamento>(entidade);
            bool salvou = eqp.InserirEquipamento();

            return jsonSerializer.Serialize(salvou);
        }

        [WebMethod]
        public string Deletar(int Id)
        {
            Equipamento eq = new Equipamento();
            bool apagou = eq.DeletarEquipamento(Id);

            return jsonSerializer.Serialize(apagou);
        }
        
        [WebMethod]
        public string BuscarTodosEquipamentos(string busca)
        {
           Equipamento eq = new Equipamento();
           Equipamento[] eqp = eq.BuscarEquipamentos();

           return jsonSerializer.Serialize(eqp);
        }

        [WebMethod]
        public string BuscarEquipamentosPorId(int id)
        {
            Equipamento eq = new Equipamento();
            Equipamento eqp = eq.BuscarEquipamentoPorId(id);

            return jsonSerializer.Serialize(eqp);
        }

        [WebMethod]
        public string BuscarEquipamentosPorMarca(int idMarca)
        {
            Equipamento eq = new Equipamento();
            Equipamento[] eqp = eq.BuscarEquipamentoPorMarca(idMarca);

            return jsonSerializer.Serialize(eqp);
        }

        
        [WebMethod]
        public string BuscarEquipamentosPorCategoria(int idCategoria)
        {
            Equipamento eq = new Equipamento();
            Equipamento[] eqp = eq.BuscarEquipamentoPorCategoria(idCategoria);

            return jsonSerializer.Serialize(eqp);
        }

        [WebMethod]
        public string BuscarEquipamentosPorCategoriaeComodo(int idCategoria, int idComodo)
        {
            Equipamento eq = new Equipamento();
            Equipamento[] eqp = eq.BuscarEquipamentoPorCategoriaeComodo(idCategoria,idComodo);

            return jsonSerializer.Serialize(eqp);
        }

        [WebMethod]
        public string BuscarEquipamentosPorComodo(int idComodo)
        {
            Equipamento eq = new Equipamento();
            Equipamento[] eqp = eq.BuscarEquipamentoPorComodo(idComodo);

            return jsonSerializer.Serialize(eqp);
        }

    }
}
