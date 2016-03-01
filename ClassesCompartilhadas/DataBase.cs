using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClassesCompartilhadas
{
    public class DataBase
    {
        /*
            IP do Servidor - Externo: 189.16.45.2,8000
            IP do Servidor - Interno: 172.16.10.241


            Base de testes:
            Usuário: tst_integrador
            Senha: qwe@123
            Base de dados: bd_tst_integrador



            Base de produção:
            Usuário: prd_integrador
            Senha: 12qw!@QW
            Base de dados: bd_integrador
         */

        //EXTERNO
        private static readonly string CONN_STRING = (@"Data Source=172.16.10.241;Initial Catalog=bd_tst_integrador;User Id=tst_integrador;Password=qwe@123;pooling=false");

        /// <summary>
        /// Método responsável pela execução de operações sem retorno de resultados (INSERT, UPDATE e DELETE) em bancos de dados 
        /// </summary>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado pelo método</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência à uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros da instrução SQL</param>
        /// <returns>Número de registros afetados pela instrução SQL</returns>
        public static int ExecuteNonQuery(CommandType cmdType,
            string cmdText,
            params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(CONN_STRING))
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw (ex);
                }
            }
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(CONN_STRING))
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw (ex);
                }
            }
        }

        /// <summary>
        /// Neste OverLoad do método as operações são executadas na forma de Transaction SQL, ou seja, execução
        /// de mais de uma operação em bancos de dados
        /// </summary>
        /// <param name="trans">Instância do objeto SqlTransaction do .NET Framework</param>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado pelo método</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência à uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros da instrução SQL</param>
        /// <returns>Número de registros afetados pela instrução SQL</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Neste OverLoad do método as operações são executadas na forma de Transaction SQL, ou seja, execução
        /// de mais de uma operação em bancos de dados
        /// </summary>
        /// <param name="trans">Instância do objeto SqlTransaction do .NET Framework</param>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado pelo método</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência à uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros da instrução SQL</param>
        /// <returns>SqlCommand utilizado na execução da instrução T-SQL</returns>
        public static SqlCommand ExecuteNonQueryCmd(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return cmd;
        }

        /// <summary>
        /// Método responsável pela execução de operações com retorno de resultados (instruções SELECT) através do 
        /// objeto SqlDataReader do ADO.NET 
        /// </summary>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado pelo método</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência a uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros da instrução SQL</param>
        /// <returns>SqlDataReader armazenando os registros selecionados</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }
        }

        /// <summary>
        /// Método responsável pela execução de operações com retorno de resultados (instruções SELECT)
        /// através do objeto DataSet do ADO.NET 
        /// </summary>
        /// <param name="cmdText">Comando SQL a ser executado (string SQL convencional)</param>
        /// <returns>DataSet armazenando os registros selecionados</returns>
        public static DataSet ExecuteReaderDs(string cmdText)
        {
            SqlConnection conn = new SqlConnection(CONN_STRING);
            SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);

            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }

            return ds;
        }

        /// <summary>
        /// Através deste OverLoad do método pode-se preencher um DataSet (objeto do ADO.NET) através do processamento
        /// de instruções SQL convencionais ou procedimentos armazenados (stored procedures) existentes no SGBDR.
        /// </summary>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado pelo método</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência a uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros da instrução SQL</param>
        /// <returns>DataSet armazenando os registros selecionados</returns>
        public static DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);
            DataSet ds = new DataSet();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }

            return ds;
        }

        /// <summary>
        /// Método responsável pela execução de operações com retorno de resultados (instruções SELECT)
        /// através do objeto DataTabe do .NET Framework
        /// </summary>
        /// <param name="cmdText">Comando SQL a ser executado (string SQL convencional)</param>
        /// <returns>DataTable armazenando os registros selecionados</returns>
        public static DataTable ExecuteReaderDt(string cmdText)
        {
            SqlConnection conn = new SqlConnection(CONN_STRING);
            SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);

            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }

            return dt;
        }

        /// <summary>
        /// Através deste OverLoad do método pode-se preencher um DataTable (objeto do .NET Framework) através do processamento
        /// de instruções SQL convencionais ou procedimentos armazenados (stored procedures) existentes no SGBD.
        /// </summary>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado pelo método</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência a uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros da instrução SQL</param>
        /// <returns>DataTable armazenando os registros selecionados</returns>
        public static DataTable ExecuteReaderDt(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);
            DataTable dt = new DataTable();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }
            return dt;
        }

        /// <summary>
        /// Método responsável pela manipulação de instâncias do objeto SqlCommand do ADO.NET
        /// para a execução de operações em bancos de dados
        /// </summary>
        /// <param name="cmd">Instância do objeto SqlCommand a ser ajustado</param>
        /// <param name="conn">Instância do objeto SqlConnection da conexão adotada</param>
        /// <param name="trans">Instância do objeto SqlTransaction do .NET Framework</param>
        /// <param name="cmdType">Tipo de comando (CommandType.StoredProcedure, CommandType.TableDirect, CommandType.Text) a ser
        /// executado</param>
        /// <param name="cmdText">Comando SQL a ser executado, podendo ser uma string SQL convencional ou a referência a uma
        /// stored procedure</param>
        /// <param name="cmdParms">Array de parâmetros SQL contendo eventuais parâmetros de instruções SQL</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}