namespace APBD2.Models;

public abstract class User {
    protected User(string firstName, string lastName) {
        Id = Guid.NewGuid().ToString("N");
        FirstName = firstName;
        LastName = lastName;
    }

    public string Id { get; set; }
    public string FirstName { get; }
    public string LastName { get; }
    public abstract UserType UserType { get; }
    public abstract int MaxRentals { get; }

    public override string ToString() {
        return $"{FirstName} {LastName}";
    }
}