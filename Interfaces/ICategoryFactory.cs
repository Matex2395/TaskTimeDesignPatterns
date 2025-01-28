using TaskTimePredicter.Models;

namespace TaskTimeDesignPatterns.Interfaces
{
    public interface ICategoryFactory
    {
        Category CreateCategory(string name, string? description);
    }
}
