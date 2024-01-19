namespace BsMerchantAPI.Models.Responses;

public class MerchantApiResponse<T>
{
    public int StatusCode { get; set; }
    public T? Data { get; set; }
}