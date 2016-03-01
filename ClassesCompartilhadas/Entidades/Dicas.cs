using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ClassesCompartilhadas.Entidades
{
    public class Dicas
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public Dicas BuscarDicasPorId(int id)
        {
            return Util.BuscaEntidadePorId<Dicas>(id);
        }

        public int QuantidadeDeDicas()
        {
            string query = (@"select top 1 * from Dicas order by ID desc");

            Dicas d = Util.BuscaEntidadeCustomizada<Dicas>(query, new SqlParameter[] { })[0];

            return d.Id;
        }
    }
}
