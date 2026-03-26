using APBD2.Models;

namespace APBD2.Services;

public class RentalService {
    private readonly List<Rental> _rentals;
    private readonly UserService _userService;
    private readonly EquipmentService _equipmentService;

    private const decimal DailyLateFee = 10.0m;

    public RentalService(List<Rental> rentals, UserService userService, EquipmentService equipmentService) {
        _rentals = rentals;
        _userService = userService;
        _equipmentService = equipmentService;
    }

    public Rental RentEquipment(string userId, string equipmentId, int days) {
        var user = _userService.GetUserById(userId);
        var equipment = _equipmentService.GetEquipmentById(equipmentId);

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

        var equipment = _equipmentService.GetEquipmentById(rental.EquipmentId);
        equipment.Status = StatusEq.Available;
    }

    public IEnumerable<Rental> GetActiveRentalsForUser(string userId) =>
        _rentals.Where(r => r.UserId == userId && r.IsActive);
}
