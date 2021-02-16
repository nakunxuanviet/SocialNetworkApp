using CsvHelper.Configuration;
using SocialNetwork.Application.TodoLists.Queries.ExportTodos;
using System.Globalization;

namespace SocialNetwork.Infrastructure.Files.Maps
{
    public class TodoItemRecordMap : ClassMap<TodoItemRecord>
    {
        public TodoItemRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Done).ConvertUsing(c => c.Done ? "Yes" : "No");
        }
    }
}