namespace Budgets.DataBase;

sealed class BudgetsDataBaseException: Exception
{
    public BudgetsDataBaseException() : base()
    {
    }

    public BudgetsDataBaseException(string message) : base(message)
    {
    }

    public BudgetsDataBaseException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
}