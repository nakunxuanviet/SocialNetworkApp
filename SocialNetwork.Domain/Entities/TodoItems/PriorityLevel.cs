using Ardalis.SmartEnum;

namespace SocialNetwork.Domain.Entities.TodoItems
{
    public class PriorityLevel : SmartEnum<PriorityLevel>
    {
        public static readonly PriorityLevel None = new PriorityLevel(nameof(None), 1);
        public static readonly PriorityLevel Low = new PriorityLevel(nameof(Low), 1);
        public static readonly PriorityLevel Medium = new PriorityLevel(nameof(Medium), 1);
        public static readonly PriorityLevel High = new PriorityLevel(nameof(High), 1);

        private PriorityLevel(string name, int value) : base(name, value)
        {
        }
    }
}