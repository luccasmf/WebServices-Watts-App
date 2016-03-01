using ClassesCompartilhadas.Entidades;
using ClassesCompartilhadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Text;
using System.Data.SqlClient;

namespace WebService
{
    /// <summary>
    /// Summary description for EquipamentoUsuarioWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EquipamentoUsuarioWebService : Classes.WebServiceComum
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">Entidade Usuario</param>
        /// <param name="equipamento">Entidade Equipamento</param>
        /// <param name="dadosComplementares">Dados complementares: idComodo (int) e tempoDeConsumo (timeSpan)</param>
        /// <returns></returns>
        [WebMethod]
        public string InserirEquipamentoPorUsuarioUnitario(string usuario, string equipamento, string dadosComplementares)
        {
            Usuario user = new Usuario();
            try
            {
                user = jsonSerializer.Deserialize<Usuario>(usuario);
            }
            catch
            {
                try
                {
                    int IdUsuario = jsonSerializer.Deserialize<int>(usuario);
                    user = user.BuscarUsuarioPorId(IdUsuario);
                }
                catch (Exception e)
                {
                    return "";
                }
            }

            Equipamento eqp = new Equipamento();
            try
            {
                eqp = jsonSerializer.Deserialize<Equipamento>(equipamento);
            }

            catch
            {
                try
                {
                    int IdEquipamento = jsonSerializer.Deserialize<int>(equipamento);
                    eqp = eqp.BuscarEquipamentoPorId(IdEquipamento);
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            DadosComplementares dc = new DadosComplementares();
            try { dc = jsonSerializer.Deserialize<DadosComplementares>(dadosComplementares); }
            catch { };

            EquipamentoUsuario equipamentoUsuario = new EquipamentoUsuario();
            equipamentoUsuario.IdUsuario = user.Id;
            equipamentoUsuario.IdEquipamento = eqp.Id;
            equipamentoUsuario.Consumo = eqp.Consumo;
            equipamentoUsuario.StandBy = eqp.StandBy;
            equipamentoUsuario.IdComodo = dc.idComodo;
            equipamentoUsuario.TempoConsumo = dc.tempoDeUsoDiario;
            equipamentoUsuario.TempoStandBy = dc.tempoDeUsoDiario;

            return jsonSerializer.Serialize(equipamentoUsuario.InserirEquipamentoUsuario());
        }


        [WebMethod]
        public string InserirEquipamentoPorUsuarioAgrupado(string usuario, string equipamento, string dadosComplementares)
        {
            Usuario user = new Usuario();
            try
            {
                user = jsonSerializer.Deserialize<Usuario>(usuario);
            }
            catch
            {
                try
                {
                    int IdUsuario = jsonSerializer.Deserialize<int>(usuario);
                    user = user.BuscarUsuarioPorId(IdUsuario);
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            Equipamento[] eqp = jsonSerializer.Deserialize<Equipamento[]>(equipamento);
            DadosComplementares[] dc = jsonSerializer.Deserialize<DadosComplementares[]>(dadosComplementares);
            EquipamentoUsuario equipamentoUsuario = new EquipamentoUsuario();
            StringBuilder query = new StringBuilder();
            List<SqlParameter> parametros = new List<SqlParameter>();

            query.AppendLine("Begin tran T1");

            for (int i = 0; i < eqp.Length; i++)
            {
                equipamentoUsuario = new EquipamentoUsuario
                {
                    IdUsuario = user.Id,
                    IdEquipamento = eqp[i].Id,
                    Consumo = eqp[i].Consumo,
                    StandBy = eqp[i].StandBy,
                    IdComodo = dc[i].idComodo,
                    TempoConsumo = dc[i].tempoDeUsoDiario,
                    TempoStandBy = dc[i].tempoDeUsoDiario,
                    Quantidade = dc[i].quantidade
                };

                query.AppendFormat("Insert into EquipamentoUsuario values(@IdUsuario, @IdEquipamento{0}, @Consumo{0}, @StandBy{0}, @TempoConsumo{0}, @TempoStandBy{0}, @IdComodo{0}, @Quantidade{0});", i);

                parametros.Add(new SqlParameter(string.Format("IdEquipamento{0}", i), equipamentoUsuario.IdEquipamento));
                parametros.Add(new SqlParameter(string.Format("Consumo{0}", i), equipamentoUsuario.Consumo));
                parametros.Add(new SqlParameter(string.Format("StandBy{0}", i), equipamentoUsuario.StandBy));
                parametros.Add(new SqlParameter(string.Format("TempoConsumo{0}", i), equipamentoUsuario.TempoConsumo));
                parametros.Add(new SqlParameter(string.Format("TempoStandBy{0}", i), equipamentoUsuario.TempoStandBy));
                parametros.Add(new SqlParameter(string.Format("IdComodo{0}", i), equipamentoUsuario.IdComodo));
                parametros.Add(new SqlParameter(string.Format("Quantidade{0}", i), equipamentoUsuario.Quantidade));

            }

            parametros.Add(new SqlParameter("IdUsuario", equipamentoUsuario.IdUsuario));

            query.AppendLine("commit");

            return jsonSerializer.Serialize(equipamentoUsuario.InserirLote(query.ToString(), parametros.ToArray()));
        }

        [WebMethod]
        public string BuscaEquipamentoUsuarioPorUsuario(string usuario)
        {
            Usuario user = new Usuario();
            try
            {
               user = jsonSerializer.Deserialize<Usuario>(usuario);
            }
            catch
            {
                try
                {
                    int IdUsuario = jsonSerializer.Deserialize<int>(usuario);
                    user = user.BuscarUsuarioPorId(IdUsuario);
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            EquipamentoUsuario eqp = new EquipamentoUsuario();

            
            EquipamentoUsuario[] equipamentos = eqp.BuscarEquipaentoPorUsuario(user.Id);

            EqpRetorno preparaEqpRetorno = new EqpRetorno();
            List<EqpRetorno> eqpRetorno = new List<EqpRetorno>();
            foreach(EquipamentoUsuario equip in equipamentos)
            {
                eqpRetorno.Add(preparaEqpRetorno.converteEquipamentos(equip));
            }

            return jsonSerializer.Serialize(eqpRetorno);
        }

        [WebMethod]
        public string BuscaEquipamentoUsuarioPorComodoeUsuario(string comodo, string usuario)
        {
            Usuario user = new Usuario();
            try
            {
                user = jsonSerializer.Deserialize<Usuario>(usuario);
            }
            catch
            {
                try
                {
                    int IdUsuario = jsonSerializer.Deserialize<int>(usuario);
                    user = user.BuscarUsuarioPorId(IdUsuario);
                }
                catch (Exception e)
                {
                    return "";
                }
            }

            Comodo com = jsonSerializer.Deserialize<Comodo>(comodo);

            EquipamentoUsuario eqp = new EquipamentoUsuario();


            EquipamentoUsuario[] equipamentos = eqp.BuscarEquipaentoPorComodoeUsuario(com.Id, user.Id);
            EqpRetorno preparaEqpRetorno = new EqpRetorno();
            List<EqpRetorno> eqpRetorno = new List<EqpRetorno>();
            foreach (EquipamentoUsuario equip in equipamentos)
            {
                eqpRetorno.Add(preparaEqpRetorno.converteEquipamentos(equip));
            }

            return jsonSerializer.Serialize(eqpRetorno);
        }

    }

    public struct DadosComplementares
    {
        public int idComodo { get; set; }
        public int tempoDeUsoDiario { get; set; }
        public int quantidade { get; set; }
    }

   
}
