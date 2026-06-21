using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.DataAccess.Entities;
using AcademyLMS.DataAccess.Repositories;
using AutoMapper;

namespace AcademyLMS.BusinessLogic.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<StudentDto>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default)
    {
        var students = await _studentRepository.GetAllAsync(email, cancellationToken);
        return _mapper.Map<IReadOnlyList<StudentDto>>(students);
    }

    public async Task<StudentDto?> GetByIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        return student is null ? null : _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var student = _mapper.Map<Student>(createDto);
        var created = await _studentRepository.AddAsync(student, cancellationToken);
        return _mapper.Map<StudentDto>(created);
    }

    public async Task<StudentDto?> UpdateAsync(int studentId, StudentDto studentDto, CancellationToken cancellationToken = default)
    {
        var existing = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(studentDto, existing);
        existing.StudentId = studentId;

        await _studentRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<StudentDto>(existing);
    }

    public async Task<bool> DeleteAsync(int studentId, CancellationToken cancellationToken = default)
    {
        return await _studentRepository.DeleteAsync(studentId, cancellationToken);
    }
}
