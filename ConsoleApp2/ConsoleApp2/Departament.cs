using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;

internal class Departament : Company
{
    private readonly string Name;
    private readonly string Description;
    private readonly double Cost;
    private List<Company> employees = new List<Company>();

    public Departament(string name, string description, double cost)
    {
        this.Name = name;
        this.Description = description;
        this.Cost = cost;
    }

    public void AddEmployee(Company employee)
    {
        employees.Add(employee);
    }

    public void RemoveEmployee(Company employee)
    {
        employees.Remove(employee);
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return Description;
    }

    public double CalculateCost()
    {
        return Cost + employees.Sum(e => e.CalculateCost());
    }
}
