namespace ECommerece.CommonLibrary.Generics
{
    public interface IGenericService <T> where T : class
    {
        Task<BaseResponse> CreateAsync(T entity);
        Task<BaseResponse> UpdateAsync(T entity);
        Task<BaseResponse> DeleteAsync(T entity);
        Task<BaseResponse> GetAllAsync();
        Task<BaseResponse> GetByIdAsync(int id);
        Task<BaseResponse> GetByAsync(Expression<Func<T, bool>> predicate); //the parameter represents a lamda expression that i will pass when i call this method then the reocrds that achives that lamda filter they will return true (the usage of the bool) the the method will take that ones that returns true
    }
}
