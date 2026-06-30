using AutoMapper;
using ClinicFlow.DataAccess.Repositories;

namespace ClinicFlow.BusinessLogic.Services;

public class GenericService<TEntity, TDto, TCreateDto> : IGenericService<TDto, TCreateDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IGenericRepository<TEntity> Repository;
    protected readonly IMapper Mapper;
    private readonly Action<TEntity, int> _setId;

    public GenericService(
        IGenericRepository<TEntity> repository,
        IMapper mapper,
        Action<TEntity, int> setId)
    {
        Repository = repository;
        Mapper = mapper;
        _setId = setId;
    }

    public virtual async Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.GetAllAsync(cancellationToken);
        return Mapper.Map<IReadOnlyList<TDto>>(entities);
    }

    public virtual async Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : Mapper.Map<TDto>(entity);
    }

    public async Task<TDto> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TEntity>(createDto);
        var created = await Repository.AddAsync(entity, cancellationToken);
        return Mapper.Map<TDto>(created);
    }

    public virtual async Task<TDto?> UpdateAsync(int id, TDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await Repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        Mapper.Map(dto, existing);
        _setId(existing, id);

        await Repository.UpdateAsync(existing, cancellationToken);
        return Mapper.Map<TDto>(existing);
    }

    public virtual Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return Repository.DeleteAsync(id, cancellationToken);
    }
}
