using AutoMapper;
using Domain.Entity;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AutorController> _logger;
        private readonly ILivroService _livroService;

        public AutorController(IMapper mapper, ILogger<AutorController> logger, ILivroService livroService)
        {
            _mapper = mapper;
            _logger = logger;
            _livroService = livroService;
        }

  
        [HttpPost]
        [Route("CreateAutor")]
        public async Task<IActionResult> CreateAutor(AutorModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<AutorEntity>(parameters);
                var ret = await _livroService.CreateAutor(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar autor");
                return StatusCode(500, "Erro ao criar autor");
            }
        }

        [HttpGet]
        [Route("GetAutor")]
        public async Task<IActionResult> GetAutor([FromQuery] int codAu)
        {
            try
            {
                var parameters = new AutorModel { CodAu = codAu };
                var resultMapper = _mapper.Map<AutorEntity>(parameters);

                var ret = await _livroService.GetAutor(resultMapper);

                if (ret == null)
                    return NotFound("Autor não encontrado.");

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar autor");
                return StatusCode(500, "Erro ao buscar autor");
            }
        }

        [HttpGet]
        [Route("GetAutorList")]
        public async Task<IActionResult> GetAutorList()
        {
            try
            {
                var ret = await _livroService.GetAutorList();

                if (ret == null)
                    return NotFound("Autor não encontrado.");

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar autor");
                return StatusCode(500, "Erro ao buscar autor");
            }
        }

        [HttpPost]
        [Route("UpdateAutor")]
        public async Task<IActionResult> UpdateAutor(AutorModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<AutorEntity>(parameters);
                var ret = await _livroService.UpdateAutor(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar autor");
                return StatusCode(500, "Erro ao atualizar autor");
            }
        }

        [HttpDelete]
        [Route("DeleteAutor")]
        public async Task<IActionResult> DeleteAutor([FromQuery] int codAu)
        {
            try
            {
                var parameters = new AutorModel { CodAu = codAu };
                var resultMapper = _mapper.Map<AutorEntity>(parameters);
                var ret = await _livroService.DeleteAutor(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir autor");
                return StatusCode(500, "Erro ao excluir autor");
            }
        }

    }
}
