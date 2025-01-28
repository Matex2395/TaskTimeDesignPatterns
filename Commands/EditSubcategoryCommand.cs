using Microsoft.EntityFrameworkCore;
using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;

namespace TaskTimeDesignPatterns.Commands
{
    public class EditSubcategoryCommand : ICustomCommands
    {
        private readonly AppDbContext _context;
        private readonly int _id;
        private readonly string _name;
        private readonly string? _description;
        private readonly int? _categoryId;

        public EditSubcategoryCommand(AppDbContext context, int id, string name, string? description, int? categoryId)
        {
            _context = context;
            _id = id;
            _name = name;
            _description = description;
            _categoryId = categoryId;
        }

        public async Task ExecuteAsync()
        {
            var subcategory = await _context.Subcategories.FindAsync(_id);
            if (subcategory == null)
            {
                throw new InvalidOperationException("La subcategoría no existe.");
            }

            subcategory.SubcategoryName = _name;
            subcategory.SubcategoryDescription = _description;
            subcategory.CategoryId = _categoryId;

            if (_categoryId.HasValue)
            {
                subcategory.Category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == _categoryId.Value);
                if (subcategory.Category == null)
                {
                    throw new InvalidOperationException("La categoría especificada no existe.");
                }
            }

            _context.Subcategories.Update(subcategory);
            await _context.SaveChangesAsync();
        }
    }
}
