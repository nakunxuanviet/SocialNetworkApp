namespace SocialNetwork.Domain.SeedWork
{
    public class ApplicationService
    {
        public ApplicationService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected internal IUnitOfWork UnitOfWork { get; set; }
    }
}
