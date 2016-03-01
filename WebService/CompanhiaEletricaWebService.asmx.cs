using ClassesCompartilhadas.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using ClassesCompartilhadas.Entidades;
using ClassesCompartilhadas;

namespace WebService
{
    /// <summary>
    /// Summary description for CompanhiaEletricaWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CompanhiaEletricaWebService : Classes.WebServiceComum
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string Salvar(Equipamento entidade)
        {
            bool salvou = ClassesCompartilhadas.Util.Salvar(entidade);

            return jsonSerializer.Serialize(salvou);
        }

        [WebMethod]
        public string BuscarCompanhiaPorUf(string uf)
        {
            CompanhiaEletrica companhia = new CompanhiaEletrica();
            
            return jsonSerializer.Serialize(companhia.BuscarCompanhiaPorUF(uf));
        }

        [WebMethod]
        public string AtualizarCompanhiaEletrica()
        {
            try
            {


                CompanhiaEletrica[] companhiasEletricas = Util.BuscaTodosRegistrosEntidade<CompanhiaEletrica>();
                string codigoHTML = GetHtml();
                int inicioTabela = codigoHTML.IndexOf("http://www.aneel.gov.br/aplicacoes/tarifaAplicada/ordUp.gif");
                int tamanhoTabela = (codigoHTML.IndexOf("grafico") - inicioTabela);

                codigoHTML = codigoHTML.Substring(inicioTabela, tamanhoTabela);

                codigoHTML = codigoHTML.Replace("&nbsp;", "");
                string[] stringSeparators = new string[] { "<tr>" };
                string[] stringPreExtraida = codigoHTML.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 1; i < stringPreExtraida.Length - 1; i++)
                {
                    string temp = stringPreExtraida[i];
                    temp = temp.Replace("\r\n\t\t\t<td width=\"5%\">", ";");
                    temp = temp.Replace("</td>\r\n\t\t\t<td width=\"45%\">\r\n\t\t\t", ";");
                    temp = temp.Replace("\r\n\t\t\t</td>\r\n\t\t\t<td align=\"center\" width=\"30%\">\r\n\r\n\r\n\t\t\t", ";");
                    temp = temp.Replace("\r\n\t\t\t\r\n\r\n\t\t\t</td>\r\n\t\t\t<td align=\"center\" width=\"15%\">\r\n\t\t\t\t", ";");
                    temp = temp.Replace(" <br> <b>at&eacute;</b><br>", ";");
                    temp = temp.Replace("\r\n\t\t\t </td>\r\n\t\t</tr>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\t\t", ";");
                    temp = temp.Replace("\r\n\t\t\t </td>\r\n\t\t</tr>\r\n\r\n\r\n\r\n\r\n\r\n", ";");

                    string[] stringExtraida = temp.Split(';');
                    CompanhiaEletrica companhia = new CompanhiaEletrica();
                    bool bEncontrou = false;

                    foreach (var item in companhiasEletricas)
                    {
                        if (item.Sigla.Equals(stringExtraida[1]))
                        {
                            bEncontrou = true;
                            companhia = item;
                            break;
                        }
                    }

                    if (bEncontrou)
                    {
                        companhia.Valor = decimal.Parse(stringExtraida[3]);
                        companhia.DataInicialVigencia = DateTime.Parse(stringExtraida[4]);
                        companhia.DataFinalVigencia = DateTime.Parse(stringExtraida[5]);
                    }
                    else
                    {
                        companhia.Sigla = stringExtraida[1];
                        companhia.Nome = stringExtraida[2];
                        companhia.IdEstado = 1;//
                        companhia.Valor = decimal.Parse(stringExtraida[3]);
                        companhia.DataInicialVigencia = DateTime.Parse(stringExtraida[4]);
                        companhia.DataFinalVigencia = DateTime.Parse(stringExtraida[5]);
                    }


                }


                foreach (var item in companhiasEletricas)
                {
                    Util.Salvar(item);
                }
                return "ok";
            }
            catch (Exception)
            {
                return "Erro";

            }


        }




        public string GetHtml()
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://www.aneel.gov.br/area.cfm?idArea=493");
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.GetEncoding(1252));
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

    }
}
