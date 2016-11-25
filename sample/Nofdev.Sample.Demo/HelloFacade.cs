using System;
using System.Threading.Tasks;

namespace Nofdev.Sample.API
{
    public interface IHelloFacade
    {
        DateTime GetNow();

        Task<string> AddTask(TaskDTO dto);
    }

    public class HelloFacade : IHelloFacade
    {
        #region Implementation of IHelloFacade

        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        public async Task<string> AddTask(TaskDTO dto)
        {
            dto.OrderNumber = Guid.NewGuid().ToString("N");
            return dto.OrderNumber;
        }


        #endregion
    }

    public class TaskDTO
    {
        public string OrderNumber { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
