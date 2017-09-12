using libermedical.Enums;

namespace libermedical.Request
{
    public class GetListRequest
    {
        public GetListRequest(int limit, int page, string searchValue = "", string searchFields = "", string sortField = "", SortDirectionEnum sortDirection = SortDirectionEnum.Asc)
        {
            SortDirection = sortDirection;
            SearchValue = searchValue;
            SearchFields = searchFields;
            SortField = sortField;
            Limit = limit;
            Page = page;
        }

        public string SearchValue { get; }
        public int Limit { get; }
        public int Page { get; }
        public string SearchFields { get; }
        public string SortField { get; set; }
        public SortDirectionEnum SortDirection { get; }
    }
}
