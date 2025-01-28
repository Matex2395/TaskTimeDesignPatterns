using Microsoft.Build.Evaluation;
using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;

namespace TaskTimeDesignPatterns.Factories
{
    public class ProjectFactory : IProjectFactory
    {
        private readonly AppDbContext _context;

        public ProjectFactory(AppDbContext context)
        {
            _context = context;
        }

        TaskTimePredicter.Models.Project IProjectFactory.CreateProject(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre es obligatorio", nameof(name));
            }

            var project = new TaskTimePredicter.Models.Project
            {
                ProjectName = name,
                ProjectDescription = description
            };

            _context.Add(project);
            return project;
        }
    }
}
