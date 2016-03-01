using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesCompartilhadas.Entidades
{
    class MarcaCategoria
    {
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }

        public bool InserirMarcaCategoria()
        {
            return Util.Salvar<MarcaCategoria>(this);
        }

        public object BuscarMarca(int idCategoria)
        {
            return true;
        }

        public object BuscarCategoria(int idMarca)
        {
            return true;
        }
    }
}
