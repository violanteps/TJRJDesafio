using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CrudController> _logger;
        private readonly ILivroService _livroService;

        public CrudController(IMapper mapper, ILogger<CrudController> logger, ILivroService livroService)
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
