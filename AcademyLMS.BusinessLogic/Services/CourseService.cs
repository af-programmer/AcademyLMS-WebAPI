using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Exceptions;
using AcademyLMS.DataAccess.Entities;
using AcademyLMS.DataAccess.Repositories;
using AutoMapper;

namespace AcademyLMS.BusinessLogic.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public CourseService(
        ICourseRepository courseRepository,
        ITeacherRepository teacherRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CourseDto>> GetAllAsync(int? teacherId = null, CancellationToken cancellationToken = default)
    {
        var courses = await _courseRepository.GetAllAsync(teacherId, cancellationToken);
        return _mapper.Map<IReadOnlyList<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
        return course is null ? null : _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateAsync(CourseCreateDto createDto, CancellationToken cancellationToken = default)
    {
        await EnsureTeacherExistsAsync(createDto.TeacherId, cancellationToken);

        var course = _mapper.Map<Course>(createDto);
        var created = await _courseRepository.AddAsync(course, cancellationToken);
        return _mapper.Map<CourseDto>(created);
    }

    public async Task<CourseDto?> UpdateAsync(int courseId, CourseDto courseDto, CancellationToken cancellationToken = default)
    {
        var existing = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        await EnsureTeacherExistsAsync(courseDto.TeacherId, cancellationToken);

        _mapper.Map(courseDto, existing);
        existing.CourseId = courseId;

        await _courseRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<CourseDto>(existing);
    }

    public async Task<bool> DeleteAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _courseRepository.DeleteAsync(courseId, cancellationToken);
    }

    private async Task EnsureTeacherExistsAsync(int teacherId, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);
        if (teacher is null)
        {
            throw new NotFoundException($"Teacher with id {teacherId} was not found.");
        }
    }
}
