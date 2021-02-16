using SocialNetwork.Application.Common.Mappings;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}