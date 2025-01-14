namespace FalconSoftChallenge.Business.QueryObjects
{
    public record OrdersQueryModel(
        string? Description, 
        decimal? Amount, 
        DateTime? CreatedDate, 
        int Page = 1, 
        int PageSize = 10, 
        OrderSortingField SortingField = OrderSortingField.CreatedDate,
        OrderSortingWay SortingWay = OrderSortingWay.Desc);

    public enum OrderSortingField 
    {
        Description,
        //Amount,
        CreatedDate
    }

    public enum OrderSortingWay 
    {
        Asc,
        Desc
    }
}
