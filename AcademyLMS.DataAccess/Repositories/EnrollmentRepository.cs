using AcademyLMS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademyLMS.DataAccess.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AcademyDbContext _context;

    public EnrollmentRepository(AcademyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetByIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.StudentId == studentId && e.CourseId == courseId,
                cancellationToken);
    }

    public async Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        await _context.Enrollments.AddAsync(enrollment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return enrollment;
    }

    public async Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        var enrollment = await _context.Enrollments.FindAsync([studentId, courseId], cancellationToken);
        if (enrollment is null)
        {
            return false;
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
