namespace POC_TaskBoard.API.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<TaskOfBoard>? Tasks { get; set; }
        public ICollection<BoardUser>? BoardUsers { get; set; }

    }
}
