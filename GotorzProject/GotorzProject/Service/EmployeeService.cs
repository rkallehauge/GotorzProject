using GotorzProject.Model;
using GotorzProject.Model.ObjectRelationMapping;
using Microsoft.EntityFrameworkCore;

namespace GotorzProject.Client.Service
{
    public class EmployeeService
    {
        private readonly DbSet<Employee> _employees;

        public EmployeeService(ApplicationDbContext DBContext)
        {
            _employees = DBContext.Employees;
        }

        public List<Employee> GetAllEmployees() => new(_employees);

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public void DeleteEmployee(int id)
        {
            _employees.Remove(_employees.FirstOrDefault(e => e.ID == id));
        }

    }
}
