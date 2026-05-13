using Application.Common.Interfaces.Repository;
using Domain.Entities;

namespace Application.UseCases;

public class GetSubscriberInfoUseCase
{
    private readonly ISubscriberRepository _repository;
    
    public GetSubscriberInfoUseCase(ISubscriberRepository repository) => _repository = repository;
    
    public Task<Subscriber?> ExecuteAsync(string userId, CancellationToken ct) => _repository.GetByPlatformIdAsync(userId, ct);
}