// Importerar nödvändiga klasser från andra delar av projektet.
using MinBudget.Domain;
using MinBudget.Service;

namespace MinBudget.Domain
{
    // BudgetManager är huvudklassen för att hantera all budgetlogik.
    // Den ansvarar för att ladda, spara, lägga till och ta bort inkomster och utgifter.
    public class BudgetManager
    {
        // En privat referens till LocalStorageService, som används för att spara och hämta data från webbläsarens lokala lagring.
        private readonly LocalStorageService _localStorage;
        
        // En offentlig lista över inkomster. `private set` innebär att listan bara kan ändras inifrån denna klass.
        public List<Income> Incomes { get; private set; } = new();
        
        // En offentlig lista över utgifter.
        public List<Expense> Expenses { get; private set; } = new();
        
        // Privata fält för att hålla det valda året och månaden. Startvärdena är nuvarande år och månad.
        private int _year = DateTime.Now.Year;
        private int _month = DateTime.Now.Month;

        // Dynamiskt genererade nycklar för att spara och hämta data i local storage.
        // Nycklarna är unika för varje månad och år, t.ex. "incomes-2023-10".
        private string IncomeKey => $"incomes-{_year}-{_month:D2}";
        private string ExpenseKey => $"expenses-{_year}-{_month:D2}";

        // Konstruktorn tar emot en instans av LocalStorageService (via dependency injection).
        public BudgetManager(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // Asynkron metod för att ladda inkomster och utgifter från local storage baserat på nuvarande _year och _month.
        public async Task LoadAsync()
        {
            Console.WriteLine($"Laddar inkomster med nyckel: {IncomeKey}");
            Console.WriteLine($"Laddar utgifter med nyckel: {ExpenseKey}");
            // Använder _localStorage för att läsa listorna. Om en nyckel inte finns, returneras en tom lista.
            Incomes = await _localStorage.ReadListAsync<Income>(IncomeKey);
            Expenses = await _localStorage.ReadListAsync<Expense>(ExpenseKey);
            Console.WriteLine($"Antal inkomster: {Incomes.Count}");
            Console.WriteLine($"Antal utgifter: {Expenses.Count}");
        }

        // Överlagrad version av LoadAsync som först sätter år och månad innan den laddar datan.
        public async Task LoadAsync(int year, int month)
        {
            _year = year;
            _month = month;
            await LoadAsync();
        }

        // Asynkron metod för att lägga till en ny inkomst i listan och spara den uppdaterade listan till local storage.
        public async Task AddIncomeAsync(Income income)
        {
            Incomes.Add(income);
            await _localStorage.SaveListAsync(IncomeKey, Incomes);
        }

        // Asynkron metod för att lägga till en ny utgift och spara den.
        public async Task AddExpenseAsync(Expense expense)
        {
            Expenses.Add(expense);
            await _localStorage.SaveListAsync(ExpenseKey, Expenses);
        }

        // Asynkron metod för att ta bort en inkomst från listan baserat på dess index.
        public async Task RemoveIncomeAsync(int index)
        {
            // Säkerställer att indexet är giltigt.
            if (index >= 0 && index < Incomes.Count)
            {
                Incomes.RemoveAt(index);
                await _localStorage.SaveListAsync(IncomeKey, Incomes);
            }
        }

        // Asynkron metod för att ta bort en utgift från listan baserat på dess index.
        public async Task RemoveExpenseAsync(int index)
        {
            if (index >= 0 && index < Expenses.Count)
            {
                Expenses.RemoveAt(index);
                await _localStorage.SaveListAsync(ExpenseKey, Expenses);
            }
        }

        // Asynkron metod för att spara ändringar i en specifik inkomst (t.ex. en notering).
        public async Task SaveIncomeNoteAsync(int index)
        {
            if (index >= 0 && index < Incomes.Count)
            {
                // Sparar hela listan igen, eftersom en post i den har ändrats.
                await _localStorage.SaveListAsync(IncomeKey, Incomes);
            }
        }

        // Asynkron metod för att spara ändringar i en specifik utgift.
        public async Task SaveExpenseNoteAsync(int index)
        {
            if (index >= 0 && index < Expenses.Count)
            {
                await _localStorage.SaveListAsync(ExpenseKey, Expenses);
            }
        }

        // Metod som beräknar och returnerar den totala summan av alla inkomster.
        public decimal TotalIncome() => Incomes.Sum(i => i.Amount);
        
        // Metod som beräknar och returnerar den totala summan av alla utgifter.
        public decimal TotalExpense() => Expenses.Sum(e => e.Amount);
        
        // Metod som beräknar och returnerar skillnaden mellan totala inkomster och utgifter.
        public decimal LeftThisMonth() => TotalIncome() - TotalExpense();
    }
}
