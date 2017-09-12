using libermedical.Enums;

namespace libermedical.Request
{
    public interface IGetListRequest<TModel>
        where TModel : class
    {
        int Limit { get; }
        int Page { get; }
        string SearchValue { get; }
        string SearchFields { get; }
        string SortField { get; set; }

        SortDirectionEnum SortDirection { get; }
    }
}
