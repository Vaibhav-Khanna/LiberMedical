using System.Collections.Generic;

namespace libermedical.Responses
{
    public class PaginationResponse<T>
    {
        public int limit { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
        public int total { get; set; }
        public List<T> rows { get; set; }
    }
}
