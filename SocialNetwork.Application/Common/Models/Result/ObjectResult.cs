using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Application.Common.Models.Result
{
    public class ObjectResult
    {
        internal ObjectResult(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public static ObjectResult Success()
        {
            return new ObjectResult(true, new string[] { });
        }

        public static ObjectResult Failure(IEnumerable<string> errors)
        {
            return new ObjectResult(false, errors);
        }
    }
}