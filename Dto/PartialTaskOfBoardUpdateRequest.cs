using System.ComponentModel.DataAnnotations;

namespace POC_TaskBoard.API.Dto
{
    public class PartialTaskOfBoardUpdateRequest
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue)]
        public int? OrderId { get; set; }
    }
}
