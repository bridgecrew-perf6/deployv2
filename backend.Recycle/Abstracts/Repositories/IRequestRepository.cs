using backend.Recycle.Data.ViewModels;

namespace backend.Recycle.Abstracts.Repositories
{
    public interface IRequestRepository
    {
        Task<bool> PostRequest(UserRequestModel model,string userId);
    }
}
