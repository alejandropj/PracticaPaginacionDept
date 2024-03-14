using Microsoft.EntityFrameworkCore;
using PracticaPaginacionDept.Models;

namespace PracticaPaginacionDept.Data
{
    public class DepartamentoContext:DbContext
    {
        public DepartamentoContext(DbContextOptions<DepartamentoContext> options)
            :base(options) { }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
    }
}
