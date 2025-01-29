using TaskTimePredicter.Data;

namespace TaskTimeDesignPatterns.Interfaces
{
    public interface IAnalysisStrategy
    {
        object Analyze(AppDbContext context);
    }
}
