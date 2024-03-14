using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaPaginacionDept.Models;
using PracticaPaginacionDept.Repositories;

namespace PracticaPaginacionDept.Controllers
{
    public class DepartamentoController : Controller
    {
        private RepositoryDepartamento repo;
        public DepartamentoController(RepositoryDepartamento repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Departamentos()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
        public async Task<IActionResult> EmpleadosDepartamento
        (int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                //POSICION PARA EL EMPLEADO
                posicion = 1;
            }
            EmpleadoPaginacion model = await
                this.repo.GetEmpleadoDepartamentoAsync
                (posicion.Value, iddepartamento);
            Departamento departamento =
                await this.repo.FindDepartamentoAsync(iddepartamento);
            ViewData["DEPARTAMENTOSELECCIONADO"] = departamento;
            ViewData["REGISTROS"] = model.Registros;
            ViewData["DEPARTAMENTO"] = iddepartamento;
            int siguiente = posicion.Value + 1;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > model.Registros)
            {
                //EFECTO OPTICO
                siguiente = model.Registros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.Registros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return View(model.Empleado);
        }


        public async Task<IActionResult> _PaginacionPartial
(int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            EmpleadoPaginacion model = await
                this.repo.GetEmpleadoDepartamentoAsync
                (posicion.Value, iddepartamento);
            Departamento departamento =
                await this.repo.FindDepartamentoAsync(iddepartamento);
            ViewData["DEPARTAMENTOSELECCIONADO"] = departamento;
            ViewData["REGISTROS"] = model.Registros;
            ViewData["DEPARTAMENTO"] = iddepartamento;
            int siguiente = posicion.Value + 1;
            if (siguiente > model.Registros)
            {
                siguiente = model.Registros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.Registros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return PartialView("_PaginacionPartial", model.Empleado);
        }
    }
}
