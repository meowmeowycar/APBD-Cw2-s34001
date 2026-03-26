namespace APBD2.Models;

public class Camera : EQ {
    public double MegaPixels { get; }
    public bool HasVideoRecording { get; }

    public Camera(string name, double megaPixels, bool hasVideoRecording) : base(name) {
        MegaPixels = megaPixels;
        HasVideoRecording = hasVideoRecording;
    }

    public override string ToString() {
        return $"{base.ToString()}, MP: {MegaPixels}, Video: {(HasVideoRecording ? "Tak" : "Nie")}";
    }
}
