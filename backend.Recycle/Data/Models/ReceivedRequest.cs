namespace backend.Recycle.Data.Models
{
    public class ReceivedRequest
    {
        public int Id { get; set; }

        public Users Employee { get; set; }
        public string EmployeeId { get; set; }
        public RequestEntity Request { get; set; }
        public int RequestId { get; set; }
    }
}
