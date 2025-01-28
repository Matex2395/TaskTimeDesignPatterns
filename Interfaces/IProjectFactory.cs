using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TaskTimeDesignPatterns.Interfaces
{
    public interface IProjectFactory
    {
        TaskTimePredicter.Models.Project CreateProject(string name, string? description);
    }
}
