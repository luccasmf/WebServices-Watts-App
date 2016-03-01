using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class Equipamento
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public int IdMarca = 1;
        public string Nome { get; set; }
        public decimal Consumo { get; set; }
        public decimal StandBy { get; set; }
        public string Imagem { get; set; }
       // public string Tipo { get; set; }

        public bool InserirEquipamento()
        {
          return Util.Salvar<Equipamento>(this);
        }

        public bool DeletarEquipamento(int id)
        {
            return Util.DeletarEntidadePorId<Equipamento>(id);
        }

        public Equipamento BuscarEquipamentoPorId(int id)
        {
            return Util.BuscaEntidadePorId<Equipamento>(id);
        }

        public Equipamento[] BuscarEquipamentos()
        {
            return Util.BuscaTodosRegistrosEntidade<Equipamento>();
        }

        public Equipamento[] BuscarEquipamentoPorMarca(int idMarca) //implementar busca certa
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
           
            string query = string.Format("SELECT * FROM Equipamento WHERE IdMarca = @IdMarca");
            parametros.Add(new SqlParameter("IdMarca", idMarca));
            return Util.BuscaEntidadeCustomizada<Equipamento>(query, parametros.ToArray()); 
        }

        public Equipamento[] BuscarEquipamentoPorCategoria(int idCategoria) //implementar busca certa
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format("SELECT * FROM Equipamento WHERE IdCategoria = @IdCategoria");
            parametros.Add(new SqlParameter("IdCategoria", idCategoria));
            return Util.BuscaEntidadeCustomizada<Equipamento>(query, parametros.ToArray()); 
        }

        public Equipamento[] BuscarEquipamentoPorCategoriaeComodo(int idCategoria, int idComodo)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = @"select e.* 
                            from Equipamento e
                            left join Categoria c
                            on c.Id = e.IdCategoria
                            left join ComodoCategoria cc
                            on cc.IdCategoria = c.Id
                            where 
                            cc.IdCategoria = @IdCategoria and
                            cc.IdComodo = @IdComodo";

            parametros.Add(new SqlParameter("IdCategoria", idCategoria));
            parametros.Add(new SqlParameter("IdComodo", idComodo));

            return Util.BuscaEntidadeCustomizada<Equipamento>(query, parametros.ToArray());


        }

        public Equipamento[] BuscarEquipamentoPorComodo(int idComodo)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = @"select e.* 
                            from Equipamento e
                            left join Categoria c
                            on c.Id = e.IdCategoria
                            left join ComodoCategoria cc
                            on cc.IdCategoria = c.Id
                            where 
                            cc.IdComodo = @IdComodo";

            parametros.Add(new SqlParameter("IdComodo", idComodo));

            return Util.BuscaEntidadeCustomizada<Equipamento>(query, parametros.ToArray());
        }
    }
}
