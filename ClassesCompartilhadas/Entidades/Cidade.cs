using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class Cidade
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public string Nome { get; set; }

        public bool InserirCidade ()
        {
            return Util.Salvar<Cidade>(this);
        }

        public Cidade BuscarCidadePorId(int id)
        {
            return Util.BuscaEntidadePorId<Cidade>(id);
        }

        public Cidade[] BuscarCidades()
        {
            return Util.BuscaTodosRegistrosEntidade<Cidade>();
        }

        public Cidade[] BuscarCidadePorEstado(int idEstado)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format("SELECT * FROM Cidade WHERE IdEstado = @IdEstado");
            parametros.Add(new SqlParameter("IdEstado", idEstado));
            return Util.BuscaEntidadeCustomizada<Cidade>(query, parametros.ToArray()); 
        }

        public Cidade[] BuscarCidadePorEstado(string UF)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = @"select c.* from Cidade c
                            left join Estado e
                            on e.Id = c.IdEstado
                            where e.Sigla = @Sigla";

            parametros.Add(new SqlParameter("Sigla", UF));
           // parametros.Add(new SqlParameter("IdComodo", idComodo));

            return Util.BuscaEntidadeCustomizada<Cidade>(query, parametros.ToArray());
        }
    }
}
