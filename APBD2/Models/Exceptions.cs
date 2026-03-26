using System;

namespace APBD2.Models;


public abstract class ModelException : Exception {
    protected ModelException(string message) : base(message) { }
}

public class EquipmentUnavailableException : ModelException {
    public EquipmentUnavailableException(string equipmentId) : base($"Sprzęt o ID {equipmentId} jest obecnie niedostępny do wypożyczenia.") { }
}

public class UserLimitExceededException : ModelException {
    public UserLimitExceededException(string userId, int limit) : base($"Użytkownik {userId} przekroczył limit aktywnych wypożyczeń ({limit}).") { }
}