namespace APBD2.Models;

public class Student : User {
    public Student(string firstName, string lastName) : base(firstName, lastName) { }

    public override UserType UserType => UserType.Student;
    public override int MaxRentals => 2;
}
