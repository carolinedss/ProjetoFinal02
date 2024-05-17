using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto02.Context;
using Projeto02.Models;

namespace Projeto02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColaboradorController(AppDbContext context)
        {
            _context = context;

        }

        /// <summary>
        /// retorna os cadastro existentes
        /// </summary>
        /// <returns></returns>
        ///<remarks>
        ///Sample request:
        ///
        ///  Get do cadastro os dados retornados serão {
        ///"codigo_colab": 1, 
        ///"nome": "nome do colaborador",
        ///"email": "email do colaborador",
        ///"telefone": 18 99999999
        ///"ctps": 00
        ///"dtademissao": 30/11/23
        ///"cpf": 333333333
        /// }
        /// </remarks>
        /// <response code="200"> sucesso no retorno de dados </response>

        // GET: api/Cadastro
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaboradors()
        {
            if (_context.Colaboradors == null)
            {
                return NotFound();
            }
            return await _context.Colaboradors.ToListAsync();
        }

        /// <summary>
        /// retorna o colaborador de acordo com o ID passado
        /// </summary>
        ///<remarks>
        ///Retorna os dados do colaborador
        /// Exemplo : GET/api/Colaborador/{id}
        /// Substitua o Id pelo código de identificação do usuário 
        ///}
        ///</remarks>
     
        /// GET: api/Colaborador/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Colaborador>> GetColaborador(int id)
        {
            if (_context.Colaboradors == null)
            {
                return NotFound();
            }
            var colaborador = await _context.Colaboradors.FindAsync(id);

            if (colaborador == null)
            {
                return NotFound();
            }

            return colaborador;
        }

        /// <summary>
        /// Atualiza o colaborador no banco de dados
        /// </summary>
        /// <remarks>
        /// Atualiza os dados do colaborador
        /// Exemplo : GET/api/Colaborador/{id}
        /// Substitua o Id pelo código de identificação do usuário 
        ///}
        /// </remarks>
        

        // PUT: api/Colaborador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> PutColaborador(int id, Colaborador colaborador)
        {
            if (id != colaborador.CodigoColab)
            {
                return BadRequest();
            }

            _context.Entry(colaborador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColaboradorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <remarks>
        /// exemplo:
///        {
///   "telefone": "string",
///   "nome": "string",
///   "ctps": "string",
///   "dtadmissao": "2024-04-25",
///   "gerente": "string",
///   "email": "string",
///   "cpf": numérico
/// }
        /// </remarks>
        /// 

        // POST: api/Colaborador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize("Admin")]
        public async Task<ActionResult<Colaborador>> PostColaborador(Colaborador colaborador)
        {
            if (_context.Colaboradors == null)
            {
                return Problem("Entity set 'AppDbContext.Colaboradors'  is null.");
            }
            _context.Colaboradors.Add(colaborador);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ColaboradorExists(colaborador.CodigoColab))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetColaborador", new { id = colaborador.CodigoColab }, colaborador);
        }

        /// <summary>
        /// deleta um usuário existente
        /// </summary>


        // DELETE: api/Colaborador/5
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            if (_context.Colaboradors == null)
            {
                return NotFound();
            }
            var colaborador = await _context.Colaboradors.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }

            _context.Colaboradors.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColaboradorExists(int id)
        {
            return (_context.Colaboradors?.Any(e => e.CodigoColab == id)).GetValueOrDefault();
        }
    }
}
