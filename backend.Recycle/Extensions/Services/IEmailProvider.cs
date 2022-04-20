using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace backend.Recycle.Services
{
    public interface IEmailProvider
    {
        Task Send(string to, string body, string from="");
    }
}
