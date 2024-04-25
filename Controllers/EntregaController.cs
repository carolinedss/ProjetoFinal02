using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto02.Context;
using Projeto02.Models;

namespace Projeto02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EntregaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// retorna as entregas existentes
        /// </summary>
        /// <remarks>
        ///  {
   /// "codigoEpi": 0,
    ///"nome": "string",
   /// "formaDu": 0
  ///     }
        /// </remarks>
        ///  /// <response code="200"> sucesso no retorno de dados </response>
          
        // GET: api/Entrega
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entrega>>> GetEntregas()
        {
          if (_context.Entregas == null)
          {
              return NotFound();
          }
            return await _context.Entregas.ToListAsync();
        }

    /// <summary>
        /// retorna a entrega de acordo com o ID passado
        /// </summary>
        ///<remarks>
        ///Retorna os dados da entrega
        /// Exemplo : GET/api//{id}
        /// Substitua o Id pelo código de identificação do usuário 
        ///}
        ///</remarks>
        // GET: api/Entrega/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrega>> GetEntrega(int id)
        {
          if (_context.Entregas == null)
          {
              return NotFound();
          }
            var entrega = await _context.Entregas.FindAsync(id);

            if (entrega == null)
            {
                return NotFound();
            }

            return entrega;
        }

          /// <summary>
        /// Atualiza a entrega no banco de dados
        /// </summary>
        /// <remarks>
        /// Atualiza os dados da entrega
        /// Exemplo : GET/api/Entrega/{id}
        /// Substitua o Id pelo código de identificação do usuário 
        ///}
        /// </remarks>

        // PUT: api/Entrega/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrega(int id, Entrega entrega)
        {
            if (id != entrega.CodigoEntrega)
            {
                return BadRequest();
            }

            _context.Entry(entrega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntregaExists(id))
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
        /// Cadastra uma nova entrega
        /// </summary>
        /// <remarks>
        /// exemplo: {
 /// "codigoEntrega": 0,
 /// "codigoEpi": 0,
///  "codigoColab": 0,
  ///"dtValidade": "2024-04-25",
  ///"dtEntrega": "2024-04-25"
///}
          /// </remarks>
        /// 

        // POST: api/Entrega
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entrega>> PostEntrega(Entrega entrega)
        {
          if (_context.Entregas == null)
          {
              return Problem("Entity set 'AppDbContext.Entregas'  is null.");
          }
            _context.Entregas.Add(entrega);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EntregaExists(entrega.CodigoEntrega))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEntrega", new { id = entrega.CodigoEntrega }, entrega);
        }

        /// <summary>
        /// deleta uma entrega existente
        /// </summary>

        // DELETE: api/Entrega/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrega(int id)
        {
            if (_context.Entregas == null)
            {
                return NotFound();
            }
            var entrega = await _context.Entregas.FindAsync(id);
            if (entrega == null)
            {
                return NotFound();
            }

            _context.Entregas.Remove(entrega);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntregaExists(int id)
        {
            return (_context.Entregas?.Any(e => e.CodigoEntrega == id)).GetValueOrDefault();
        }
    }
}
