using Castle.DynamicProxy;
using RentACar.Core.Utilities.Interceptors;
using System.Transactions;  

namespace RentACar.Core.Aspects.Autofac.Transaction
{
    /// <summary>
    /// Metot çalışma anında araya girerek (Interception) veritabanı işlemleri için Transaction yönetimini sağlar.
    /// Senkron (void/T) ve Asenkron (Task/Task<T>) metotları destekler. Hata durumunda işlemleri geri alır (Rollback).
    /// </summary>
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled
            );

            try
            {
                invocation.Proceed();

                if (invocation.ReturnValue is Task task)
                {
                    var returnType = invocation.Method.ReturnType;

                    if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        var resultType = returnType.GetGenericArguments()[0];

                        var method = typeof(TransactionScopeAspect)
                            .GetMethod(nameof(HandleAsyncWithResult), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.MakeGenericMethod(resultType);

                        invocation.ReturnValue = method?.Invoke(this, new object[] { task, transactionScope });
                    }
                    else
                    {
                        invocation.ReturnValue = HandleAsync(task, transactionScope);
                    }
                }
                else
                {
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
            }
            catch (System.Exception)
            {
                transactionScope.Dispose();
                throw;
            }
        }

        private async Task<T> HandleAsyncWithResult<T>(Task task, TransactionScope transactionScope)
        {
            try
            {
                var genericTask = (Task<T>)task;
                var result = await genericTask;

                transactionScope.Complete();
                return result;
            }
            finally
            {
                transactionScope.Dispose();
            }
        }

        private async Task HandleAsync(Task task, TransactionScope transactionScope)
        {
            try
            {
                await task;
                transactionScope.Complete();
            }
            finally
            {
                transactionScope.Dispose();
            }
        }
    }
}
