using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;

namespace TaskTimeDesignPatterns.Factories
{
    public class CategoryFactory : ICategoryFactory
    {
        private readonly AppDbContext _context;
        public CategoryFactory(AppDbContext context)
        {
            _context = context;
        }
        public Category CreateCategory(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre es obligatorio", nameof(name));
            }

            var category = new Category
            {
                CategoryName = name,
                CategoryDescription = description
            };

            _context.Add(category);
            return category;
        }
    }
}
