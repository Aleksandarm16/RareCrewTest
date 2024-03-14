using RareCrewTest.Models;
using System.Text.Json;

namespace RareCrewTest.Services
{
    public class EmpolyeeService
    {

        public async Task<List<EmployeeWorkHoursSum>> GetEmployeesWorkHours()
        {
            try
            {
                // Fetch employees data
                List<Employee> employees = await GetEmployees();

                // Calculate hours worked
                List<EmployeeWorkHoursSum> employeeWorkHoursSum = CalculateHoursWorked(employees);

                return employeeWorkHoursSum;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error retrieving employee work hours: " + ex.Message, ex);
            }
        }


        private async Task<List<Employee>> GetEmployees()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://rc-vault-fap-live-1.azurewebsites.net/");
                    HttpResponseMessage response = await httpClient.GetAsync("api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==");

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Employee>? employees = JsonSerializer.Deserialize<List<Employee>>(responseBody);

                    return employees;
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error retrieving employees: " + ex.Message, ex);
            }
        }
        private List<EmployeeWorkHoursSum> CalculateHoursWorked(List<Employee> employee)
        {
            var employeeHours = employee
                .Where(e => e.EmployeeName != null)
                .GroupBy(entry => entry.EmployeeName)
                .Select(group => new EmployeeWorkHoursSum
                {
                    EmployeeName = group.Key,
                    HoursWorked = Math.Round(group.Sum(entry => (entry.EndTimeUtc - entry.StarTimeUtc).TotalHours), 2)
                })
                .OrderBy(e => e.EmployeeName)
                .ToList();

            return employeeHours;
        }
    }
}
