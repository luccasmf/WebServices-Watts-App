using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdCidade { get; set; }
        public int IdCompanhiaEletrica { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public bool InserirUsuario()
        {
            return Util.Salvar<Usuario>(this);
        }

        public bool AtualizarUsuario()
        {
            return Util.Salvar<Usuario>(this);
        }

        public object BuscarUsuarioPorNome(string nome)
        {
            return true;
        }

        public Usuario[] BuscarUsuarios()
        {
            return Util.BuscaTodosRegistrosEntidade<Usuario>();
        }

        public Usuario BuscarUsuarioPorId(int id)
        {
            return Util.BuscaEntidadePorId<Usuario>(id);
        }

        public Usuario BuscarUsuarioPorEmail(string email)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            string query = string.Format("SELECT * FROM USUARIO WHERE Email = @Email");
            parametros.Add(new SqlParameter("Email", email));
            return Util.BuscaEntidadeCustomizada<Usuario>(query, parametros.ToArray())[0]; 
        }

        public object BuscarUsuarioPorCidade(int idCidade)
        {
            return true;
        }

    }
}
