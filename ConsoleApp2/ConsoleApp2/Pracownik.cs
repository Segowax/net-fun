namespace ConsoleApp2;

internal class Pracownik : Company
{
    private readonly string Name;
    private readonly string Description;
    private readonly double Cost;

    public Pracownik(string name, string description, double cost)
    {
        this.Name = name;
        this.Description = description;
        this.Cost = cost;
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
        return Cost;
    }

}
