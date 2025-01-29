using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;

namespace TaskTimeDesignPatterns.Strategies
{
    public class CategoryTimeStrategy : IAnalysisStrategy
    {
        public object Analyze(AppDbContext context)
        {
            var categorias = context.Categories
                .Select(c => new
                {
                    CategoriaId = c.CategoryId,
                    CategoriaName = c.CategoryName,
                    Subcategorias = context.Subcategories
                        .Where(s => s.CategoryId == c.CategoryId)
                        .Select(s => new
                        {
                            SubcategoryId = s.SubcategoryId,
                            SubcategoryName = s.SubcategoryName
                        }).ToList()
                }).ToList();

            var resultadosCategorias = new List<object>();

            foreach (var categoria in categorias)
            {
                if (!categoria.Subcategorias.Any())
                {
                    resultadosCategorias.Add(new
                    {
                        Categoria = categoria.CategoriaName,
                        Subcategoria = "N/A",
                        TiempoPromedioEstimado = 0,
                        TiempoPromedioReal = 0
                    });
                }
                else
                {
                    foreach (var subcategoria in categoria.Subcategorias)
                    {
                        var tareasSubcategoria = context.Quests
                            .Where(q => q.SubcategoryId == subcategoria.SubcategoryId)
                            .ToList();

                        double totalTiempoReal = 0;
                        double totalTiempoEstimado = 0;
                        int count = 0;

                        foreach (var tarea in tareasSubcategoria)
                        {
                            totalTiempoEstimado += tarea.EstimatedTime;
                            if (tarea.ActualTime.HasValue)
                            {
                                totalTiempoReal += tarea.ActualTime.Value;
                                count++;
                            }
                        }

                        if (count > 0)
                        {
                            double tiempoPromedioReal = totalTiempoReal / count;
                            double tiempoPromedioEstimado = totalTiempoEstimado / count;
                            resultadosCategorias.Add(new
                            {
                                Categoria = categoria.CategoriaName,
                                Subcategoria = subcategoria.SubcategoryName,
                                TiempoPromedioEstimado = tiempoPromedioEstimado,
                                TiempoPromedioReal = tiempoPromedioReal
                            });
                        }
                        else
                        {
                            resultadosCategorias.Add(new
                            {
                                Categoria = categoria.CategoriaName,
                                Subcategoria = subcategoria.SubcategoryName,
                                TiempoPromedioEstimado = 0,
                                TiempoPromedioReal = 0
                            });
                        }
                    }
                }
            }

            return resultadosCategorias;
        }
    }
}
