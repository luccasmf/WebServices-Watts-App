using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ClassesCompartilhadas.Entidades;
using ClassesCompartilhadas;
using System.Data.SqlClient;


namespace WebService
{
    /// <summary>
    /// Summary description for RelatorioWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RelatorioWebService : Classes.WebServiceComum
    {

        [WebMethod]
        public string RankEquipamentoConsumoUsuario(int idUsuario)
        {
            List<RankEquipamentoUsuario> listaEquipamentoConsumo = new List<RankEquipamentoUsuario>();
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format(@" SELECT TOP 10
                                                   E.Nome AS NOMEEQUIPAMENTO,
                                                   EU.Consumo/1000 AS KWHCONSUMOEQUIPAMENTO,
                                                   EU.StandBy/1000 AS KWHSTANDBYEQUIPAMENTO,
                                                   EU.TempoConsumo,
                                                   EU.TempoStandBy ,
                                                   ((CAST(EU.TempoConsumo AS DECIMAL)/60) * (EU.Consumo/1000)) AS KWHCONSUMO,
                                                   ((CAST(EU.TempoStandBy AS DECIMAL)/60) * (EU.StandBy/1000)) AS KWHSTANDBY,
                                                   ((CAST(EU.TempoConsumo AS DECIMAL)/60) * (EU.Consumo/1000)) + ((CAST(EU.TempoStandBy AS DECIMAL)/60)*(EU.StandBy/1000)) AS KWHTOTAL,
                                                   (CE.Valor/(((100-(29+ES.ICMS))/100))) AS CUSTOENERGIA,
                                                   ((CAST(EU.TempoConsumo AS DECIMAL)/60) * (EU.Consumo/1000)) + ((CAST(EU.TempoStandBy AS DECIMAL)/60)*(EU.StandBy/1000)) * (CE.Valor/(((100-(29+ES.ICMS))/100))) AS CUSTOCONSUMO
                                              FROM EQUIPAMENTOUSUARIO EU
                                              LEFT JOIN Usuario U ON U.Id = EU.IdUsuario
                                              LEFT JOIN EQUIPAMENTO E ON E.ID = EU.IdEquipamento 
                                              LEFT JOIN CompanhiaEletrica CE ON CE.Id = U.IdCompanhiaEletrica
                                              LEFT JOIN Estado ES ON ES.ID = CE.IdEstado
                                             WHERE EU.IdUsuario = @idUsuario
                                             ORDER BY 10 DESC");
            parametros.Add(new SqlParameter("idUsuario", idUsuario));


            SqlDataReader dt = DataBase.ExecuteReader(System.Data.CommandType.Text, query, parametros.ToArray());
            while (dt.Read())
            {
                RankEquipamentoUsuario item = new RankEquipamentoUsuario();
                item.NomeEquipamento = dt["NomeEquipamento"].ToString();
                item.kwhConsumoEquipamento = decimal.Parse(dt["kwhConsumoEquipamento"].ToString());
                item.kwhStandByEquipamento = decimal.Parse(dt["kwhStandByEquipamento"].ToString());
                item.TempoConsumo = int.Parse(dt["TempoConsumo"].ToString());
                item.TempoStandBy = int.Parse(dt["TempoStandBy"].ToString());
                item.KwhConsumo = decimal.Parse(dt["KwhConsumo"].ToString());
                item.KwhStandyBy = decimal.Parse(dt["KwhStandBy"].ToString());
                item.KwhTotal = decimal.Parse(dt["KwhTotal"].ToString());
                item.CustoEnergia = decimal.Parse(dt["CustoEnergia"].ToString());
                item.CustoConsumo = decimal.Parse(dt["CustoConsumo"].ToString());
                listaEquipamentoConsumo.Add(item);
            }


            return jsonSerializer.Serialize(listaEquipamentoConsumo);


        }

        [WebMethod]
        public string RankEquipamentoConsumoUsuarioPeriodo(int idUsuario, int dias)
        {
            List<RankEquipamentoUsuario> listaEquipamentoConsumo = new List<RankEquipamentoUsuario>();
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format(@" SELECT E.Nome AS NOMEEQUIPAMENTO,
                                                   COALESCE(EU.Consumo/1000,0) AS KWHCONSUMOEQUIPAMENTO,
                                                   COALESCE(EU.StandBy/1000,0) AS KWHSTANDBYEQUIPAMENTO,
                                                   EU.TempoConsumo,
                                                   EU.TempoStandBy ,
                                                   COALESCE(((CAST(EU.TempoConsumo AS DECIMAL)/60) * (EU.Consumo/1000)),0) AS KWHCONSUMO,
                                                   COALESCE(((CAST(EU.TempoStandBy AS DECIMAL)/60) * (EU.StandBy/1000)),0) AS KWHSTANDBY,
                                                   COALESCE(((CAST(EU.TempoConsumo AS DECIMAL)/60) * (EU.Consumo/1000)) + ((CAST(EU.TempoStandBy AS DECIMAL)/60)*(EU.StandBy/1000)),0) AS KWHTOTAL,
                                                   (CE.Valor/(((100-(29+ES.ICMS))/100))) AS CUSTOENERGIA,
                                                   COALESCE(((CAST(EU.TempoConsumo AS DECIMAL)/60) * (EU.Consumo/1000)) + ((CAST(EU.TempoStandBy AS DECIMAL)/60)*(EU.StandBy/1000)) * (CE.Valor/(((100-(29+ES.ICMS))/100))),0) AS CUSTOCONSUMO
                                              FROM EQUIPAMENTOUSUARIO EU
                                              LEFT JOIN Usuario U ON U.Id = EU.IdUsuario
                                              LEFT JOIN EQUIPAMENTO E ON E.ID = EU.IdEquipamento 
                                              LEFT JOIN CompanhiaEletrica CE ON CE.Id = U.IdCompanhiaEletrica
                                              LEFT JOIN Estado ES ON ES.ID = CE.IdEstado
                                             WHERE EU.IdUsuario = @idUsuario
                                             ORDER BY 10 DESC");
            parametros.Add(new SqlParameter("idUsuario", idUsuario));


            SqlDataReader dt = DataBase.ExecuteReader(System.Data.CommandType.Text, query, parametros.ToArray());
            while (dt.Read())
            {
                RankEquipamentoUsuario item = new RankEquipamentoUsuario();
                item.NomeEquipamento = dt["NomeEquipamento"].ToString();
                item.kwhConsumoEquipamento = decimal.Parse(dt["kwhConsumoEquipamento"].ToString()) * dias;
                item.kwhStandByEquipamento = decimal.Parse(dt["kwhStandByEquipamento"].ToString()) * dias;
                item.TempoConsumo = int.Parse(dt["TempoConsumo"].ToString()) * dias;
                item.TempoStandBy = int.Parse(dt["TempoStandBy"].ToString()) * dias;
                item.KwhConsumo = decimal.Parse(dt["KwhConsumo"].ToString()) * dias;
                item.KwhStandyBy = decimal.Parse(dt["KwhStandBy"].ToString()) * dias;
                item.KwhTotal = decimal.Parse(dt["KwhTotal"].ToString()) * dias;
                item.CustoEnergia = decimal.Parse(dt["CustoEnergia"].ToString());
                item.CustoConsumo = decimal.Parse(dt["CustoConsumo"].ToString()) * dias;
                listaEquipamentoConsumo.Add(item);
            }


            return jsonSerializer.Serialize(listaEquipamentoConsumo);


        }



    }



}
