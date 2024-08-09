using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class RelLivrosPorAutorComValorETipoVendaDTO
    {
        public int tipoRel { get; set; }
        
        public string Autor { get; set; }
        
        public string Livro {  get; set; }

        public string Valor { get; set; }

        public string TipoVenda { get; set; }

        public RelLivrosPorAutorComValorETipoVendaDTO() { }
    }
}
