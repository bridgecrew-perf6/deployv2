namespace backend.Recycle.Abstracts.Repositories
{
    public interface IUserRepository
    {
        Task<bool> SetAvailabilityEmployee(string userId, int availabilityZoneId);
        
    }
}
