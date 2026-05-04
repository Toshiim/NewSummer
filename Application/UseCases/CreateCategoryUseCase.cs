using Application.Common.Interfaces.Repository;
using Application.Common.Models;
using Domain.Entities;

namespace Application.UseCases;

public class CreateCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryUseCase(ICategoryRepository repository,  IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateCategory(CreateCategoryCommand command, CancellationToken ct)
    {
        var category = new Category(command.DisplayName, command.Slug);
        await _repository.AddAsync(category, ct);
        await _unitOfWork.SaveChangesAsync();
        return category.Id;
    }
}