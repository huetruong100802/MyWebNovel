using MyWebNovel.Application.DTOs.Novel;
using MyWebNovel.Application.Exceptions;
using MyWebNovel.Application.Extensions;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Pagination;
using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Shared.ValueObjects;

namespace MyWebNovel.Application.Services
{
    public class NovelService(IUnitOfWork unitOfWork) : INovelService
    {
        public async Task ModifyTagAsync(Guid novelId, string TagName, bool addTag)
        {
            var novel = await unitOfWork.Novels.GetByIdAsync(novelId)
                ?? throw new ArgumentException("Novel not found.", nameof(novelId));

            var Tag = await unitOfWork.Tags.GetByNameAsync(TagName)
                ?? throw new ArgumentException("Tag not found.", nameof(TagName));

            if (addTag)
            {
                novel.AddTag(Tag);
            }
            else
            {
                novel.RemoveTag(Tag);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<NovelDto> CreateNovelAsync(CreateNovelDto command)
        {
            var novel = Novel.Create(
                command.Title,
                command.Synopsis,
                Language.Create(command.Language),
                command.AuthorId,
                PublicationStatus.Draft,
                command.PublicationDate,
                contentRating: ContentRating.General
            );
            await unitOfWork.Novels.AddAsync(novel);
            await unitOfWork.SaveChangesAsync();
            return novel.MapToDto();
        }

        public async Task<PaginatedResult<NovelDto>> GetFilteredNovelsAsync(PaginationParams pagination, NovelFilterDto novelFilterDto)
        {
            return (await unitOfWork.Novels.GetFilteredNovelsAsync(novelFilterDto.MapToNovelFilter()))
                .Select(e => e.MapToDto())
                .ToPaginatedResult(pagination);
        }

        public async Task<NovelDto?> GetNovelByIdAsync(Guid novelId)
        {
            var novel = await unitOfWork.Novels.GetByIdAsync(novelId);
            return novel?.MapToDto();
        }

        public async Task UpdateNovelAsync(Guid id, NovelUpdateDto input)
        {
            var novel = await unitOfWork.Novels.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Novel), nameof(Novel.Id), id);

            // Use the Update method of the Novel class
            novel.Update(
                input.Title ?? novel.Title,
                input.Synopsis ?? novel.Synopsis,
                input.Language is null ? novel.OriginalLanguage : Language.Create(input.Language),
                input.PublicationStatus ?? novel.PublicationStatus,
                input.PublicationDate ?? novel.PublicationDate,
                input.ContentRating ?? novel.ContentRating
            );

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNovelAsync(Guid id)
        {
            var novel = await unitOfWork.Novels.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Novel), nameof(Novel.Id), id);
            //unitOfWork.Novels.Remove(novel);
            novel.SoftDelete();

            await unitOfWork.SaveChangesAsync();
        }
    }
}
