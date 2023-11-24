using Microsoft.EntityFrameworkCore;
using POC_TaskBoard.API.Context;
using POC_TaskBoard.API.Entities;
using POC_TaskBoard.API.Repos.Interface;

namespace POC_TaskBoard.API.Repos
{
    public class TaskRepository : GenericRepository<TaskOfBoard>, ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskOfBoard>> GetTaskInfoByIdsAsync(IEnumerable<int> boardIds)
        {

            var tasks = await _dbContext.Tasks.Where(x => boardIds.Contains(x.BoardId)).ToListAsync();

            return (tasks);
        }

        public async Task<List<TaskOfBoard>> GetTaskInfoByBoardIdAsync(int boardId)
        {

            var tasks = _dbContext.Tasks.Where(x => x.BoardId == boardId).ToList();

            return (tasks);
        }

        public async Task<List<TaskOfBoard>> GetByTaskBordSectionStatusIdAsync(int statusId)
        {
            var enteties = _dbContext.Tasks
                .Where(t => t.StatusId == statusId)
                .OrderBy(t => t.OrderId).ToList();

            return (enteties);
        }

        public async Task<List<TaskOfBoard>> UpdateListAsync(List<TaskOfBoard> entityList)
        {
            _dbContext.UpdateRange(entityList);
            await _dbContext.SaveChangesAsync();
            return (entityList);
        }
    }
}
