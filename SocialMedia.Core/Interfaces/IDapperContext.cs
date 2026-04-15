using SocialMedia.Core.Enum;
using System.Data;

namespace SocialMedia.Core.Interfaces
{
    public interface IDapperContext
    {
        DataBaseProvider Provider { get; }

        Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        Task<T?> QueryFirstOrDefault<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        Task<int> ExecuteAsync(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        Task<T> ExecuteScalarAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text
            );

        void SetAmbientConnection(
            IDbConnection conn,
            IDbTransaction? tx);

        //UnitOfWork llamara este método al finalizar/rollback
        void ClearAmbientConnection();
    }
}
