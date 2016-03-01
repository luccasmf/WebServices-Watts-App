using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class CompanhiaEletrica
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataInicialVigencia { get; set; }
        public DateTime DataFinalVigencia { get; set; }



        public CompanhiaEletrica()
        {

        }

        public List<CompanhiaEletrica> BuscarCompanhiaPorUF(string uf)
        {
            List<CompanhiaEletrica> listaCompanhiaEletrica = new List<CompanhiaEletrica>();
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format(@"SELECT C.Id,
                                                  C.Nome,
                                                  C.Sigla  
                                             FROM CompanhiaEletrica C
                                             LEFT JOIN Estado E ON E.Id = C.IdEstado
                                            WHERE E.Sigla = @UF");
            parametros.Add(new SqlParameter("UF", uf));


            SqlDataReader dt = DataBase.ExecuteReader(System.Data.CommandType.Text, query, parametros.ToArray());
            while (dt.Read())
            {
                CompanhiaEletrica companhia = new CompanhiaEletrica();
                companhia.Id = int.Parse(dt["Id"].ToString());
                companhia.Nome = dt["Nome"].ToString();
                companhia.Sigla = dt["Sigla"].ToString();
                listaCompanhiaEletrica.Add(companhia);
            }


            return listaCompanhiaEletrica;

        }
    }
}
