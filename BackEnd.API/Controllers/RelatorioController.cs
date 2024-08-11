using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RelatorioController> _logger;
        private readonly ILivroService _livroService;

        public RelatorioController(IMapper mapper, ILogger<RelatorioController> logger, ILivroService livroService)
        {
            _mapper = mapper;
            _logger = logger;
            _livroService = livroService;
        }

        [HttpGet]
        [Route("RelatorioEstoque/{tipoRelatorio}")]
        public async Task<IActionResult> GetRelatorioEstoque(int tipoRelatorio)
        {
            try
            {
                IEnumerable<object> relatorio;
                if (tipoRelatorio == 1)
                {
                    relatorio = await _livroService.GerarRelatorioPorAutorComAssunto(tipoRelatorio);
                }
                else if (tipoRelatorio == 2)
                {
                    relatorio = await _livroService.GerarRelatorioPorAutorComValor(tipoRelatorio);
                }
                else
                {
                    return BadRequest("Tipo de relat�rio inv�lido.");
                }

                
                if (relatorio == null || !relatorio.Any())
                {
                    return NotFound("Relat�rio n�o encontrado.");
                }

                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar o relat�rio.");
                return StatusCode(500, "Erro ao gerar o relat�rio.");
            }
        }

        [HttpGet]
        [Route("GetRelatoriosList")]
        public IActionResult GetRelatoriosList()
        {
            try
            {
                var relatorios = new List<object>
                {
                    new { tipoRel = 1, assunto = "Relat�rio por Autor e Assunto" },
                    new { tipoRel = 2, assunto = "Relat�rio por Autor e Valor" },
                    new { tipoRel = 3, assunto = "Relat�rio por Assunto e Valor" }
                };

                return Ok(relatorios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de relat�rios.");
                return StatusCode(500, "Erro ao obter a lista de relat�rios.");
            }
        }
        }

}
