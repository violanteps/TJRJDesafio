﻿using AutoMapper;
using Domain.Entity;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValorLivroController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AutorController> _logger;
        private readonly ILivroService _livroService;

        public ValorLivroController(IMapper mapper, ILogger<AutorController> logger, ILivroService livroService)
        {
            _mapper = mapper;
            _logger = logger;
            _livroService = livroService;
        }

        [HttpPost]
        [Route("CreateLivroValor")]
        public async Task<IActionResult> CreateLivroValor(LivroValorModel parameters)
        {
            try
            {
                var resultMapper = _mapper.Map<LivroValorEntity>(parameters);
                var ret = await _livroService.CreateLivroValor(resultMapper);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar livro");
                return StatusCode(500, "Erro ao criar livro");
            }
        }

        [HttpGet]
        [Route("GetLivroValor")]
        public async Task<IActionResult> GetLivroValor([FromQuery] int livro_Codl)
        {
            try
            {
                if(livro_Codl == 0)
                    return NotFound("Livro não encontrado.");

                var ret = await _livroService.GetLivroValor(livro_Codl);

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
        [Route("GetLivroValorList")]
        public async Task<IActionResult> GetLivroValorList()
        {
            try
            {  
                var ret = await _livroService.GetLivroValorList();

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
        [Route("GetTipoVendaiList")]
        public async Task<IActionResult> GetTipoVendaiList()
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
        [Route("UpdateLivroValor")]
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
        [Route("DeleteLivroValor")]
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
