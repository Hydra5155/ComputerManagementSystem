using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> c185de045bf346af6557941082a4a8df0f427ebc

namespace ComputerStore.Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Specifications { get; set; }
<<<<<<< HEAD

        // Alias tương thích ngược: tự động trỏ về Specifications mà không tạo cột mới trong DB
        [NotMapped]
        public string? Description
        {
            get => Specifications;
            set => Specifications = value;
        }
=======
>>>>>>> c185de045bf346af6557941082a4a8df0f427ebc

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}