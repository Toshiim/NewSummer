using Application.Common.Interfaces.Repository;
using Application.Common.Models;
using Domain.Entities;

namespace Application.UseCases;

public class CreateSourceUseCase
{
    private readonly ISourceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSourceUseCase(ISourceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Guid> CreateSource(CreateSourceCommand command, CancellationToken ct)
    {
        var source = new Source(
            command.Name,
            command.SiteUrl,
            command.FeedUrl);
        
        await _repository.AddAsync(source, ct);
        await _unitOfWork.SaveChangesAsync();
        return source.Id;
    }
}