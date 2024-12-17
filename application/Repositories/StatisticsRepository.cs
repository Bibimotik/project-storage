using application.Abstraction.Interfaces;
using application.Utilities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using application.Abstraction;

namespace application.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDatabaseService _databaseService;

        public StatisticsRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Линейный график: Количество товаров, заказанных по дням
        public async Task<IEnumerable<(DateTime Date, int Count)>> GetOrdersByDateAsync(Guid entityId, DateTime startDate, DateTime endDate)
        {
            const string query = @"
                SELECT 
                    o.application_date AS Date,
                    SUM(e.Count) AS Count
                FROM ""order"" o
                INNER JOIN ENTITY_PRODUCT_ORDER e ON e.Order_ID = o.ID
                WHERE o.Entity_ID = @EntityId
                AND o.Application_Date BETWEEN @StartDate AND @EndDate
                GROUP BY o.application_date
                ORDER BY o.application_date";

            return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
            {
                var result = await dbConnection.QueryAsync<(DateTime Date, int Count)>(query, new { EntityId = entityId, StartDate = startDate, EndDate = endDate });
                return result;
            }, _databaseService);
        }

        // Столбчатая диаграмма: Количество заказанных товаров по категориям
        public async Task<IEnumerable<(string Product, int TotalCount)>> GetProductSalesByCategoryAsync(Guid entityId, DateTime startDate, DateTime endDate)
        {
            const string query = @"
                SELECT 
                    p.Title AS Product,
                    SUM(e.Count) AS TotalCount
                FROM ""order"" o
                INNER JOIN ENTITY_PRODUCT_ORDER e ON e.Order_ID = o.ID
                INNER JOIN product p ON p.ID = e.Product_ID
                WHERE o.Entity_ID = @EntityId
                AND o.Application_Date BETWEEN @StartDate AND @EndDate
                AND p.Is_Deleted = false
                GROUP BY p.Title
                ORDER BY TotalCount DESC";

            return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
            {
                var result = await dbConnection.QueryAsync<(string Product, int TotalCount)>(query, new { EntityId = entityId, StartDate = startDate, EndDate = endDate });
                return result;
            }, _databaseService);
        }

        // Круговая диаграмма: Процентное распределение товаров по заказам
        public async Task<IEnumerable<(string Product, double Percentage)>> GetProductSalesPercentageAsync()
        {
            const string query = @"
                SELECT 
					p.Title AS Product,
					SUM(e.Count) AS TotalCount
				FROM ""order"" o
				INNER JOIN ENTITY_PRODUCT_ORDER e ON e.Order_ID = o.ID
				INNER JOIN product p ON p.ID = e.Product_ID
				WHERE o.Entity_ID = 'cde4838c-5b14-4eb7-8eb9-24ee52db62b3'
				AND o.Application_Date BETWEEN to_date('2024-12-01', 'YYYY-MM-DD') AND to_date('2024-12-20', 'YYYY-MM-DD')
				AND p.Is_Deleted = false
				GROUP BY p.Title";

            var productSales = await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
            {
                return await dbConnection.QueryAsync<(string Product, double TotalCount)>(query);
            }, _databaseService);

            double totalSales = productSales.Sum(x => x.TotalCount);

            // Вычисляем процентное распределение для каждого продукта
            return productSales.Select(x => (x.Product, (x.TotalCount / totalSales) * 100));
        }
    }
}
