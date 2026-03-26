using APBD2.Models;

namespace APBD2.Services;

public class UserService {
    private readonly List<User> _users;

    public UserService(List<User> users) {
        _users = users;
    }

    public void AddUser(User user) => _users.Add(user);

    public User GetUserById(string userId) =>
        _users.FirstOrDefault(u => u.Id == userId)
        ?? throw new ArgumentException("Nie znaleziono użytkownika.");

    public IEnumerable<User> GetAllUsers() => _users;
}
