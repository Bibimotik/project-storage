using System.Data;
using Storage.Domain.Interfaces;

namespace Storage.Infrastructure;

public static class RepositoryHelper
{
    public static async Task<T> ExecuteWithErrorHandling<T>(Func<IDbConnection, Task<T>> func,
                                                         IStorageDBContext context,
                                                         CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection dbConnection = await context.CreateConnection(cancellationToken);
            return await func(dbConnection);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при выполнении операции с базой данных: {ex.Message}");
        }
    }
}