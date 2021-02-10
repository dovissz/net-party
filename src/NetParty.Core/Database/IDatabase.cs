using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.Database
{
    /// <summary>
    /// Database interface
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Queries the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql) where T : class;

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        Task ExecuteAsync(string sql);
    }
}
