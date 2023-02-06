using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;

namespace cacambaonline.Controllers
{
    public class LogNotificacaosController : Controller
    {
        private readonly MeuDbContext _context;

        public LogNotificacaosController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: LogNotificacaos
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.LogNotificacoes.Include(l => l.Notificacoes).Include(l => l.Notificacoes_Antigo).Include(l => l.Notificacoes_Novo);
            return View(await meuDbContext.ToListAsync());
        }

        // GET: LogNotificacaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LogNotificacoes == null)
            {
                return NotFound();
            }

            var logNotificacao = await _context.LogNotificacoes
                .Include(l => l.Notificacoes)
                .Include(l => l.Notificacoes_Antigo)
                .Include(l => l.Notificacoes_Novo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logNotificacao == null)
            {
                return NotFound();
            }

            return View(logNotificacao);
        }

        // GET: LogNotificacaos/Create
        public IActionResult Create()
        {
            ViewData["NotificacoesId"] = new SelectList(_context.Notificacoes, "Id", "Id");
            ViewData["Notificacoes_AntigoId"] = new SelectList(_context.Notificacoes_Antigas, "Id", "Id");
            ViewData["Notificacoes_NovoId"] = new SelectList(_context.Notificacoes_Novas, "Id", "Id");
            return View();
        }

        // POST: LogNotificacaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Operacao,UsuarioId,NotificacoesId,Notificacoes_AntigoId,Notificacoes_NovoId")] LogNotificacao logNotificacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logNotificacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificacoesId"] = new SelectList(_context.Notificacoes, "Id", "Id", logNotificacao.NotificacoesId);
            ViewData["Notificacoes_AntigoId"] = new SelectList(_context.Notificacoes_Antigas, "Id", "Id", logNotificacao.Notificacoes_AntigoId);
            ViewData["Notificacoes_NovoId"] = new SelectList(_context.Notificacoes_Novas, "Id", "Id", logNotificacao.Notificacoes_NovoId);
            return View(logNotificacao);
        }

        // GET: LogNotificacaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LogNotificacoes == null)
            {
                return NotFound();
            }

            var logNotificacao = await _context.LogNotificacoes.FindAsync(id);
            if (logNotificacao == null)
            {
                return NotFound();
            }
            ViewData["NotificacoesId"] = new SelectList(_context.Notificacoes, "Id", "Id", logNotificacao.NotificacoesId);
            ViewData["Notificacoes_AntigoId"] = new SelectList(_context.Notificacoes_Antigas, "Id", "Id", logNotificacao.Notificacoes_AntigoId);
            ViewData["Notificacoes_NovoId"] = new SelectList(_context.Notificacoes_Novas, "Id", "Id", logNotificacao.Notificacoes_NovoId);
            return View(logNotificacao);
        }

        // POST: LogNotificacaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Operacao,UsuarioId,NotificacoesId,Notificacoes_AntigoId,Notificacoes_NovoId")] LogNotificacao logNotificacao)
        {
            if (id != logNotificacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logNotificacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogNotificacaoExists(logNotificacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificacoesId"] = new SelectList(_context.Notificacoes, "Id", "Id", logNotificacao.NotificacoesId);
            ViewData["Notificacoes_AntigoId"] = new SelectList(_context.Notificacoes_Antigas, "Id", "Id", logNotificacao.Notificacoes_AntigoId);
            ViewData["Notificacoes_NovoId"] = new SelectList(_context.Notificacoes_Novas, "Id", "Id", logNotificacao.Notificacoes_NovoId);
            return View(logNotificacao);
        }

        // GET: LogNotificacaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LogNotificacoes == null)
            {
                return NotFound();
            }

            var logNotificacao = await _context.LogNotificacoes
                .Include(l => l.Notificacoes)
                .Include(l => l.Notificacoes_Antigo)
                .Include(l => l.Notificacoes_Novo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logNotificacao == null)
            {
                return NotFound();
            }

            return View(logNotificacao);
        }

        // POST: LogNotificacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LogNotificacoes == null)
            {
                return Problem("Entity set 'MeuDbContext.LogNotificacoes'  is null.");
            }
            var logNotificacao = await _context.LogNotificacoes.FindAsync(id);
            if (logNotificacao != null)
            {
                _context.LogNotificacoes.Remove(logNotificacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogNotificacaoExists(int id)
        {
          return _context.LogNotificacoes.Any(e => e.Id == id);
        }
    }
}
