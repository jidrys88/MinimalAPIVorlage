namespace DataModels.Dtos
{
    // Was der Client beim Lesen zu sehen bekommt.
    // Bleibt unabhängig davon, wie sich die Product-Entity/DB-Tabelle künftig ändert.
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
