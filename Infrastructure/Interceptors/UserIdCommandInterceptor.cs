using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Domain.API.Interfaces;

namespace Infrastructure.Interceptors
{
    public class UserIdCommandInterceptor : DbCommandInterceptor
    {
        private readonly ICurrentUserContext _userContext;

        public UserIdCommandInterceptor(ICurrentUserContext userContext)
        {
            _userContext = userContext;
            Console.WriteLine("[INTERCEPTOR] UserIdCommandInterceptor criado!");
        }

        // SÍNCRONO: Não usa await
        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            PrintIntercept("NonQueryExecuting", command.CommandText);
            SetUserId(command);
            return base.NonQueryExecuting(command, eventData, result);
        }

        // ASSÍNCRONO: Usa await
        public override async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            PrintIntercept("NonQueryExecutingAsync", command.CommandText);
            await SetUserIdAsync(command);
            return await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            PrintIntercept("ReaderExecuting", command.CommandText);
            if (IsInsertOrUpdate(command.CommandText))
                SetUserId(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            PrintIntercept("ReaderExecutingAsync", command.CommandText);
            if (IsInsertOrUpdate(command.CommandText))
                await SetUserIdAsync(command);
            return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            PrintIntercept("ScalarExecuting", command.CommandText);
            if (IsInsertOrUpdate(command.CommandText))
                SetUserId(command);
            return base.ScalarExecuting(command, eventData, result);
        }

        public override async ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            PrintIntercept("ScalarExecutingAsync", command.CommandText);
            if (IsInsertOrUpdate(command.CommandText))
                await SetUserIdAsync(command);
            return await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        // Utilitário para identificar comandos que precisam setar o user_id
        private bool IsInsertOrUpdate(string commandText)
        {
            var sql = commandText.TrimStart().ToUpperInvariant();
            return sql.StartsWith("INSERT") || sql.StartsWith("UPDATE") || sql.StartsWith("DELETE");
        }

        // Síncrono
        private void SetUserId(DbCommand command)
        {
            try
            {
                var userId = _userContext.IsExternalRequest ? -1 : _userContext.Id;
                Console.WriteLine($"[INTERCEPTOR] Setando @user_id para: {userId} | Query: {command.CommandText}");

                // Evita loop infinito ao interceptar o próprio SET
                if (command.CommandText.StartsWith("SET @user_id") || command.CommandText.StartsWith("SET SESSION"))
                    return;

                using var setCommand = command.Connection.CreateCommand();
                setCommand.Transaction = command.Transaction;
                setCommand.CommandText = $"SET @user_id = {userId};";
                setCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[INTERCEPTOR] EXCEPTION: " + ex);
            }
        }

        // Assíncrono
        private async Task SetUserIdAsync(DbCommand command)
        {
            try
            {
                var userId = _userContext.IsExternalRequest ? -1 : _userContext.Id;
                Console.WriteLine($"[INTERCEPTOR] Setando @user_id para: {userId} | Query: {command.CommandText}");

                if (command.CommandText.StartsWith("SET @user_id") || command.CommandText.StartsWith("SET SESSION"))
                    return;

                using var setCommand = command.Connection.CreateCommand();
                setCommand.Transaction = command.Transaction;
                setCommand.CommandText = $"SET @user_id = {userId};";
                await setCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[INTERCEPTOR] EXCEPTION: " + ex);
            }
        }

        private void PrintIntercept(string method, string sql)
        {
            Console.WriteLine($"[INTERCEPTOR] {method} chamado para: {sql.Substring(0, Math.Min(sql.Length, 120))}");
        }
    }
}
