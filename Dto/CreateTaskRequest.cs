namespace POC_TaskBoard.API.Dto
{
    public class CreateTaskRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int BoardId { get; set; }
        public int StatusId { get; set; }
    }
}
