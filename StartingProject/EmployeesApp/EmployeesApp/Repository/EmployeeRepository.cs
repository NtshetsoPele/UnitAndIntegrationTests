using EmployeesApp.Contracts;
using EmployeesApp.Models;

namespace EmployeesApp.Repository
{
	public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context) =>
            _context = context;

        public IEnumerable<Employee> GetAll() => 
            _context.Set<Employee>().ToList();

        public Employee? GetEmployee(Guid id) => 
            _context.Set<Employee>().SingleOrDefault(e => e.Id.Equals(id));

        public void CreateEmployee(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();
        }
    }
}
