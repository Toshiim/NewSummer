using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;

namespace Infrastructure.FiltrationService;


public class DigestFiltrationService : IDigestFiltrationService
{
    public IEnumerable<ArticleViewModel> FilterCandidates(IEnumerable<ArticleViewModel> candidates, IReadOnlyCollection<Category> categories, int limit)
    {
        var result = new List<ArticleViewModel>();
        var coveredCategories = new HashSet<Guid>();
        var usedArticleIds = new HashSet<Guid>();
        var userCategoryIds = categories.Select(x => x.Id).ToList();
        var userCats = userCategoryIds.ToHashSet();

        var candidateList = candidates.ToList();

        foreach (var article in candidateList)
        {
            if (result.Count >= limit) break;

            var newRelevantCategories = article.CategoryIds
                .Where(id => userCats.Contains(id) && !coveredCategories.Contains(id))
                .ToList();

            if (newRelevantCategories.Any())
            {
                result.Add(article);
                usedArticleIds.Add(article.Id);
                
                foreach (var catId in newRelevantCategories)
                    coveredCategories.Add(catId);
            }

            if (coveredCategories.Count == userCats.Count) break;
        }

        if (result.Count < limit)
        {
            var leftovers = candidateList
                .Where(a => !usedArticleIds.Contains(a.Id))
                .Take(limit - result.Count);

            result.AddRange(leftovers);
        }

        return result.OrderByDescending(a => a.ImportanceScore);
    }
}