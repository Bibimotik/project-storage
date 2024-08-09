using System.Data;
using System.Windows;

using application.Abstraction;

namespace application.Utilities;

public static class RepositoryHelper
{
    public static async Task<T> ExecuteWithErrorHandling<T>(Func<IDbConnection, Task<T>> func, IDatabaseService databaseService)
    {
        try
        {
            using IDbConnection dbConnection = databaseService.CreateConnection();
            return await func(dbConnection);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
            throw;
        }
    }
}
