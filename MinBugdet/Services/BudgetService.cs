namespace MinBudget.Services;

public class BudgetService
{
    public List<decimal> Incomes { get; } = new();
    public List<decimal> Expenses { get; } = new();

    public decimal TotalIncome => Incomes.Sum();
    public decimal TotalExpense => Expenses.Sum();
    public decimal LeftThisMonth => TotalIncome - TotalExpense;

    public void AddIncome(decimal value) => Incomes.Add(value);
    public void AddExpense(decimal value) => Expenses.Add(value);
}