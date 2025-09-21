namespace ECommerece.CommonLibrary.Responses
{
    public record BaseResponse(bool IsSuccess = false, string Message = null!, dynamic Data = null!);
}
