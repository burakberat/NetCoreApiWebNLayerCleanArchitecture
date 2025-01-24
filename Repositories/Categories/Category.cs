using App.Repositories.Products;

namespace App.Repositories.Categories;

public class Category : BaseEntity<int>, IAuditEntity
{
    public string Name { get; set; } = default!;
    public List<Product>? Products { get; set; } //illa bir product'ı olmayabilir. Null olabilir.
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}