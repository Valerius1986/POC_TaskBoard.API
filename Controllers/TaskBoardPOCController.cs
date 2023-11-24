using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using POC_TaskBoard.API.Context;
using POC_TaskBoard.API.Dto;
using POC_TaskBoard.API.Entities;
using POC_TaskBoard.API.Service.Interfaces;
using POC_TaskBoard.API.SignalR;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POC_TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskBoardPOCController : ControllerBase
    {
        private readonly ITaskBoardService _service;
        private readonly ILogger<TaskBoardPOCController> _logger;
        private readonly IHubContext<TaskBoardHub> _hubContext;

        public TaskBoardPOCController(
            ITaskBoardService service,
            ILogger<TaskBoardPOCController> logger,
            IHubContext<TaskBoardHub> hubContext)
        {
            _service = service;
            _logger = logger;
            _hubContext = hubContext;
        }


        #region Board
        // GET: api/<BoardController>
        [HttpGet("/GetAllBoards")]
        public async Task<ActionResult<IEnumerable<Board>>> GetAllBoards()
        {
            var boarlList = await _service.GetBoardListAsync();
            return Ok(boarlList);
        }

        // GET api/<BoardController>/5
        [HttpGet("/GetBoardById/{id}")]
        public async Task<ActionResult<Board>> GetBoardById(int id)
        {
            var board = await _service.GetBoardByIdAsync(id);
            return Ok(board);
        }

        // POST api/<BoardController>
        [HttpPost("/PostBoard")]
        public async Task<ActionResult<Board>> PostBoard(CreateBoardRequest request)
        {
            _logger.LogInformation("Start PostBoard method");
            try
            {
                var board = await _service.CreateBoardAsync(request);
                await _hubContext.Clients.All.SendAsync("AddBoard", request);
                //_logger.LogInformation("Board Sucsessfully added to Db");
                return Ok(board);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create board");
                return BadRequest();
            }
        }

        // PUT api/<BoardController>/5
        [HttpPut("/UpdateBoard")]
        public async Task<ActionResult<Board>> PutBoard(UpdateBoardRequest request)
        {
            var board = await _service.UpdateBoardAsync(request);
            await _hubContext.Clients.All.SendAsync("ChangeBoard", request);
            return Ok(board);
        }

        // DELETE api/<BoardController>/5
        [HttpDelete("/DeleteBoard/{id}")]
        public async Task<ActionResult> DeleteBoard(int id)
        {
            await _service.DeleteBoardAsync(id);
            await _hubContext.Clients.All.SendAsync("RemoveBoard", id);
            return NoContent();
        }

        #endregion

        #region Task

        // GET: api/<BoardController>
        [HubMethodName("GetAllTasks")]
        [HttpGet("/GetAllTasks")]
        public async Task<ActionResult<IEnumerable<TaskOfBoard>>> GetAllTasks(int boardId)
        {
            var tasklList = await _service.GetTaskListByBorderIdAsync(boardId);
            await _hubContext.Clients.All.SendAsync("GetAllTasks", boardId);
            return Ok(tasklList);
        }

        // GET api/<BoardController>/5
        [HttpGet("/GetTaskById/{id}")]
        public async Task<ActionResult<TaskOfBoard>> GetTaskById(int id)
        {
            var task = await _service.GetTaskByIdAsync(id);
            return Ok(task);
        }

        // POST api/<BoardController>
        [HttpPost("/PostTask")]
        public async Task<ActionResult<TaskOfBoard>> PostTask(CreateTaskRequest request)
        {
            var task = await _service.CreateTaskdAsync(request);
            //var listTasks = await _service.GetTaskListByBorderIdAsync(request.BoardId);
            await _hubContext.Clients.All.SendAsync("AddTask", request);
            //await _hubContext.Clients.All.SendAsync("All tasks", listTasks);
            return Ok(task);
        }

        // PUT api/<BoardController>/5
        [HttpPut("/UpdateTask")]
        public async Task<ActionResult<TaskOfBoard>> PutTask(UpdateTaskRequest request)
        {
            var task = await _service.UpdateTaskAsync(request);
            await _hubContext.Clients.All.SendAsync("ChangeTask", request);
            return Ok(task);
        }

        // PUT api/<BoardController>/5
        [HttpPut("/UpdateOrderIdOfTask")]
        public async Task<ActionResult<TaskOfBoard>> PutOrderOfTask(PartialTaskOfBoardUpdateRequest request, int boardId)
        {
            var task = await _service.UpdateOrderIdOfTaskAsync(request);
            await _hubContext.Clients.All.SendAsync("ChangeOrderIdOfTask", request, boardId);
            return Ok(task);
        }

        // PUT api/<BoardController>/5
        [HttpPut("/UpdateStatusOfTask")]
        public async Task<ActionResult<TaskOfBoard>> PutStatusOfTask(UpdateStatusOfTaskRequest request)
        {
            var task = await _service.UpdateStatusOfTaskAsync(request);
            await _hubContext.Clients.All.SendAsync("ChangeSectionOfTaskd", request);
            return Ok(task);
        }

        // DELETE api/<BoardController>/5
        [HttpDelete("/DeleteTask/{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            await _service.DeleteTaskAsync(id);
            await _hubContext.Clients.All.SendAsync("RemoveTask", id);
            return NoContent();
        }

        #endregion

        #region Status

        // GET: api/<BoardController>
        [HttpGet("/GetAllStatuses")]
        public async Task<ActionResult<IEnumerable<SectionStatus>>> GetAllStatuses()
        {
            var statuslList = await _service.GetStatusesListAsync();
            return Ok(statuslList);
            
        }

        // GET api/<BoardController>/5
        [HttpGet("/GetStatusById/{id}")]
        public async Task<ActionResult<SectionStatus>> GetStatusById(int id)
        {
            var status = await _service.GetStatusByIdAsync(id);
            return Ok(status);
        }
        

        // POST api/<BoardController>
        [HttpPost("/PostStatus")]
        public async Task<ActionResult<SectionStatus>> PostStatus(CreateStatusRequest request)
        {
            var status = await _service.CreateStatusAsync(request);
            await _hubContext.Clients.All.SendAsync("AddSection", request);
            return Ok(status);
        }

        // PUT api/<BoardController>/5
        [HttpPut("/UpdateStatus")]
        public async Task<ActionResult<SectionStatus>> PutStatus(UpdateStatusRequest request)
        {
            var status = await _service.UpdateStatusAsync(request);
            await _hubContext.Clients.All.SendAsync("ChangeSection", request);
            return Ok(status);
        }

        // DELETE api/<BoardController>/5
        [HttpDelete("/DeleteStatus/{id}")]
        public async Task<ActionResult> DeleteStatus(int id)
        {
            await _service.DeleteStatusAsync(id);
            await _hubContext.Clients.All.SendAsync("RemoveSection", id);
            return NoContent();
        }

        #endregion
    }
}
