using System.Collections.Generic;

namespace ReadLater5.Public.Models
{
    public class GenericResponse
    {
        public bool IsSuccess { get; set; }
        public IReadOnlyList<string> Errors { get; set; }
    }
}
