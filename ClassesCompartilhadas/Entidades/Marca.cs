using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    class Marca
    {
        public int Id { get; set; }
        public int Nome { get; set; }

        public bool InserirMarca()
        {
            return Util.Salvar<Marca>(this);
        }

        public Marca BuscarMarcaPorId(int id)
        {
            return Util.BuscaEntidadePorId<Marca>(id);
        }

        public Marca[] BuscarMarcas()
        {
            return Util.BuscaTodosRegistrosEntidade<Marca>();
        }

        public object BuscarMarcaPorCategoria(int idCategoria)
        {
            return true;
        }
    }
}
