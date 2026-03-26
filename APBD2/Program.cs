using APBD2.Models;
using APBD2.Services;

// Współdzielone kolekcje danych
var users = new List<User>();
var equipment = new List<EQ>();
var rentals = new List<Rental>();

// Inicjalizacja serwisów
var userService = new UserService(users);
var equipmentService = new EquipmentService(equipment);
var rentalService = new RentalService(rentals, userService, equipmentService);
var reportService = new ReportService(users, equipment, rentals);


Console.WriteLine("=== Dodanie sprzętu ===");
var laptop1 = new Laptop("Dell XPS 15", 16, "Intel i7");
var laptop2 = new Laptop("MacBook Pro", 32, "Apple M3");
var projector1 = new Projector("Epson EB-X51", 3800, "1024x768");
var camera1 = new Camera("Canon EOS R6", 20.1, true);
var camera2 = new Camera("GoPro Hero 12", 27.0, true);

equipmentService.AddEquipment(laptop1);
equipmentService.AddEquipment(laptop2);
equipmentService.AddEquipment(projector1);
equipmentService.AddEquipment(camera1);
equipmentService.AddEquipment(camera2);

Console.WriteLine($"Dodano: {laptop1}");
Console.WriteLine($"Dodano: {laptop2}");
Console.WriteLine($"Dodano: {projector1}");
Console.WriteLine($"Dodano: {camera1}");
Console.WriteLine($"Dodano: {camera2}");


Console.WriteLine("\n=== Dodanie użytkowników ===");
var student1 = new Student("Jan", "Kowalski");
var student2 = new Student("Anna", "Nowak");
var employee1 = new Employee("Piotr", "Wiśniewski");

userService.AddUser(student1);
userService.AddUser(student2);
userService.AddUser(employee1);

Console.WriteLine($"Dodano studenta: {student1} (max wypożyczeń: {student1.MaxRentals})");
Console.WriteLine($"Dodano studenta: {student2} (max wypożyczeń: {student2.MaxRentals})");
Console.WriteLine($"Dodano pracownika: {employee1} (max wypożyczeń: {employee1.MaxRentals})");


Console.WriteLine("\n===  Wypożyczenie sprzętu ===");
var rental1 = rentalService.RentEquipment(student1.Id, laptop1.Id.ToString("N"), 7);
Console.WriteLine($"{student1} wypożyczył {laptop1.Name} na 7 dni");

var rental2 = rentalService.RentEquipment(student1.Id, camera1.Id.ToString("N"), 3);
Console.WriteLine($"{student1} wypożyczył {camera1.Name} na 3 dni");

var rental3 = rentalService.RentEquipment(employee1.Id, projector1.Id.ToString("N"), 14);
Console.WriteLine($"{employee1} wypożyczył {projector1.Name} na 14 dni");


Console.WriteLine("\n===  Próba niepoprawnych operacji ===");


try {
    rentalService.RentEquipment(student1.Id, laptop2.Id.ToString("N"), 5);
} catch (UserLimitExceededException ex) {
    Console.WriteLine($"Błąd: {ex.Message}");
}


try {
    rentalService.RentEquipment(student2.Id, laptop1.Id.ToString("N"), 5);
} catch (EquipmentUnavailableException ex) {
    Console.WriteLine($"Błąd: {ex.Message}");
}


equipmentService.MarkAsUnavailable(laptop2.Id.ToString("N"));
Console.WriteLine($"\n{laptop2.Name} oznaczony jako niedostępny (serwis)");

try {
    rentalService.RentEquipment(employee1.Id, laptop2.Id.ToString("N"), 3);
} catch (EquipmentUnavailableException ex) {
    Console.WriteLine($"Błąd: {ex.Message}");
}


Console.WriteLine("\n===Dostępny sprzęt ===");
foreach (var eq in equipmentService.GetAvailableEquipment()) {
    Console.WriteLine($"  - {eq}");
}


Console.WriteLine($"\n=== Aktywne wypożyczenia: {student1} ===");
foreach (var r in rentalService.GetActiveRentalsForUser(student1.Id)) {
    Console.WriteLine($"  - {r}");
}


Console.WriteLine("\n===Zwrot w terminie ===");
rentalService.ReturnEquipment(rental1.Id, DateTime.Now.AddDays(5));
Console.WriteLine($"{student1} zwrócił {laptop1.Name} w terminie - brak kary");


Console.WriteLine("\n=== Zwrot opóźniony ===");
rentalService.ReturnEquipment(rental2.Id, DateTime.Now.AddDays(10));
var daysLate = (DateTime.Now.AddDays(10).Date - rental2.DueDate.Date).Days;
Console.WriteLine($"{student1} zwrócił {camera1.Name} z opóźnieniem {daysLate} dni - kara: {rental2.LateFee:C}");


Console.WriteLine("\n=== Przeterminowane wypożyczenia ===");
var overdueDate = DateTime.Now.AddDays(30);
var overdue = reportService.GetOverdueRentals(overdueDate).ToList();
if (overdue.Any()) {
    foreach (var r in overdue) {
        Console.WriteLine($"  - {r}");
    }
} else {
    Console.WriteLine("  Brak przeterminowanych wypożyczeń.");
}


Console.WriteLine("\n=== Raport końcowy ===");
Console.WriteLine(reportService.GenerateReport(DateTime.Now));
