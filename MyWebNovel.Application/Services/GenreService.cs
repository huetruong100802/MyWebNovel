using MyWebNovel.Application.Exceptions;
using MyWebNovel.Application.DTOs.NovelTag;
using MyWebNovel.Application.Extensions;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Domain.Entities.Novels;

namespace MyWebNovel.Application.Services
{
    public class TagService(IUnitOfWork unitOfWork) : ITagService
    {
        public async Task<TagDto> CreateTagAsync(TagCreateDto newTag)
        {
            var tag = NovelTag.Create(newTag.Name, newTag.Type, newTag.IsActive);
            await unitOfWork.Tags.AddAsync(tag);
            await unitOfWork.SaveChangesAsync();
            return tag.MapToDto();
        }

        public async Task DeleteTagAsync(Guid id)
        {
            var tag = await unitOfWork.Tags.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(NovelTag), nameof(NovelTag.Id), id);
            unitOfWork.Tags.HardDelete(tag);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagDto>> GetAllTags()
        {
            return (await unitOfWork.Tags.GetAllAsync())
                .Select(g => g.MapToDto());
        }

        public async Task<TagDto?> GetTagByIdAsync(Guid id)
        {
            return (await unitOfWork.Tags.GetByIdAsync(id))
                ?.MapToDto();
        }

        public async Task UpdateTagAsync(Guid id, TagUpdateDto tag)
        {
            var existingTag = await unitOfWork.Tags.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(NovelTag), nameof(NovelTag.Id), id);
            existingTag.Update(
                tag.Name ?? existingTag.Name,
                tag.Type ?? existingTag.Type,
                tag.IsActive ?? existingTag.IsActive
                );
            await unitOfWork.SaveChangesAsync();
        }
    }
}
