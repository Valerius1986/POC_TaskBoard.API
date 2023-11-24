namespace POC_TaskBoard.API.Dto
{
    public class CreateStatusRequest
    {
        public int BoardId { get; set; }
        public string? Name { get; set; }
    }
}
