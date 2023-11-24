using POC_TaskBoard.API.Entities;

namespace POC_TaskBoard.API.SignalR
{
    public interface ITaskBoardHubClient
    {
        Task ReceiveAllTasks(List<TaskOfBoard> tasks);
    }
}
