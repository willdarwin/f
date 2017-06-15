using System;
using System.Collections.Generic;
using System.Text;
using IdeaDomain.DomainLayer.Entities;

namespace IdeaDomain.DomainLayer.RepositoryInterfaces
{
    public interface IRowRepository
    {
        /// <summary>
        /// Gets the row by id.
        /// </summary>
        /// <param name="rowId">The row id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        Row GetRowById(int rowId, int userId);

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        bool AddRow(Row row);

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        bool UpdateRow(Row row);

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="rowID">The row ID.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        bool DeleteRow(int rowID, int userID);

    }
}
