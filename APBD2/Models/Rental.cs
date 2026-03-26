namespace APBD2.Models;

public sealed class Rental {
    public Rental(string userId, string equipmentId, DateTime rentedAt, int rentalDays) {
        Id = Guid.NewGuid().ToString("N");
        UserId = userId;
        EquipmentId = equipmentId;
        RentedAt = rentedAt;
        RentalDays = rentalDays;
        DueDate = rentedAt.AddDays(rentalDays);
    }

    public string Id { get; }
    public string UserId { get; }
    public string EquipmentId { get; }
    public DateTime RentedAt { get; }
    public int RentalDays { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal LateFee { get; private set; }

    public bool IsActive => ReturnedAt is null;
    public bool IsOverdue(DateTime today) => IsActive && today.Date > DueDate.Date;

    public void Return(DateTime returnDate, decimal lateFee) {
        if (ReturnedAt is not null) {
            throw new InvalidOperationException("Invalid state!");
        }

        ReturnedAt = returnDate;
        LateFee = lateFee;
    }

    public override string ToString() {
        var state = IsActive ? "Active" : $"Returned: {ReturnedAt:yyyy-MM-dd}, Fee: {LateFee:C}";

        return $"Rental [{Id}] user={UserId}, equipment={EquipmentId}, from={RentedAt:yyyy-MM-dd}, till={DueDate:yyyy-MM-dd}, status={state}";
    }
}