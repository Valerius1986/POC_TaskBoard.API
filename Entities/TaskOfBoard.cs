namespace POC_TaskBoard.API.Entities
{
    public class TaskOfBoard
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int BoardId { get; set; }
        public int StatusId { get; set; }
        public int? OrderId { get; set; }
    }
}
