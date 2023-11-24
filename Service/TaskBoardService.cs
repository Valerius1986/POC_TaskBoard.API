using POC_TaskBoard.API.Dto;
using POC_TaskBoard.API.Entities;
using POC_TaskBoard.API.Repos.Interface;
using POC_TaskBoard.API.Service.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace POC_TaskBoard.API.Service
{
    public class TaskBoardService : ITaskBoardService
    {
        private readonly IGenericRepository<Board> _boardRepo;
        private readonly IGenericRepository<BoardUser> _boarduserRepo;
        private readonly ITaskRepository _taskRepository;
        private readonly IStatusOfTaskRepository _statusOfTaskRepository;
        private readonly IGenericRepository<TaskOfBoard> _taskRepo;
        private readonly IGenericRepository<SectionStatus> _statusRepo;

        public TaskBoardService(
            IGenericRepository<Board> boardRepo,
            IGenericRepository<BoardUser> boarduserRepo,
            ITaskRepository taskRepository,
            IStatusOfTaskRepository statusOfTaskRepository,
            IGenericRepository<TaskOfBoard> taskRepo,
            IGenericRepository<SectionStatus> statusRepo)
        {
            _boardRepo = boardRepo;
            _boarduserRepo = boarduserRepo;
            _taskRepository = taskRepository;
            _statusOfTaskRepository = statusOfTaskRepository;
            _taskRepo = taskRepo;
            _statusRepo = statusRepo;
        }

        #region Board
        public async Task<IEnumerable<Board>> GetBoardListAsync()
        {
            var list = await _boardRepo.GetAllAsync();
            var boardIds = list.Select(b => b.Id).ToList();
            var taskList = await _taskRepository.GetTaskInfoByIdsAsync(boardIds);
            foreach (var board in list)
            {
                board.Tasks = taskList.Where(t => t.BoardId == board.Id).ToList();
            }
            return list;
        }

        public async Task<Board> GetBoardByIdAsync(int id)
        {
            var board = await _boardRepo.GetByIdAsync(id);
            if (board is null)
                throw new ArgumentException("No board with this Id");
            var taskList = await _taskRepository.GetTaskInfoByBoardIdAsync(board.Id);
            board.Tasks = taskList;

            return board;
        }

        public async Task<Board> CreateBoardAsync(CreateBoardRequest request)
        {
            var board = new Board();
            board.Name = request.Name;

            var entity = await _boardRepo.AddAsync(board);

            var boardUser = new BoardUser();
            boardUser.UserId = request.UserId;
            boardUser.BoardId = entity.Id;

            await _boarduserRepo.AddAsync(boardUser);

            return entity;
        }

        public async Task<Board> UpdateBoardAsync(UpdateBoardRequest request)
        {
            var board = await _boardRepo.GetByIdAsync(request.Id);
            if(board is null)
                throw new ArgumentException("No board with this Id");
            var taskList = await _taskRepository.GetTaskInfoByBoardIdAsync(board.Id);
            
            board.Name = request.Name;

            var entity = await _boardRepo.UpdateAsync(board, board.Id);
            entity.Tasks = taskList;
            return entity;
        }

        public async Task DeleteBoardAsync(int id)
        {
            var board = await _boardRepo.GetByIdAsync(id);
            if (board is null)
                throw new ArgumentException("No board with this Id");
            
            await _boardRepo.DeleteAsync(id);
        }

        #endregion

        #region Task

        public async Task<IEnumerable<TaskOfBoard>> GetTaskListAsync()
        {
            var list = await _taskRepo.GetAllAsync();
            return list;
        }

        public async Task<List<TaskOfBoard>> GetTaskListByBorderIdAsync(int id)
        {
            var list = await _taskRepo.GetListWhereAsync(t => t.BoardId == id);
            return list;
        }

        public async Task<TaskOfBoard> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task is null)
                throw new ArgumentException("No task with this Id");
            return task;
        }

        public async Task<TaskOfBoard> CreateTaskdAsync(CreateTaskRequest request)
        {
            var board = await _boardRepo.GetByIdAsync(request.BoardId);
            if (board is null)
                throw new ArgumentException("No board with this Id");

            var listTask = await _taskRepository.GetByTaskBordSectionStatusIdAsync(request.StatusId);

            var task = new TaskOfBoard();

            if (listTask.Count() == 0)
            {
                task.OrderId = 1;
            }
            else
            {
                task.OrderId = listTask.Count() + 1;
            }

            task.Name = request.Name;
            task.Description = request.Description;
            task.StartDate = request.StartDate;
            task.DueDate = request.DueDate;
            task.BoardId = request.BoardId;
            task.StatusId = request.StatusId;

            var entity = await _taskRepo.AddAsync(task);
            return entity;
        }

        public async Task<TaskOfBoard> UpdateTaskAsync(UpdateTaskRequest request)
        {
            var task = await _taskRepo.GetByIdAsync(request.Id);
            if (task is null)
                throw new ArgumentException("No task with this Id");
            task.Name = request.Name;
            task.Description = request.Description;
            task.StartDate = request.StartDate;
            task.DueDate = request.DueDate;

            var entity = await _taskRepo.UpdateAsync(task, task.Id);
            return entity;
        }

        public async Task<TaskOfBoard> UpdateOrderIdOfTaskAsync(PartialTaskOfBoardUpdateRequest? request)
        {
            var task = await _taskRepo.GetByIdAsync(request.Id);
            if (task == null)
            {
                throw new ArgumentException("No task with this Id");
            }

            var listTasks = await _taskRepo.GetListWhereAsync(t => t.BoardId == task.BoardId && t.StatusId == task.StatusId);
            var orderedList = listTasks.OrderBy(x => x.OrderId).ToList();
            var index = orderedList.FindIndex(x => x.Id == task.Id);

            orderedList.RemoveAt(index);
            foreach (var item in orderedList)
            {
                item.OrderId = null;
            }
            var resultList = new List<TaskOfBoard>();
            if (request.OrderId == listTasks.Count())
            {
                for (int i = 0, k = 1; i < orderedList.Count(); i++, k++)
                {
                    orderedList[i].OrderId = k;
                    resultList.Add(orderedList[i]);

                }
                task.OrderId = request.OrderId;
                resultList.Add(task);
            }
            else
            {
                for (int i = 0, k = 1; i < orderedList.Count(); i++, k++)
                {
                    if (k == request.OrderId)
                    {
                        task.OrderId = request.OrderId;
                        resultList.Add(task);
                        i--;
                    }
                    else
                    {
                        orderedList[i].OrderId = k;
                        resultList.Add(orderedList[i]);
                    }
                }
            }
            var entity = await _taskRepository.UpdateListAsync(resultList);
            //var resEntity = resultList.Where(x => x.Id == request.Id).FirstOrDefault();


            return task;
        }

        public async Task<TaskOfBoard> UpdateStatusOfTaskAsync(UpdateStatusOfTaskRequest request)
        {
            var task = await _taskRepo.GetByIdAsync(request.Id);
            if (task is null)
                throw new ArgumentException("No task with this Id");
            task.StatusId = request.StatusId;

            var entity = await _taskRepo.UpdateAsync(task, task.Id);
            return entity;
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task is null)
                throw new ArgumentException("No task with this Id");

            await _taskRepo.DeleteAsync(id);
        }

        #endregion

        #region Status

        public async Task<IEnumerable<SectionStatus>> GetStatusesListAsync()
        {
            var list = await _statusRepo.GetAllAsync();
            return list;
        }

        public async Task<SectionStatus> GetStatusByIdAsync(int id)
        {
            var status = await _statusRepo.GetByIdAsync(id);
            if (status is null)
                throw new ArgumentException("No status with this Id");
            return status;
        }

        public async Task<List<SectionStatus>> GetStatusListByBorderIdAsync(int boardId)
        {
            var status = await _statusOfTaskRepository.GetStatusListByBoardIdAsync(boardId);
            if (status is null)
                throw new ArgumentException("No status with this Board Id");
            return status;
        }

        public async Task<SectionStatus> CreateStatusAsync(CreateStatusRequest request)
        {
            var board = await _boardRepo.GetByIdAsync(request.BoardId);
            if (board is null)
                throw new ArgumentException("No board with this Id");

            var status = new SectionStatus();
            status.Name = request.Name;
            status.BoardId = request.BoardId;

            var entity = await _statusRepo.AddAsync(status);
            return entity;
        }

        public async Task<SectionStatus> UpdateStatusAsync(UpdateStatusRequest request)
        {
            var status = await _statusRepo.GetByIdAsync(request.Id);
            if (status is null)
                throw new ArgumentException("No status with this Id");
            status.Name = request.Name;

            var entity = await _statusRepo.UpdateAsync(status, status.Id);
            return entity;
        }

        public async Task DeleteStatusAsync(int id)
        {
            var status = await _statusRepo.GetByIdAsync(id);
            if (status is null)
                throw new ArgumentException("No status with this Id");

            await _statusRepo.DeleteAsync(id);
        }
        #endregion
    }
}
