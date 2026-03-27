using Microsoft.EntityFrameworkCore;
using PlataformaServicos.Data;
using PlataformaServicos.Models;

namespace PlataformaServicos.Services
{
    public class PrestadorService
    {
        private readonly AppDbContext _context;

        public PrestadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Prestador>> ListarAsync()
        {
            return await _context.Prestadores.ToListAsync();
        }

        public async Task<Prestador?> BuscarPorIdAsync(int id)
        {
            return await _context.Prestadores.FindAsync(id);
        }

        public async Task<Prestador> CriarAsync(Prestador prestador)
        {
            _context.Prestadores.Add(prestador);
            await _context.SaveChangesAsync();
            return prestador;
        }

        public async Task<bool> AtualizarAsync(int id, Prestador prestadorAtualizado)
        {
            var prestador = await _context.Prestadores.FindAsync(id);

            if (prestador == null)
                return false;

            prestador.Nome = prestadorAtualizado.Nome;
            prestador.Email = prestadorAtualizado.Email;
            prestador.Especialidade = prestadorAtualizado.Especialidade;
            prestador.NotaMedia = prestadorAtualizado.NotaMedia;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var prestador = await _context.Prestadores.FindAsync(id);

            if (prestador == null)
                return false;

            _context.Prestadores.Remove(prestador);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}