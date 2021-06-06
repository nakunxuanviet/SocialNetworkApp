using SocialNetwork.Domain.SeedWork;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        /// <summary>
        /// Publish domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        Task Publish(DomainEvent domainEvent);
    }
}