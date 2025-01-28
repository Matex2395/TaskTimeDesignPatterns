using Microsoft.EntityFrameworkCore;
using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;

namespace TaskTimeDesignPatterns.Commands
{
    public class CreateSubcategoryCommand : ICustomCommands
    {
        private readonly AppDbContext _context;
        private readonly string _name;
        private readonly string? _description;
        private readonly int? _categoryId;

        public CreateSubcategoryCommand(AppDbContext context, string name, string? description, int? categoryId)
        {
            _context = context;
            _name = name;
            _description = description;
            _categoryId = categoryId;
        }
        public async Task ExecuteAsync()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new ArgumentException("El nombre de la subcategoría es obligatorio.");
            }

            var subcategory = new Subcategory
            {
                SubcategoryName = _name,
                SubcategoryDescription = _description,
                CategoryId = _categoryId
            };

            if (_categoryId.HasValue)
            {
                subcategory.Category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == _categoryId.Value);
                if (subcategory.Category == null)
                {
                    throw new InvalidOperationException("La categoría especificada no existe.");
                }
            }

            _context.Subcategories.Add(subcategory);
            await _context.SaveChangesAsync();
        }
    }
}
