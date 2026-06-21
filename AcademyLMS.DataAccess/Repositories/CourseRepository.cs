using AcademyLMS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademyLMS.DataAccess.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AcademyDbContext _context;

    public CourseRepository(AcademyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Course>> GetAllAsync(int? teacherId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Courses.AsNoTracking();

        if (teacherId.HasValue)
        {
            query = query.Where(c => c.TeacherId == teacherId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CourseId == courseId, cancellationToken);
    }

    public async Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await _context.Courses.AddAsync(course, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return course;
    }

    public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([courseId], cancellationToken);
        if (course is null)
        {
            return false;
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
