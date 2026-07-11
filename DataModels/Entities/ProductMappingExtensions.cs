using DataModels.Dtos;

namespace DataModels.Entities
{
    public static class ProductMappingExtensions
    {
        public static ProductDto ToDto(this Product product) =>
            new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

        public static Product ToEntity(this ProductRequestDto dto) =>
            new()
            {
                Name = dto.Name,
                Price = dto.Price
            };

        public static void ApplyTo(this ProductRequestDto dto, Product product)
        {
            product.Name = dto.Name;
            product.Price = dto.Price;
        }
    }
}
