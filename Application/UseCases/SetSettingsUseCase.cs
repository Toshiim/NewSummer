using Application.Common.Interfaces.Repository;
using Application.Common.Models;

namespace Application.UseCases;

public class SetSettingsUseCase
{
    private readonly ISubscriberRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SetSettingsUseCase(ISubscriberRepository repository,  IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string userId, SetSettingsCommand settings, CancellationToken ct)
    {
        var subscriber = await _repository.GetByPlatformIdAsync(userId, ct)
                         ?? throw new InvalidOperationException($"Пользователь {userId} не найден");
        subscriber.SetSettings(settings.ArticlesCount, settings.UserTime, settings.UserOffset);
        await _unitOfWork.SaveChangesAsync();
    }
}