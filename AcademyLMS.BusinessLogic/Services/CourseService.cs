using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.DataAccess.Entities;
using AcademyLMS.DataAccess.Repositories;
using AutoMapper;

namespace AcademyLMS.BusinessLogic.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CourseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var courses = await _courseRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
        return course is null ? null : _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateAsync(CourseCreateDto createDto, CancellationToken cancellationToken = default)
    {
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

        _mapper.Map(courseDto, existing);
        existing.CourseId = courseId;

        await _courseRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<CourseDto>(existing);
    }

    public async Task<bool> DeleteAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _courseRepository.DeleteAsync(courseId, cancellationToken);
    }
}
