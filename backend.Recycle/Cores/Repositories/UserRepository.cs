using backend.Recycle.Abstracts.Repositories;
using backend.Recycle.Data;
using backend.Recycle.Data.Models;

namespace backend.Recycle.Cores.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly REGISTERDbContext _context;

        public UserRepository(REGISTERDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SetAvailabilityEmployee(string userId, int availabilityZoneId)
        {
            var user = _context.Users.FirstOrDefault(e => e.Id == userId);
            if (user == null)
                return false;
            var zone = _context.AvailabilityZones.FirstOrDefault(e => e.Id == availabilityZoneId);
            if (zone == null)
                return false;
            await _context.AvailabilityEmployee.AddAsync(new AvailabilityEmployee()
            {
                EmployeeId = user.Id,
                AvailabilityZoneId = zone.Id
            });
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
