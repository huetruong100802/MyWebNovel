using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Novels.Repositories;

namespace MyWebNovel.Infrastructure.Persistence.Repositories.Novels
{
    public class NovelTagRepository(AppDbContext context) : Repository<NovelTag>(context), INovelTagRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<NovelTag?> GetByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(g => g.Name == name);
        }
    }
}
