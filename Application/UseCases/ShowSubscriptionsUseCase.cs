using Application.Common.Interfaces.Repository;
using Domain.Entities;

namespace Application.UseCases;

public class ShowSubscriptionsUseCase
{
    private readonly ISubscriberRepository _subscriberRepository;

    public ShowSubscriptionsUseCase(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;
    }
    
    public async Task<Category[]> ExecuteAsync(string userId, CancellationToken ct)
    {
        var subscriber = await _subscriberRepository.GetByPlatformIdAsync(userId, ct);
        if (subscriber == null) throw new NullReferenceException("subscriber is null");
        return subscriber.Categories.ToArray();
    }
}