using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AutorController> _logger;
        private readonly ILivroService _livroService;

        public LivroController(IMapper mapper, ILogger<AutorController> logger, ILivroService livroService)
        {
            _mapper = mapper;
            _logger = logger;
            _livroService = livroService;
        }

        [HttpPost]
        [Route("CreateLivro")]
        public async Task<IActionResult> CreateLivro(LivroModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<LivroEntity>(parameters);
                var ret = await _livroService.CreateLivro(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar livro");
                return StatusCode(500, "Erro ao criar livro");
            }
        }

        [HttpGet]
        [Route("GetLivro")]
        public async Task<IActionResult> GetLivro([FromQuery] int codl)
        {
            try
            {
                var parameters = new LivroModel { Codl = codl };
                var resultMapper = _mapper.Map<LivroEntity>(parameters);

                var ret = await _livroService.GetLivro(resultMapper);

                if (ret == null)
                {
                    return NotFound("Livro não encontrado.");
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar livro");
                return StatusCode(500, "Erro ao buscar livro");
            }
        }

        [HttpGet]
        [Route("GetLivroList")]
        public async Task<IActionResult> GetLivroList()
        {
            try
            {
               
                var ret = await _livroService.GetLivroList();

                if (ret == null)
                {
                    return NotFound("Livro não encontrado.");
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar livro");
                return StatusCode(500, "Erro ao buscar livro");
            }
        }

        [HttpPost]
        [Route("UpdateLivro")]
        public async Task<IActionResult> UpdateLivro(LivroModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<LivroEntity>(parameters);
                var ret = await _livroService.UpdateLivro(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar livro");
                return StatusCode(500, "Erro ao atualizar livro");
            }
        }

        [HttpDelete]
        [Route("DeleteLivro")]
        public async Task<IActionResult> DeleteLivro([FromQuery] int codl)
        {
            try
            {
                var parameters = new LivroModel { Codl = codl };
                var resultMapper = _mapper.Map<LivroEntity>(parameters);
                var ret = await _livroService.DeleteLivro(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir livro");
                return StatusCode(500, "Erro ao excluir livro");
            }
        }

    }
}
