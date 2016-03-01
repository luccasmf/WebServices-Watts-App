using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }

        public bool InserirEstado()
        {
            return Util.Salvar<Estado>(this);
        }

        public Estado BuscarEstadoPorId(int id)
        {
            return Util.BuscaEntidadePorId<Estado>(id);
        }

        public Estado[] BuscarEstados()
        {
            return Util.BuscaTodosRegistrosEntidade<Estado>();
        }

        public Estado BuscarEstadoPorSigla(string sigla)
        {
            return this;
        }

        public Estado BuscarEstadoPorCidade(int idCidade)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = @"select e.* from Estado e
                                left join Cidade c
                                on c.IdEstado = e.Id
                                where c.Id = @Id";
            parametros.Add(new SqlParameter("Id", idCidade));
            return Util.BuscaEntidadeCustomizada<Estado>(query, parametros.ToArray())[0];
        }

    }
}
