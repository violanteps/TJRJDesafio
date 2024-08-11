using AutoMapper;
using Domain.Entity;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssuntoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AutorController> _logger;
        private readonly ILivroService _livroService;

        public AssuntoController(IMapper mapper, ILogger<AutorController> logger, ILivroService livroService)
        {
            _mapper = mapper;
            _logger = logger;
            _livroService = livroService;
        }


        [HttpPost]
        [Route("CreateAssunto")]
        public async Task<IActionResult> CreateAssunto(AssuntoModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<AssuntoEntity>(parameters);
                var ret = await _livroService.CreateAssunto(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar assunto");
                return StatusCode(500, "Erro ao criar assunto");
            }
        }

        [HttpGet]
        [Route("GetAssuntoList")]
        public async Task<IActionResult> GetAssuntoList()
        {
            try
            {
                var ret = await _livroService.GetAssuntoList();

                if (ret == null)
                    return NotFound("Assunto não encontrado.");

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar assunto");
                return StatusCode(500, "Erro ao buscar assunto");
            }
        }

        [HttpGet]
        [Route("GetAssunto")]
        public async Task<IActionResult> GetAssunto([FromQuery] int codAs)
        {
            try
            {
                var parameters = new AssuntoModel { CodAs = codAs };
                var resultMapper = _mapper.Map<AssuntoEntity>(parameters);

                var ret = await _livroService.GetAssunto(resultMapper);

                if (ret == null)
                    return NotFound("Assunto não encontrado.");

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar assunto");
                return StatusCode(500, "Erro ao buscar assunto");
            }
        }

        [HttpPost]
        [Route("UpdateAssunto")]
        public async Task<IActionResult> UpdateAssunto(AssuntoModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<AssuntoEntity>(parameters);
                var ret = await _livroService.UpdateAssunto(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar assunto");
                return StatusCode(500, "Erro ao atualizar assunto");
            }
        }

        [HttpDelete]
        [Route("DeleteAssunto")]
        public async Task<IActionResult> DeleteAssunto([FromQuery] int codAs)
        {
            try
            {
                var parameters = new AssuntoModel { CodAs = codAs };
                var resultMapper = _mapper.Map<AssuntoEntity>(parameters);
                var ret = await _livroService.DeleteAssunto(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir assunto");
                return StatusCode(500, "Erro ao excluir assunto");
            }
        }


    }
}
