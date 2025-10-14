using MinBudget.Domain;
using MinBudget.Service;

namespace MinBudget.Domain
{
    public class BudgetManager
    {
        private readonly LocalStorageService _localStorage;
        public List<Income> Incomes { get; private set; } = new();
        public List<Expense> Expenses { get; private set; } = new();

        public BudgetManager(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task LoadAsync()
        {
            Incomes = await _localStorage.ReadListAsync<Income>("incomes");
            Expenses = await _localStorage.ReadListAsync<Expense>("expenses");
        }

        public async Task AddIncomeAsync(Income income)
        {
            Incomes.Add(income);
            await _localStorage.SaveListAsync("incomes", Incomes);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            Expenses.Add(expense);
            await _localStorage.SaveListAsync("expenses", Expenses);
        }

        public async Task RemoveIncomeAsync(int index)
        {
            if (index >= 0 && index < Incomes.Count)
            {
                Incomes.RemoveAt(index);
                await _localStorage.SaveListAsync("incomes", Incomes);
            }
        }

        public async Task RemoveExpenseAsync(int index)
        {
            if (index >= 0 && index < Expenses.Count)
            {
                Expenses.RemoveAt(index);
                await _localStorage.SaveListAsync("expenses", Expenses);
            }
        }

        public async Task SaveIncomeNoteAsync(int index)
        {
            if (index >= 0 && index < Incomes.Count)
            {
                await _localStorage.SaveListAsync("incomes", Incomes);
            }
        }

        public async Task SaveExpenseNoteAsync(int index)
        {
            if (index >= 0 && index < Expenses.Count)
            {
                await _localStorage.SaveListAsync("expenses", Expenses);
            }
        }

        public decimal TotalIncome() => Incomes.Sum(i => i.Amount);
        public decimal TotalExpense() => Expenses.Sum(e => e.Amount);
        public decimal LeftThisMonth() => TotalIncome() - TotalExpense();
    }
}
