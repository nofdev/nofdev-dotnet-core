using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Nofdev.Sample.API
{
    public interface IHelloFacade
    {
        DateTime GetNow();

        Task<string> AddTask(TaskDTO dto);

        string Say(MessageDTO dto);
    }

    public class MessageDTO
    {
        [Required]
        [StringLength(10)]
        public string Message { get; set; }
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

        public string Say(MessageDTO dto)
        {
            return "Hello," + dto.Message;
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
