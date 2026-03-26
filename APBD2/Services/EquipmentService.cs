using APBD2.Models;

namespace APBD2.Services;

public class EquipmentService {
    private readonly List<EQ> _equipment;

    public EquipmentService(List<EQ> equipment) {
        _equipment = equipment;
    }

    public void AddEquipment(EQ equipment) => _equipment.Add(equipment);

    public EQ GetEquipmentById(string equipmentId) =>
        _equipment.FirstOrDefault(e => e.Id.ToString("N") == equipmentId || e.Id.ToString() == equipmentId)
        ?? throw new ArgumentException("Nie znaleziono sprzętu.");

    public void MarkAsUnavailable(string equipmentId) {
        var equipment = GetEquipmentById(equipmentId);

        if (equipment.Status == StatusEq.Rented) {
            throw new InvalidOperationException("Nie można oznaczyć wypożyczonego sprzętu jako niedostępny.");
        }

        equipment.Status = StatusEq.Unavailable;
    }

    public IEnumerable<EQ> GetAllEquipment() => _equipment;
    public IEnumerable<EQ> GetAvailableEquipment() => _equipment.Where(e => e.Status == StatusEq.Available);
}
