using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebNovel.Application.DTOs.Novel;
using MyWebNovel.Application.Extensions;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Pagination;
using MyWebNovel.Application.Security;
using MyWebNovel.Domain.Entities.Enums;
using MyWebNovel.Domain.Entities.Novels;
namespace MyWebNovel.Api.Endpoints;

public static class NovelEndpoints
{
    public static void MapNovelEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Novel").WithTags($"{nameof(Novel)}s").AddEndpointFilter<PermissionFilter>();

        group.MapGet("/", async Task<Ok<PaginatedResult<NovelDto>>> (INovelService service, [FromQuery] PaginationParams pagination, [FromQuery] NovelFilterDto novelFilter) =>
        {
            PaginatedResult<NovelDto> novels = await service.GetFilteredNovelsAsync(pagination, novelFilter);
            return TypedResults.Ok(novels);
        })
        .WithName("GetFilteredNovels")
        .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.View));

        group.MapGet("/{id}", async Task<Results<Ok<NovelDto>, NotFound>> (Guid id, INovelService service) =>
        {
            return await service.GetNovelByIdAsync(id)
                is var novel
                    ? TypedResults.Ok(novel)
                    : TypedResults.NotFound();
        })
        .WithName("GetNovelById")
        .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.View));

        group.MapPut("/{id}", async (INovelService service, Guid id, NovelUpdateDto input) =>
        {
            await service.UpdateNovelAsync(id, input);
            return TypedResults.NoContent();
        })
        .WithName("UpdateNovel")
        .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.Modifed));

        group.MapPost("/", async (INovelService service, CreateNovelDto command) =>
        {
            command.WithAuthorId(Guid.NewGuid());
            var model = await service.CreateNovelAsync(command);
            return TypedResults.Created($"/api/Novels/{model.Id}", model);
        })
        .WithName("CreateNovel")
        .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.Create));

        group.MapDelete("/{id}", async (INovelService service, Guid id) =>
        {
            await service.DeleteNovelAsync(id);
            return TypedResults.NoContent();
        })
        .WithName("DeleteNovel")
        .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.Modifed));

        group.MapPost("/{id}/Tags/{TagName}", async (INovelService service, Guid id, string TagName) =>
        {
            await service.ModifyTagAsync(id, TagName, true);
            return TypedResults.NoContent();
        })
            .WithName("AddTagToNovel")
            .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.Modifed));

        group.MapDelete("/{id}/Tags/{TagName}", async (INovelService service, Guid id, string TagName) =>
        {
            await service.ModifyTagAsync(id, TagName, false);
            return TypedResults.NoContent(); ;
        })
            .WithName("RemoveTagFromNovel")
            .WithMetadata(new EndpointPermissionMetadata(nameof(Novel), PermissionLevelEnum.Modifed));
    }
}
