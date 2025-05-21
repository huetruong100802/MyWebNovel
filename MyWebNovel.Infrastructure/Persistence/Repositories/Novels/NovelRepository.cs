using Microsoft.EntityFrameworkCore;
using MyWebNovel.Domain.Entities.Novels;
using MyWebNovel.Domain.Entities.Novels.Filters;
using MyWebNovel.Domain.Entities.Novels.Repositories;
using MyWebNovel.Infrastructure.Specifications;
using MyWebNovel.Infrastructure.Specifications.CommonSpecifcations;
using MyWebNovel.Infrastructure.Specifications.NovelSpecifications;

namespace MyWebNovel.Infrastructure.Persistence.Repositories.Novels
{
    public class NovelRepository(AppDbContext context) : Repository<Novel>(context), INovelRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Novel>> GetFilteredNovelsAsync(NovelFilter novelFilter)
        {
            // Start with a base specification (e.g., a "True" specification)
            ISpecification<Novel> combinedSpecification = new TrueSpecification<Novel>();

            // Combine Title Filter
            if (!string.IsNullOrWhiteSpace(novelFilter.Title))
            {
                var titleSpec = new TitleSearchSpecification(novelFilter.Title);
                combinedSpecification = titleSpec.And(combinedSpecification);
            }

            // Combine Languages Filter
            if (novelFilter.Languages is { Count: > 0 })
            {
                var languageSpec = new LanguageFilterSpecification(novelFilter.Languages);
                combinedSpecification = languageSpec.And(combinedSpecification);
            }

            // Combine Tags Filter
            if (novelFilter.Tags is { Count: > 0 })
            {
                var tagSpec = new TagFilterSpecification(novelFilter.Tags);
                combinedSpecification = tagSpec.And(combinedSpecification);
            }

            // Combine PublicationStatuses Filter
            if (novelFilter.PublicationStatuses is { Count: > 0 })
            {
                var statusSpec = new PublicationStatusFilterSpecification(novelFilter.PublicationStatuses);
                combinedSpecification = statusSpec.And(combinedSpecification);
            }

            // Combine MinimumRating Filter
            if (novelFilter.MinimumRating.HasValue)
            {
                var ratingSpec = new RatingFilterSpecification(novelFilter.MinimumRating.Value);
                combinedSpecification = ratingSpec.And(combinedSpecification);
            }

            // Combine ContentRatings Filter
            if (novelFilter.ContentRatings is { Count: > 0 })
            {
                var contentRatingSpec = new ContentRatingFilterSpecification(novelFilter.ContentRatings);
                combinedSpecification = contentRatingSpec.And(combinedSpecification);
            }

            // Apply the combined specification
            var query = _context.Novels.AsQueryable().Where(combinedSpecification.ToExpression());

            // Execute the query and return the result as a list
            return await query.ToListAsync();
        }
    }
}
