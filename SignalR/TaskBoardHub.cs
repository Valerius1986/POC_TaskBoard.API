using Microsoft.AspNetCore.SignalR;
using POC_TaskBoard.API.Controllers;
using POC_TaskBoard.API.Dto;
using POC_TaskBoard.API.Entities;
using POC_TaskBoard.API.Service.Interfaces;

namespace POC_TaskBoard.API.SignalR
{
    public class TaskBoardHub : Hub //Hub<ITaskBoardHubClient>
    {
        private readonly ITaskBoardService _service;
        private readonly IHubContext<TaskBoardHub> _hubContext;

        public TaskBoardHub(ITaskBoardService service, IHubContext<TaskBoardHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        #region Board

        public async Task GetAllBoards()
        {
            var boardList = await _service.GetBoardListAsync();
            await Clients.All.SendAsync("AllBoards", boardList);
        }

        public async Task AddBoard(CreateBoardRequest request)
        {
            var board = await _service.CreateBoardAsync(request);
            await Clients.All.SendAsync("BoardAdded", board);
            var listBoards = await _service.GetBoardListAsync();
            await _hubContext.Clients.All.SendAsync("AllBoards", listBoards);
        }

        public async Task ChangeBoard(UpdateBoardRequest request)
        {
            var board = await _service.UpdateBoardAsync(request);
            await Clients.All.SendAsync("BoardUpdated", board);
            var listBoards = await _service.GetBoardListAsync();
            await _hubContext.Clients.All.SendAsync("AllBoards", listBoards);
        }

        public async Task RemoveBoard(int boardId)
        {
            var board = await _service.GetBoardByIdAsync(boardId);
            await _service.DeleteBoardAsync(boardId);
            await Clients.All.SendAsync("BoardRemoved", boardId);

            var listBoards = await _service.GetBoardListAsync();
            await _hubContext.Clients.All.SendAsync("AllBoards", listBoards);
        }

        #endregion

        #region Task

        public async Task GetAllTasks(int boardId)
        {
            var tasklList = await _service.GetTaskListByBorderIdAsync(boardId);
            await Clients.All.SendAsync("All tasks", tasklList);
        }

        public async Task AddTasks(CreateTaskRequest request)
        {
            var task = await _service.CreateTaskdAsync(request);
            await Clients.All.SendAsync("TaskAdded", task);
            var listTasks = await _service.GetTaskListByBorderIdAsync(request.BoardId);
            await _hubContext.Clients.All.SendAsync("All tasks", listTasks);
        }

        public async Task ChangeTask(UpdateTaskRequest request)
        {
            var task = await _service.UpdateTaskAsync(request);
            await Clients.All.SendAsync("TaskUpdated", task);
            var listTasks = await _service.GetTaskListByBorderIdAsync(request.BoardId);
            await Clients.All.SendAsync("All tasks", listTasks);
        }

        public async Task ChangeSectionOfTask(UpdateStatusOfTaskRequest request)
        {
            var task = await _service.UpdateStatusOfTaskAsync(request);
            await Clients.All.SendAsync("SectionOfTaskUpdated", task);
            var listTasks = await _service.GetTaskListByBorderIdAsync(request.BoardId);
            await Clients.All.SendAsync("All tasks", listTasks);
        }

        public async Task ChangeOrderIdOfTask(PartialTaskOfBoardUpdateRequest request, int boardId)
        {
            var task = await _service.UpdateOrderIdOfTaskAsync(request);
            await Clients.All.SendAsync("OrderOfTaskUpdated", task);
            var listTasks = await _service.GetTaskListByBorderIdAsync(boardId);
            await Clients.All.SendAsync("All tasks", listTasks);
        }

        public async Task RemoveTask(int taskId)
        {
            var task = await _service.GetTaskByIdAsync(taskId);
            await _service.DeleteTaskAsync(taskId);
            await Clients.All.SendAsync("TaskRemoved", taskId);

            var listTasks = await _service.GetTaskListByBorderIdAsync(task.BoardId);
            await Clients.All.SendAsync("All tasks", listTasks);
        }

        #endregion

        #region Section

        public async Task GetAllSections(int boardId)
        {
            var statusList = await _service.GetStatusListByBorderIdAsync(boardId);
            await Clients.All.SendAsync("AllSections", statusList);
        }

        public async Task AddSection(CreateStatusRequest request)
        {
            var section = await _service.CreateStatusAsync(request);
            await Clients.All.SendAsync("SectionAdded", section);
            var statusList = await _service.GetStatusListByBorderIdAsync(request.BoardId);
            await Clients.All.SendAsync("AllSections", statusList);
        }

        public async Task ChangeSection(UpdateStatusRequest request)
        {
            var section = await _service.UpdateStatusAsync(request);
            await Clients.All.SendAsync("SectionUpdated", section);
            var statusList = await _service.GetStatusListByBorderIdAsync((int)section.BoardId);
            await Clients.All.SendAsync("AllSections", statusList);
        }

        public async Task RemoveSection(int sectionId)
        {
            var section = await _service.GetStatusByIdAsync(sectionId);
            await _service.DeleteStatusAsync(sectionId);
            await Clients.All.SendAsync("StatusRemoved", sectionId);

            var statusList = await _service.GetStatusListByBorderIdAsync((int)section.BoardId);
            await Clients.All.SendAsync("AllSections", statusList);
        }

        #endregion
    }
}
