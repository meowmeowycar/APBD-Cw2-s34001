# Wypożyczalnia sprzętu

Aplikacja konsolowa w C#  obsługująca system wypożyczania sprzętu na uczelni. Umożliwia rejestrowanie sprzętu, wypożyczanie go użytkownikom, zwroty z naliczaniem kar oraz generowanie raportów.

## Uruchomienie

dotnet run --project APBD2

### Odpowiedzialność

Projekt jest podzielony na dwie warstwy:

- Models - czyste klasy domenowe, przechowują dane i proste reguły. 
- Services - logika biznesowa, każdy serwis odpowiada za jeden obszar:
  - `UserService` - tylko operacje na użytkownikach
  - `EquipmentService` - tylko operacje na sprzęcie
  - `RentalService` - logika wypożyczeń i zwrotów
  - `ReportService` - generowanie raportów, nie modyfikuje danych

Taki podział sprawia, że zmiana np. sposobu naliczania kar wymaga edycji tylko `RentalService`, a dodanie nowego typu sprzętu tylko nowej klasy dziedziczącej po `EQ`.

### Dziedziczenie

- `EQ` -> `Laptop` `Projector` `Camera` - wspólne cechy w klasie bazowej, specyficzne pola w klasach potomnych.
- `User` -> `Student` `Employee` - każdy typ definiuje własny limit wypożyczeń i typ użytkownika. Dzięki temu `RentalService` nie musi wiedzieć z jakim typem użytkownika pracuje tylko po prostu sprawdza `user.MaxRentals`.

### Obsługa błędów

Zamiast zwracać kody błędów, używam dedykowanych wyjątków. Rzuca wyjątek w momencie naruszenia zasady.

### Coupling

Serwisy nie tworzą danych samodzielnie tylko otrzymują współdzielone kolekcje przez konstruktor. `RentalService` nie szuka użytkowników sam tylko daje to do `UserService`. Dzięki temu każdy serwis można łatwo podmienić.
