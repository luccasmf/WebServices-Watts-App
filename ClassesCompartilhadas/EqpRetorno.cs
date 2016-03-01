using ClassesCompartilhadas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassesCompartilhadas
{
    public class EqpRetorno
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }       
        public decimal? Consumo { get; set; } //null    
        public int TempoConsumo { get; set; }        
        public int Quantidade { get; set; }
        public string Comodo { get; set; }
        public string TipoEquipamento { get; set; }
        public string NomeEquipamento { get; set; }
        // public int IdEquipamento { get; set; }
        //public int IdComodo { get; set; }
        // public decimal? StandBy { get; set; } //null
        // public int TempoStandBy { get; set; }

        public EqpRetorno converteEquipamentos(EquipamentoUsuario eqp)
        {
            Equipamento equipamento = new Equipamento();
            equipamento = equipamento.BuscarEquipamentoPorId(eqp.IdEquipamento);

            Comodo comodo = new Comodo();
            comodo = comodo.BuscarComodoPorId(eqp.IdComodo);

            Categoria categoria = new Categoria();
            categoria = categoria.BuscarCategoriaPorId(equipamento.IdCategoria);

            EqpRetorno equipamentoRetorno = new EqpRetorno
            {
                Id = eqp.Id,
                IdUsuario = eqp.IdUsuario,
                Consumo = eqp.Consumo,
                TempoConsumo = eqp.TempoConsumo,
                Quantidade = eqp.Quantidade,
                Comodo = comodo.Nome,
                TipoEquipamento = categoria.Nome,
                NomeEquipamento = equipamento.Nome
            };
            return equipamentoRetorno;
        }
    }
}
