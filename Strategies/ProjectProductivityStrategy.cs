using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;

namespace TaskTimeDesignPatterns.Strategies
{
    public class ProjectProductivityStrategy : IAnalysisStrategy
    {
        public object Analyze(AppDbContext context)
        {
            var proyectos = context.Projects.ToList();
            var resultadosProyectos = new List<object>();

            foreach (var proyecto in proyectos)
            {
                var tareasProyecto = context.Quests.Where(q => q.ProjectId == proyecto.ProjectId).ToList();
                double totalTiempoEstimado = 0;
                double totalTiempoReal = 0;

                foreach (var tarea in tareasProyecto)
                {
                    totalTiempoEstimado += tarea.EstimatedTime;
                    totalTiempoReal += tarea.ActualTime ?? 0;
                }

                if (totalTiempoEstimado > 0)
                {
                    double eficiencia = totalTiempoEstimado / totalTiempoReal * 100;
                    resultadosProyectos.Add(new
                    {
                        Proyecto = proyecto.ProjectName,
                        TiempoEstimadoTotal = totalTiempoEstimado,
                        TiempoRealTotal = totalTiempoReal,
                        Eficiencia = eficiencia
                    });
                }
            }

            return resultadosProyectos;
        }
    }
}
