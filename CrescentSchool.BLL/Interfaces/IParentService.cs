using CrescentSchool.BLL.DTOs;
using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.BLL.Interfaces;

public interface IParentService
{
    Task<Guid> CreateParentAsync(CreateParentDto createParentDto, CancellationToken cancellationToken);
    Task<List<ParentDto>> GetParentsAsync(CancellationToken cancellationToken);
}
