using Application.Common.Interfaces.Repository;
using Domain.Entities;

namespace Application.UseCases;

public class UnsubscribeForCategoresUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnsubscribeForCategoresUseCase(ICategoryRepository categoryRepository,
        ISubscriberRepository subscriberRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _subscriberRepository = subscriberRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Category[]> ExecuteAsync(string userId, string[] rawTags, CancellationToken ct)
    {
        var normalizedTags = rawTags
            .Select(t => t.Trim().ToLower())
            .ToArray();
        
        var categories = await _categoryRepository.GetByTags(normalizedTags, ct);
        
        var subscriber = await _subscriberRepository.GetByPlatformIdAsync(userId, ct);
        foreach (var category in categories) subscriber.UnsubscribeFrom(category);

        await _unitOfWork.SaveChangesAsync();

        return subscriber.Categories.ToArray();
    }
}