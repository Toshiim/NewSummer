using Application.Common.Interfaces.Repository;
using Domain.Entities;

namespace Application.UseCases;

public class RegisterSubscriberUseCase
{    
    protected ISubscriberRepository _repository;
    protected IUnitOfWork _unitOfWork;

    public RegisterSubscriberUseCase(ISubscriberRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task ExecuteAsync(string chatId, string username, string userId, CancellationToken ct)
    {
        var subscriber = new Subscriber(username, userId, chatId);
        await _repository.AddAsync(subscriber, ct);
        await _unitOfWork.SaveChangesAsync();
    }
}