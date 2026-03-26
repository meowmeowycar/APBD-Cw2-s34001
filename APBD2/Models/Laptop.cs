namespace APBD2.Models;

public class Laptop : EQ {
    public int RamGB { get; }
    public string Processor { get; }

    public Laptop(string name, int ramGB, string processor) : base(name) {
        RamGB = ramGB;
        Processor = processor;
    }

    public override string ToString() {
        return $"{base.ToString()}, RAM: {RamGB}GB, CPU: {Processor}";
    }
}
