using CrescentSchool.BLL.DTOs;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.BLL.Mappings;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Repositories;

namespace CrescentSchool.BLL.Services;

public class ParentService(IParentsRepository parentsRepository) : IParentService
{
    public Task<Guid> CreateParentAsync(CreateParentDto createParentDto, CancellationToken cancellationToken)
        => parentsRepository.CreateParentAsync(createParentDto, cancellationToken);

    public async Task<List<ParentDto>> GetParentsAsync(CancellationToken cancellationToken)
    {
        var parents = await parentsRepository.GetParentsAsync(cancellationToken);
        return [.. parents.Select(p => p.ToParentDto())];
    }
}
