namespace ProductCrudApi.Application.DTOs
{
    // नवीन Product बनवताना वापरतो
    public class CreateProductRequestDto
    {
        public string ProductName { get; set; } = string.Empty;
    }

    // Product update करताना वापरतो
    public class UpdateProductRequestDto
    {
        public string ProductName { get; set; } = string.Empty;
    }

    // Item बनवताना/update करताना वापरतो
    public class CreateItemRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateItemRequestDto
    {
        public int Quantity { get; set; }
    }

    // Item response मध्ये दाखवण्यासाठी
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // Product response मध्ये दाखवण्यासाठी (Items सोबत)
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public List<ItemResponseDto> Items { get; set; } = new();
    }
}