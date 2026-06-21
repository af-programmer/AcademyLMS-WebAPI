using AcademyLMS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademyLMS.DataAccess.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AcademyDbContext _context;

    public TeacherRepository(AcademyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Teacher>> GetAllAsync(string? department = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Teachers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(department))
        {
            var normalizedDepartment = department.Trim().ToLower();
            query = query.Where(t =>
                t.Department != null &&
                t.Department.ToLower() == normalizedDepartment);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Teacher?> GetByIdAsync(int teacherId, CancellationToken cancellationToken = default)
    {
        return await _context.Teachers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TeacherId == teacherId, cancellationToken);
    }

    public async Task<Teacher> AddAsync(Teacher teacher, CancellationToken cancellationToken = default)
    {
        await _context.Teachers.AddAsync(teacher, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return teacher;
    }

    public async Task UpdateAsync(Teacher teacher, CancellationToken cancellationToken = default)
    {
        _context.Teachers.Update(teacher);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int teacherId, CancellationToken cancellationToken = default)
    {
        var teacher = await _context.Teachers.FindAsync([teacherId], cancellationToken);
        if (teacher is null)
        {
            return false;
        }

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
