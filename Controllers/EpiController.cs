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
    [Authorize("Admin")]
    public class EpiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EpiController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// retorna os Epis existentes
        /// </summary>
        /// <remarks>
        ///        {
        ///     "codigoEpi": 0,
        ///     "nome": "string",
        ///     "formaDu": 0
        ///   }
        /// </remarks>
        ///  /// <response code="200"> sucesso no retorno de dados </response>
        // GET: api/Epi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Epi>>> GetEpis()
        {
            if (_context.Epis == null)
            {
                return NotFound();
            }
            return await _context.Epis.ToListAsync();
        }

        /// <summary>
        /// retorna os Epis de acordo com o ID passado
        /// </summary>
        ///<remarks>
        ///Retorna os dados do Epi
        /// Exemplo :{
        ///   "codigoEpi": 0,
        /// "nome": "string",
        ///   "formaDu": 0
        /// }
        ///</remarks>
        // GET: api/Epi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Epi>> GetEpi(int id)
        {
            if (_context.Epis == null)
            {
                return NotFound();
            }
            var epi = await _context.Epis.FindAsync(id);

            if (epi == null)
            {
                return NotFound();
            }

            return epi;
        }

        /// <summary>
        /// Atualiza o Epi no banco de dados
        /// </summary>
        /// <remarks>
        /// Atualiza os dados do Epi
        /// Exemplo : {
        ///   "codigoEpi": 0,
        ///   "nome": "string",
        ///   "formaDu": 0
        /// }
        /// </remarks>
        // PUT: api/Epi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
       
        public async Task<IActionResult> PutEpi(int id, Epi epi)
        {
            if (id != epi.CodigoEpi)
            {
                return BadRequest();
            }

            _context.Entry(epi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EpiExists(id))
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
        /// Cadastra um novo Epi
        /// </summary>
        /// <remarks>
        /// exemplo: {
        ///   "codigoEpi": 0,
        ///   "nome": "string",
        ///   "formaDu": 0
        /// }
        /// </remarks>
        /// 
        // POST: api/Epi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Epi>> PostEpi(Epi epi)
        {
            if (_context.Epis == null)
            {
                return Problem("Entity set 'AppDbContext.Epis'  is null.");
            }
            _context.Epis.Add(epi);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EpiExists(epi.CodigoEpi))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEpi", new { id = epi.CodigoEpi }, epi);
        }

        // DELETE: api/Epi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEpi(int id)
        {
            if (_context.Epis == null)
            {
                return NotFound();
            }
            var epi = await _context.Epis.FindAsync(id);
            if (epi == null)
            {
                return NotFound();
            }

            _context.Epis.Remove(epi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EpiExists(int id)
        {
            return (_context.Epis?.Any(e => e.CodigoEpi == id)).GetValueOrDefault();
        }
    }
}
