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
    /// Summary description for Usuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UsuarioWebService : Classes.WebServiceComum
    {
        [WebMethod]
        public string Salvar(string entidade)
        {
            Usuario usuario = jsonSerializer.Deserialize<Usuario>(entidade);

            //bool salvou = ClassesCompartilhadas.Util.Salvar(entidade);
            bool salvou = usuario.InserirUsuario();
            return jsonSerializer.Serialize(salvou);
        }

        [WebMethod]
        public string Deletar(int Id)
        {
            return string.Empty;
        }

        /// <summary>
        /// Busca de todos os usuarios cadastrados no banco
        /// </summary>
        /// <returns>Retorna um json com um array de Usuarios</returns>
        [WebMethod]
        public string BuscarUsuarios()
        {
            Usuario usuario = new Usuario();
            Usuario[] usuarios = usuario.BuscarUsuarios();
            return jsonSerializer.Serialize(usuarios);
        }

        /// <summary>
        /// Método para login que verifica email e senha do usuario
        /// </summary>
        /// <param name="email">email do usuario obtido pelo campo do login</param>
        /// <param name="senha">senha do usuario obtida pelo campo do login</param>
        /// <returns>retorna json com uma bool q indica sucesso ou falha + mensagem</returns>
        [WebMethod]
        public string Login(string email, string senha)
        {
           
            Usuario user = new Usuario();
            try
            {
                user = BuscaUsuarioPorEmail(email);

                if (user == null)
                {
                    return jsonSerializer.Serialize(new Usuario());
                }
                else
                {
                    if (user.Senha == senha)
                    {
                        return jsonSerializer.Serialize(user);
                    }
                    else
                    {
                        return jsonSerializer.Serialize(new Usuario());

                    }

                }
            }
            catch
            {
                return jsonSerializer.Serialize(new Usuario());
            }
            
        }

        public Estado PegaEstadoPorId(string user)
        {
            Usuario usuario = jsonSerializer.Deserialize<Usuario>(user);

            Estado e = new Estado();
            return e.BuscarEstadoPorCidade(usuario.IdCidade);
            
        }
        
        public Usuario BuscaUsuarioPorEmail(string email) //nao eh webMethod
        {
            Usuario usuario = new Usuario();

            usuario = usuario.BuscarUsuarioPorEmail(email);

            return usuario;
        }

        [WebMethod]
        public string BuscarUsuarioPorId(int id)
        {
            Usuario user = new Usuario();
            user = user.BuscarUsuarioPorId(id);

            return jsonSerializer.Serialize(user);
        }

        /// <summary>
        /// Envia a senha do usuario para o seu email
        /// </summary>
        /// <param name="email">email do usuario</param>
        /// <returns>json: true se o email foi enviado false caso contrário</returns>
        [WebMethod]
        public string EsqueceuSenha(string email)
        {
            Usuario usuario = new Usuario();

            usuario = usuario.BuscarUsuarioPorEmail(email);

            bool enviouEmail = false;
            if (usuario != null && usuario.Id > 0)
                enviouEmail = ClassesCompartilhadas.Util.EnviarEmail(usuario.Email, "Sua senha do PI que você esqueceu", String.Format("Sua senha é: {0}", usuario.Senha), false);

            return jsonSerializer.Serialize(enviouEmail);
        }

    }

    public struct Retorno
    {
        public Usuario user { get; set; }
        public Estado estado { get; set; }
    }
}
