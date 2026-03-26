namespace APBD2.Models;

public abstract class EQ
{
    public Guid Id { get; } = Guid.NewGuid();
    public StatusEq Status { get; set; } = StatusEq.Available;
    
    public string Name { get; }

    protected EQ(string name)
    {
        Name = name;
    }
}