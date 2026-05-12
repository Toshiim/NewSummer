using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IDigestFiltrationService
{
    IEnumerable<ArticleViewModel> FilterCandidates(IEnumerable<ArticleViewModel> candidates, IReadOnlyCollection<Category> categories, int limit);
}