using APBD2.Models;

namespace APBD2.Services;

public class RentalService {
    private readonly List<User> _users = new();
    private readonly List<EQ> _equipment = new();
    private readonly List<Rental> _rentals = new();
    
    private const decimal DailyLateFee = 10.0m;

    public void AddUser(User user) => _users.Add(user);
    
    public void AddEquipment(EQ equipment) => _equipment.Add(equipment);

    public Rental RentEquipment(string userId, string equipmentId, int days) {
        var user = _users.FirstOrDefault(u => u.Id.ToString("N") == userId || u.Id.ToString() == userId) 
                   ?? throw new ArgumentException("Nie znaleziono użytkownika.");
                   
        var equipment = _equipment.FirstOrDefault(e => e.Id.ToString("N") == equipmentId || e.Id.ToString() == equipmentId) 
                        ?? throw new ArgumentException("Nie znaleziono sprzętu.");
        
        if (equipment.Status != StatusEq.Available) {
            throw new EquipmentUnavailableException(equipmentId);
        }

        var activeUserRentalsCount = _rentals.Count(r => r.UserId == userId && r.IsActive);
        if (activeUserRentalsCount >= user.MaxRentals) {
            throw new UserLimitExceededException(userId, user.MaxRentals);
        }

        var rental = new Rental(userId, equipmentId, DateTime.Now, days);
        _rentals.Add(rental);

        equipment.Status = StatusEq.Rented;

        return rental;
    }

    public void ReturnEquipment(string rentalId, DateTime returnDate) {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId) 
                     ?? throw new ArgumentException("Nie znaleziono wypożyczenia.");

        if (!rental.IsActive) {
            throw new InvalidOperationException("To wypożyczenie zostało już zakończone.");
        }
        decimal fee = 0;
        if (returnDate.Date > rental.DueDate.Date) {
            var daysLate = (returnDate.Date - rental.DueDate.Date).Days;
            fee = daysLate * DailyLateFee;
        }
        
        rental.Return(returnDate, fee);
        
        var equipment = _equipment.First(e => e.Id.ToString("N") == rental.EquipmentId || e.Id.ToString() == rental.EquipmentId);
        equipment.Status = StatusEq.Available;
    }
    

    public void MarkEquipmentUnavailable(string equipmentId) {
        var equipment = _equipment.FirstOrDefault(e => e.Id.ToString("N") == equipmentId || e.Id.ToString() == equipmentId)
                        ?? throw new ArgumentException("Nie znaleziono sprzętu.");

        if (equipment.Status == StatusEq.Rented) {
            throw new InvalidOperationException("Nie można oznaczyć wypożyczonego sprzętu jako niedostępny.");
        }

        equipment.Status = StatusEq.Unavailable;
    }

    public IEnumerable<EQ> GetAllEquipment() => _equipment;
    public IEnumerable<EQ> GetAvailableEquipment() => _equipment.Where(e => e.Status == StatusEq.Available);
    public IEnumerable<Rental> GetActiveRentalsForUser(string userId) => _rentals.Where(r => r.UserId == userId && r.IsActive);

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