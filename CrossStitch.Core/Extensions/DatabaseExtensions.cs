using Microsoft.EntityFrameworkCore;

namespace CrossStitch.Core.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddOrUpdate<T>(this DbContext ctx, T data) where T : class
        {
            if (data.IsDefault())
            {
                ctx.Add(data);
                return;
            }

            ctx.Update(data);
        }

        public static int Count<T>(this DbContext dbContext)
        {
            var sql = $"select count(1) from [{typeof(T).Name}s]";

            var connection = dbContext.Database.GetDbConnection();
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = sql;

                using (var reader = command.ExecuteReader())
                    if (reader.HasRows)
                        while (reader.Read())
                            return reader.GetInt32(0);
            }

            return 0;
        }
    }
}