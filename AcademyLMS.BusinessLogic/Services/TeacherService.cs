using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.DataAccess.Entities;
using AcademyLMS.DataAccess.Repositories;
using AutoMapper;

namespace AcademyLMS.BusinessLogic.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeacherDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var teachers = await _teacherRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TeacherDto>>(teachers);
    }

    public async Task<TeacherDto?> GetByIdAsync(int teacherId, CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);
        return teacher is null ? null : _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var teacher = _mapper.Map<Teacher>(createDto);
        var created = await _teacherRepository.AddAsync(teacher, cancellationToken);
        return _mapper.Map<TeacherDto>(created);
    }

    public async Task<TeacherDto?> UpdateAsync(int teacherId, TeacherDto teacherDto, CancellationToken cancellationToken = default)
    {
        var existing = await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(teacherDto, existing);
        existing.TeacherId = teacherId;

        await _teacherRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<TeacherDto>(existing);
    }

    public async Task<bool> DeleteAsync(int teacherId, CancellationToken cancellationToken = default)
    {
        return await _teacherRepository.DeleteAsync(teacherId, cancellationToken);
    }
}
