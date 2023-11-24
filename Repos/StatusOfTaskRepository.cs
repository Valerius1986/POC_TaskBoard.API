using Microsoft.EntityFrameworkCore;
using POC_TaskBoard.API.Context;
using POC_TaskBoard.API.Entities;
using POC_TaskBoard.API.Repos.Interface;

namespace POC_TaskBoard.API.Repos
{
    public class StatusOfTaskRepository : GenericRepository<SectionStatus>, IStatusOfTaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StatusOfTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<List<TaskOfBoard>> GetTaskInfoByIdsAsync(IEnumerable<int> boardIds)
        //{

        //    var tasks = await _dbContext.Tasks.Where(x => boardIds.Contains(x.BoardId)).ToListAsync();

        //    return (tasks);
        //}

        public async Task<List<SectionStatus>> GetStatusListByBoardIdAsync(int boardId)
        {

            var statuses = _dbContext.Statuses.Where(x => x.BoardId == boardId).ToList();

            return (statuses);
        }

        //public async Task<List<TaskOfBoard>> GetByTaskBordSectionStatusIdAsync(int statusId)
        //{
        //    var enteties = _dbContext.Tasks
        //        .Where(t => t.StatusId == statusId)
        //        .OrderBy(t => t.OrderId).ToList();

        //    return (enteties);
        //}

        //public async Task<List<TaskOfBoard>> UpdateListAsync(List<TaskOfBoard> entityList)
        //{
        //    _dbContext.UpdateRange(entityList);
        //    await _dbContext.SaveChangesAsync();
        //    return (entityList);
        //}
    }
}
