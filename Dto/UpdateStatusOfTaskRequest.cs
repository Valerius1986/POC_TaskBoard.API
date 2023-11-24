namespace POC_TaskBoard.API.Dto
{
    public class UpdateStatusOfTaskRequest
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int BoardId { get; set; }
    }
}
