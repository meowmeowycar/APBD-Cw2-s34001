namespace APBD2.Models;

public class Employee : User {
    public Employee(string firstName, string lastName) : base(firstName, lastName) { }

    public override UserType UserType => UserType.Employee;
    public override int MaxRentals => 5;
}
