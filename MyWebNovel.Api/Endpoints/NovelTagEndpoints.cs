using Microsoft.AspNetCore.Http.HttpResults;
using MyWebNovel.Application.DTOs.NovelTag;
using MyWebNovel.Application.Interfaces;
using MyWebNovel.Application.Security;
using MyWebNovel.Domain.Entities.Novels;
namespace MyWebNovel.Api.Endpoints;

public static class NovelTagEndpoints
{
    public static void MapTagEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Tag").WithTags($"{nameof(NovelTag)}s").AddEndpointFilter<PermissionFilter>();

        group.MapGet("/", async Task<Results<Ok<IEnumerable<TagDto>>, NotFound>> (ITagService service) =>
        {
            var Tags = await service.GetAllTags();
            return Tags.Any() ? TypedResults.Ok(Tags)
                    : TypedResults.NotFound();
        })
        .WithName("GetAllTags")
        .WithMetadata(new EndpointPermissionMetadata(nameof(NovelTag), Domain.Entities.Enums.PermissionLevelEnum.View));

        group.MapGet("/{id}", async Task<Results<Ok<TagDto>, NotFound>> (Guid id, ITagService service) =>
        {
            return await service.GetTagByIdAsync(id)
                is var model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTagById")
        .WithMetadata(new EndpointPermissionMetadata(nameof(NovelTag), Domain.Entities.Enums.PermissionLevelEnum.View));

        group.MapPut("/{id}", async Task<IResult> (Guid id, TagUpdateDto Tag, ITagService service) =>
        {
            await service.UpdateTagAsync(id, Tag);
            return TypedResults.NoContent();
        })
        .WithName("UpdateTag")
        .WithMetadata(new EndpointPermissionMetadata(nameof(NovelTag), Domain.Entities.Enums.PermissionLevelEnum.Modifed));

        group.MapPost("/", async (TagCreateDto newTag, ITagService service) =>
        {
            var Tag = await service.CreateTagAsync(newTag);
            return TypedResults.Created($"/api/Tag/{Tag.Id}", Tag);
        })
        .WithName("CreateTag")
        .WithMetadata(new EndpointPermissionMetadata(nameof(NovelTag), Domain.Entities.Enums.PermissionLevelEnum.Create));

        group.MapDelete("/{id}", async Task<IResult> (Guid id, ITagService service) =>
        {
            await service.DeleteTagAsync(id);
            return TypedResults.NoContent();
        })
        .WithName("DeleteTag")
        .WithMetadata(new EndpointPermissionMetadata(nameof(NovelTag), Domain.Entities.Enums.PermissionLevelEnum.Modifed));
    }
}
