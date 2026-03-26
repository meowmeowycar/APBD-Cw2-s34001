namespace APBD2.Models;

public class Projector : EQ {
    public int LumensOutput { get; }
    public string Resolution { get; }

    public Projector(string name, int lumensOutput, string resolution) : base(name) {
        LumensOutput = lumensOutput;
        Resolution = resolution;
    }

    public override string ToString() {
        return $"{base.ToString()}, Lumens: {LumensOutput}, Resolution: {Resolution}";
    }
}
