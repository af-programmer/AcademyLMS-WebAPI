using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Exceptions;
using AcademyLMS.DataAccess.Entities;
using AcademyLMS.DataAccess.Repositories;
using AutoMapper;

namespace AcademyLMS.BusinessLogic.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EnrollmentDto>> GetAllAsync(int? studentId = null, int? courseId = null, CancellationToken cancellationToken = default)
    {
        var enrollments = await _enrollmentRepository.GetAllAsync(studentId, courseId, cancellationToken);
        return _mapper.Map<IReadOnlyList<EnrollmentDto>>(enrollments);
    }

    public async Task<EnrollmentDto?> GetByIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(studentId, courseId, cancellationToken);
        return enrollment is null ? null : _mapper.Map<EnrollmentDto>(enrollment);
    }

    public async Task<EnrollmentDto> CreateAsync(EnrollmentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(createDto.StudentId, cancellationToken);
        if (student is null)
        {
            throw new NotFoundException($"Student with id {createDto.StudentId} was not found.");
        }

        var course = await _courseRepository.GetByIdAsync(createDto.CourseId, cancellationToken);
        if (course is null)
        {
            throw new NotFoundException($"Course with id {createDto.CourseId} was not found.");
        }

        var existing = await _enrollmentRepository.GetByIdAsync(createDto.StudentId, createDto.CourseId, cancellationToken);
        if (existing is not null)
        {
            throw new ConflictException(
                $"Student {createDto.StudentId} is already enrolled in course {createDto.CourseId}.");
        }

        var enrollment = _mapper.Map<Enrollment>(createDto);
        var created = await _enrollmentRepository.AddAsync(enrollment, cancellationToken);
        return _mapper.Map<EnrollmentDto>(created);
    }

    public async Task<EnrollmentDto?> UpdateAsync(
        int studentId,
        int courseId,
        EnrollmentDto enrollmentDto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _enrollmentRepository.GetByIdAsync(studentId, courseId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(enrollmentDto, existing);
        existing.StudentId = studentId;
        existing.CourseId = courseId;

        await _enrollmentRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<EnrollmentDto>(existing);
    }

    public async Task<bool> DeleteAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        return await _enrollmentRepository.DeleteAsync(studentId, courseId, cancellationToken);
    }
}
