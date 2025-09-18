namespace FakeStoreApp.Models
{
    public interface IProductRepo
    {

        //void Add<T>(T item) where T : class;

        //void Delete<T>(T item) where T : class;

        Task<IEnumerable<ProductModel>> GetAllProductsAsync();

        Task<ProductModel?> GetProductByIdAsync(int id);

        Task<IEnumerable<string>> GetCategoriesAsync();

        Task<IEnumerable<ProductModel>> GetByCategoryAsync(string category);
    }
}
