using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaPaginacionDept.Data;
using PracticaPaginacionDept.Models;
using System.Data;

namespace PracticaPaginacionDept.Repositories
{
    public class RepositoryDepartamento
    {
        private DepartamentoContext context;
        public RepositoryDepartamento(DepartamentoContext context)
        {
            this.context = context;
        }
        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            return await this.context.Departamentos.ToListAsync();
        }
        public async Task<Departamento> FindDepartamentoAsync(int id)
        {
            return await this.context.Departamentos
                .FirstOrDefaultAsync(x => x.IdDepartamento == id);
        }

        #region PROCEDIMIENTO ALMACENADO
        /*        
        create procedure SP_MODEL_EMPLEADO_DEPARTAMENTO
        (@posicion int, @departamento int
        , @registros int out)
        as
        select @registros = count(EMP_NO) from EMP
        where DEPT_NO = @departamento
        select EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from
            (select cast(
            ROW_NUMBER() OVER (ORDER BY APELLIDO) as int) AS POSICION
            , EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
            from EMP
            where DEPT_NO = @departamento) as QUERY
            where QUERY.POSICION = @posicion
        go*/
        #endregion
        public async Task<EmpleadoPaginacion>
           GetEmpleadoDepartamentoAsync
           (int posicion, int iddepartamento)
        {
            string sql = "SP_EMPLEADO_DEPARTAMENTO @posicion, @departamento, "
                + " @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamDepartamento =
                new SqlParameter("@departamento", iddepartamento);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;

            var consulta =
                this.context.Empleados.FromSqlRaw
                (sql, pamPosicion, pamDepartamento, pamRegistros);
            var datos = await consulta.ToListAsync();
            Empleado empleado = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new EmpleadoPaginacion
            {
                Registros = registros,
                Empleado = empleado
            };
        }
    }
}
