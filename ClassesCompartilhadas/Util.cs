using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ClassesCompartilhadas
{
    public class Util
    {
        public static bool Salvar<T>(T entidade)
        {
            string nomeTabela = entidade.GetType().Name;
            var propriedades = entidade.GetType().GetProperties();
            List<SqlParameter> parametros = new List<SqlParameter>();
            string nomesParametros = string.Empty;
            string nomesValoresParametros = string.Empty;
            string nomesParametrosAtualizar = string.Empty;
            int IdEntidade = -1;
            bool inserir = true;

            foreach (var propriedade in propriedades)
            {
                string nomeProp = propriedade.Name;
                var valor = propriedade.GetValue(entidade, null);
                string nomeParametro = "";
                
                if (nomeProp == "Id" && ((int)valor) == 0)
                {
                    inserir = true;
                    continue;
                }
                else
                {
                    nomeParametro = String.Format("@{0}", nomeProp);
                    
                    nomesParametros += String.Format("{0}, ", nomeProp);
                    nomesValoresParametros += String.Format("{0}, ", nomeParametro);
                    if (nomeProp == "Id")
                    {
                        IdEntidade = (int)valor;
                        inserir = false;
                        nomesParametrosAtualizar += String.Format("{0} = {1}, ", nomeProp, nomeParametro);
                    }
                }
                parametros.Add(new SqlParameter(
                    nomeParametro,
                    valor
                    )
                );
            }

            nomesParametros = nomesParametros.Substring(0, nomesParametros.Length - 2);
            nomesValoresParametros = nomesValoresParametros.Substring(0, nomesValoresParametros.Length - 2);

            string comando = string.Empty;

            if (inserir)
            {
                comando = String.Format("INSERT INTO {0} ({1}) VALUES ({2});", nomeTabela, nomesParametros, nomesValoresParametros);                
            }
            else
            {
                nomesParametrosAtualizar = nomesParametrosAtualizar.Substring(0, nomesParametrosAtualizar.Length - 2);
                comando = String.Format("UPDATE {0} SET {1} WHERE Id = {2};", nomeTabela, nomesParametrosAtualizar, IdEntidade);                
            }

            try
            {
                int ret = DataBase.ExecuteNonQuery(CommandType.Text, comando, parametros.ToArray());

                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static T BuscaEntidadePorId<T>(int Id)
        {
            string nomeTabela = typeof(T).Name;
            string query = String.Format("SELECT * FROM {0} WHERE Id = @Id", nomeTabela);

            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, query, new SqlParameter("@Id", Id));

            if (reader.HasRows)
                return MapearResultadoTabelaParaObjeto<T>(reader)[0];
            else
                return default(T);
        }

        public static T[] BuscaEntidadeCustomizada<T>(string busca, SqlParameter[] parametros) //Luccas
        {
            string query = busca;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, query, parametros);


            if (reader.HasRows)
                return MapearResultadoTabelaParaObjeto<T>(reader);
            else
                return default(T[]);
        }

        public static bool InsertEntidadeCustomizado(string comando, SqlParameter[] parametros) //Luccas
        {
            return (DataBase.ExecuteNonQuery(CommandType.Text, comando, parametros.ToArray()) > 0);
        }

        public static T[] BuscaTodosRegistrosEntidade<T>()
        {
            string nomeTabela = typeof(T).Name;
            string query = String.Format("SELECT * FROM {0}",nomeTabela);

            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, query);

            if (reader.HasRows)
                return MapearResultadoTabelaParaObjeto<T>(reader);
            else
                return default(T[]);
        }

        public static bool DeletarEntidadePorId<T>(int Id)
        {
            string nomeTabela = typeof(T).Name;
            string query = String.Format("DELETE FROM {0} WHERE Id = @Id", nomeTabela);


            int affected = DataBase.ExecuteNonQuery(CommandType.Text, query, new SqlParameter("@Id", Id));

            return affected > 0;
        }

        public static T[] MapearResultadoTabelaParaObjeto<T>(SqlDataReader entidade)
        {
            List<T> listaRetorno = new List<T>();

            while (entidade.Read())
            {
                T entidadeObj = Activator.CreateInstance<T>();
                var entidadePropriedades = entidadeObj.GetType().GetProperties();

                foreach (var item in entidadePropriedades)
                {
                    string nomeProp = item.Name;
                    Type tipoProp = item.PropertyType;
                    var valor = entidade[nomeProp];

                    //passa valor para o objeto
                  item.SetValue(entidadeObj, valor, null);
                      /*if(tipoProp.Equals(typeof(int))) 
                    {
                    }*/

                    
                }
               // ent
                listaRetorno.Add(entidadeObj);
            }

            return listaRetorno.ToArray();
        }

        public static bool EnviarEmail(string email, string assunto, string conteudo, bool ehConteudoHtml)
        {
            try
            {
                System.Net.Mail.MailMessage emailMessage = new System.Net.Mail.MailMessage();

                string emailRoboPi = "facecla.pi.robo.2015@gmail.com";
                string senhaRoboPi = "fcl-2015-pi-vaiqvai*@sistemas";

                System.Net.Mail.MailAddress senderEmail = new System.Net.Mail.MailAddress(emailRoboPi, "Projeto Integrador Facecla 2015");
                System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential(emailRoboPi, senhaRoboPi);
                System.Net.Mail.SmtpClient clienteSMTP = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                emailMessage.From = senderEmail;
                emailMessage.To.Add(email);

                emailMessage.Subject = assunto;
                emailMessage.IsBodyHtml = ehConteudoHtml;
                emailMessage.Body = conteudo;
                                
                //Unable SSL
                clienteSMTP.EnableSsl = true;
                clienteSMTP.Credentials = mailAuthentication;
                clienteSMTP.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                //Send Email
                clienteSMTP.Send(emailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
