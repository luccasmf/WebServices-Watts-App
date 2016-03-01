using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class EquipamentoUsuario
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdEquipamento { get; set; }
        public int IdComodo {get;set;}
        public decimal? Consumo { get; set; } //null
        public decimal? StandBy { get; set; } //null
        public int TempoConsumo { get; set; }
        public int TempoStandBy { get; set; }
        public int Quantidade { get; set; }

        public bool InserirEquipamentoUsuario()
        {
            return Util.Salvar<EquipamentoUsuario>(this);
        }

        public bool InserirLote(string comando, SqlParameter[] parametros)
        {
            return Util.InsertEntidadeCustomizado(comando, parametros);
        }

        public EquipamentoUsuario[] BuscarEquipaentoPorUsuario(int idUsuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format("SELECT * FROM EquipamentoUsuario WHERE IdUsuario = @IdUsuario");
            parametros.Add(new SqlParameter("IdUsuario", idUsuario));
            return Util.BuscaEntidadeCustomizada<EquipamentoUsuario>(query, parametros.ToArray()); 
        }

        public EquipamentoUsuario[] BuscarEquipaentoPorComodoeUsuario(int idComodo, int idUsuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format("SELECT * FROM EquipamentoUsuario WHERE IdComodo = @IdComodo and IdUsuario = @IdUsuario");
            parametros.Add(new SqlParameter("IdComodo", idComodo));
            parametros.Add(new SqlParameter("IdUsuario", idUsuario));
            return Util.BuscaEntidadeCustomizada<EquipamentoUsuario>(query, parametros.ToArray()); 
        }

    }
}
