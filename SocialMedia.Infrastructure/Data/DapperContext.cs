using SocialMedia.Core.Enum;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IDbConnectionFactory _connFactory;

        private static readonly AsyncLocal<(
            IDbConnection? Conn, IDbTransaction? Tx)>
            _ambient = new();

        public DataBaseProvider Provider => _connFactory.Provider;

        public DapperContext(IDbConnectionFactory connFactory)
        {
            _connFactory = connFactory;
        }

        /// <summary>
        /// UnitOfWork llamara a este metodo al inciar la transaccion
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tx"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetAmbientConnection(IDbConnection conn,
            IDbTransaction? tx)
        {
            _ambient.Value = (conn, tx);
        }

        /// <summary>
        /// UnitOfWork llamara cuando finalice/rollback
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void ClearAmbientConnection()
        {
            _ambient.Value = (null, null);
        }

        /// <summary>
        /// Si el UnitOfWork está activo:
        /// Usa la misma conexión y transacción. Si no hay UnitOfWork:
        /// Crea una conexión nueva desde IDbConnectionFactory.
        /// ownsConnection indica si la conexión es propiedad de este contexto (y debe cerrarla) o si la comparte con UnitOfWork(y no debe cerrarla).
        /// </summary>
        /// <returns></returns>
        private (IDbConnection conn, IDbTransaction? tx, bool ownsConnection) GetConnAndTx()
        {
            var ambient = _ambient.Value;
            if (ambient.Conn != null)
                return (ambient.Conn, ambient.Tx, false);

            var conn = _connFactory.CreateConnection();
            return (conn, null, true);
        }

        /// <summary>
        /// Antes de ejecutar algo, asegura que la conexión esté abierta.
        /// Esto evita errores tipo "Invalid operation: connection is closed".
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private async Task OpenIfNeededAsync(IDbConnection conn)
        {
            if (conn is DbConnection dbConn
                && dbConn.State
                == ConnectionState.Closed)
            {
                await dbConn.OpenAsync();
            }
        }

        public Task<int> ExecuteAsync(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public Task<T?> QueryFirstOrDefault<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            throw new NotImplementedException();
        }
    }
}
