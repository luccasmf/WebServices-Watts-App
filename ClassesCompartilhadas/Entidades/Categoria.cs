using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public bool InserirCategoria()
        {
            return Util.Salvar<Categoria>(this);
        }

        public Categoria BuscarCategoriaPorId(int id)
        {
            return Util.BuscaEntidadePorId<Categoria>(id);
        }
        
        public Categoria[] BuscarCategorias()
        {
            return Util.BuscaTodosRegistrosEntidade<Categoria>();
        }

        public object BuscarCategoriaPorComodo(int idComodo)
        {
            return "";
        }
    }
}
