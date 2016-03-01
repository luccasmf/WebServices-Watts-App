using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassesCompartilhadas.Entidades
{
    public class RankEquipamentoUsuario
    {
        public String NomeEquipamento { get; set; }
        public decimal kwhConsumoEquipamento { get; set; }
        public decimal kwhStandByEquipamento { get; set; }
        public int TempoConsumo { get; set; }
	    public int TempoStandBy { get; set; }
	    public decimal KwhConsumo { get; set; }
        public decimal KwhStandyBy { get; set; }
	    public decimal KwhTotal { get; set; }
	    public decimal CustoEnergia { get; set; }
        public decimal CustoConsumo { get; set; }

  

    }
}
