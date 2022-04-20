using AutoMapper;
using backend.Recycle.Abstracts.Repositories;
using backend.Recycle.Data;
using backend.Recycle.Data.Models;
using backend.Recycle.Data.ViewModels;
using backend.Recycle.Extensions;
using Microsoft.EntityFrameworkCore;

namespace backend.Recycle.Cores.Repositories
{
    public class RequestRepository:IRequestRepository
    {
        private readonly IMapper _mapper;
        private readonly REGISTERDbContext _context;
        public RequestRepository(IMapper mapper,REGISTERDbContext ctx)
        {
            _mapper = mapper;
            this._context = ctx;
        }
        public async Task<bool> PostRequest(UserRequestModel model,string userId)
        {
            var request = _mapper.Map<UserRequestModel, RequestEntity>(model);
            request.UserId = userId;
            await _context.Database.BeginTransactionAsync();
         var recordRequest=  await _context.Requests.AddAsync(request);
           await _context.SaveChangesAsync();
           var employees = _context.AvailabilityEmployee.Where(e => e.AvailabilityZoneId == model.AvailabilityZoneId)
               .Include(e => e.Employee).AsEnumerable();
           var getMin = GetMinimumRecieve(employees);
           await _context.ReceivedRequests.AddAsync(new ReceivedRequest()
           {
               EmployeeId = getMin,RequestId =recordRequest.Entity.Id
           });
           await _context.SaveChangesAsync();
           await _context.Database.CommitTransactionAsync();
         
         return true;

        }

        private string GetMinimumRecieve(IEnumerable<AvailabilityEmployee> employees)
        {
            
            string user = null;
            Dictionary<string, int> employeeCount = new Dictionary<string, int>();
            var result = _context.ReceivedRequests.AsEnumerable();
            foreach (var employee in employees)
            {
                user = employee.EmployeeId;
                var min = result.Where(e => e.EmployeeId == employee.EmployeeId).Count();
                employeeCount.Add(user,min);
            }

            var output =  employeeCount.MinBy(x=>x.Value);

            

            return output.Key;
        }
    }
}
