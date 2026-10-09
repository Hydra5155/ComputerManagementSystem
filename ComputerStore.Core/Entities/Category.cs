namespace ComputerStore.Core.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Quan hệ 1 danh mục có nhiều sản phẩm
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}