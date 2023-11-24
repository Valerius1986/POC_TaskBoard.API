using System.Text.Json.Serialization;

namespace POC_TaskBoard.API.Dto
{
    public class UpdateTaskRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int BoardId { get; set; }
    }
}
