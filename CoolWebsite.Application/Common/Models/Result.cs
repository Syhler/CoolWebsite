using System.Collections.Generic;
using System.Linq;

namespace CoolWebsite.Application.Common.Models
{
    public class Result
    {
        public Result(bool succeeded, IEnumerable<string> errors)
        {
            Errors = errors.ToArray();
            Succeeded = succeeded;
        }

        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, new string[] { });
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }



    }
}