using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IDigestFiltrationService
{
    IEnumerable<ArticleViewModel> FilterCandidates(IEnumerable<ArticleViewModel> candidates, IReadOnlyCollection<Category> categories, int limit);
}