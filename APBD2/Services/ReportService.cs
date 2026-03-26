using APBD2.Models;

namespace APBD2.Services;

public class ReportService {
    private readonly List<User> _users;
    private readonly List<EQ> _equipment;
    private readonly List<Rental> _rentals;

    public ReportService(List<User> users, List<EQ> equipment, List<Rental> rentals) {
        _users = users;
        _equipment = equipment;
        _rentals = rentals;
    }

    public IEnumerable<Rental> GetOverdueRentals(DateTime today) =>
        _rentals.Where(r => r.IsActive && r.IsOverdue(today));

    public string GenerateReport(DateTime today) {
        var totalEquipment = _equipment.Count;
        var available = _equipment.Count(e => e.Status == StatusEq.Available);
        var rented = _equipment.Count(e => e.Status == StatusEq.Rented);
        var unavailable = _equipment.Count(e => e.Status == StatusEq.Unavailable);
        var activeRentals = _rentals.Count(r => r.IsActive);
        var overdueRentals = _rentals.Count(r => r.IsActive && r.IsOverdue(today));
        var totalLateFees = _rentals.Sum(r => r.LateFee);

        return $"""
            === RAPORT WYPOŻYCZALNI ===
            Sprzęt łącznie: {totalEquipment}
              - Dostępny: {available}
              - Wypożyczony: {rented}
              - Niedostępny: {unavailable}
            Użytkownicy: {_users.Count}
            Aktywne wypożyczenia: {activeRentals}
            Przeterminowane: {overdueRentals}
            Suma naliczonych kar: {totalLateFees:C}
            ==========================
            """;
    }
}
