using POC_TaskBoard.API.Entities;

namespace POC_TaskBoard.API.Repos.Interface
{
    public interface ITaskRepository
    {
        Task<List<TaskOfBoard>> GetTaskInfoByIdsAsync(IEnumerable<int> boardIds);
        Task<List<TaskOfBoard>> GetTaskInfoByBoardIdAsync(int boardId);
        Task<List<TaskOfBoard>> GetByTaskBordSectionStatusIdAsync(int statusId);
        Task<List<TaskOfBoard>> UpdateListAsync(List<TaskOfBoard> entityList);
    }
}
