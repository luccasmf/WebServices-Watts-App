using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    public class Comodo
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public bool InserirComodo()
        {
            return Util.Salvar<Comodo>(this);
        }
        
        public Comodo BuscarComodoPorId(int id)
        {
            return Util.BuscaEntidadePorId<Comodo>(id);
        }

        public Comodo[] BuscarComodos()
        {
            return Util.BuscaTodosRegistrosEntidade<Comodo>();             
        }
    }
}
