using Lanchonete.Context;
using Lanchonete.Models;
using Lanchonete.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lanchonete.Repositories
{
    public class LancheRepository : ILancheRepository
    {
        private readonly AppDbContext _context;

        public LancheRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lanche> Lanches => _context.Lanches.Include(l => l.Categoria);

        public IEnumerable<Lanche> LanchesPreferidos => _context.Lanches
                                   .Where(l => l.LanchePreferido)
                                   .Include(l => l.Categoria);

        public Lanche GetLancheByID(int lancheId)
        {
            return _context.Lanches.FirstOrDefault(l => l.LancheId == lancheId);
        }

    }
}
