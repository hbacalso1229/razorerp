using System.Data;

namespace UserManagement.Application.Common.Interfaces
{
    public interface IDapperContext
    {
        /// <summary>
        /// Custom query
        /// </summary>
        /// <typeparam name="M">Data model</typeparam>
        /// <param name="getData"></param>
        /// <returns></returns>
        Task<M> QueryCustom<M>(Func<IDbConnection, Task<M>> getData);
    }
}
