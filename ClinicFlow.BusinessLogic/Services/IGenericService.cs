namespace ClinicFlow.BusinessLogic.Services;

public interface IGenericService<TDto, TCreateDto>
{
    Task<TDto> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken = default);
}
