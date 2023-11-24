using POC_TaskBoard.API.Dto;
using POC_TaskBoard.API.Entities;

namespace POC_TaskBoard.API.Service.Interfaces
{
    public interface ITaskBoardService
    {
        Task<IEnumerable<Board>> GetBoardListAsync();
        Task<Board> GetBoardByIdAsync(int id);
        Task<Board> CreateBoardAsync(CreateBoardRequest request);
        Task<Board> UpdateBoardAsync(UpdateBoardRequest request);
        Task DeleteBoardAsync(int id);

        Task<IEnumerable<TaskOfBoard>> GetTaskListAsync();
        Task<List<TaskOfBoard>> GetTaskListByBorderIdAsync(int id);
        Task<TaskOfBoard> GetTaskByIdAsync(int id);
        Task<TaskOfBoard> CreateTaskdAsync(CreateTaskRequest request);
        Task<TaskOfBoard> UpdateTaskAsync(UpdateTaskRequest request);
        Task<TaskOfBoard> UpdateOrderIdOfTaskAsync(PartialTaskOfBoardUpdateRequest? request);
        Task<TaskOfBoard> UpdateStatusOfTaskAsync(UpdateStatusOfTaskRequest request);
        Task DeleteTaskAsync(int id);

        Task<IEnumerable<SectionStatus>> GetStatusesListAsync();
        Task<SectionStatus> GetStatusByIdAsync(int id);
        Task<List<SectionStatus>> GetStatusListByBorderIdAsync(int boardId);
        Task<SectionStatus> CreateStatusAsync(CreateStatusRequest request);
        Task<SectionStatus> UpdateStatusAsync(UpdateStatusRequest request);
        Task DeleteStatusAsync(int id);
    }
}
