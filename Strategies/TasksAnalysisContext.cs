using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;

namespace TaskTimeDesignPatterns.Strategies
{
    public class TasksAnalysisContext
    {
        private IAnalysisStrategy _strategy;

        public void SetStrategy(IAnalysisStrategy strategy)
        {
            _strategy = strategy;
        }

        public object ExecuteStrategy(AppDbContext context)
        {
            return _strategy.Analyze(context);
        }
    }
}
