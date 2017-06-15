using System.Collections.Generic;
using IdeaDomain.DomainLayer.Entities;

namespace IdeaDomain.DomainLayer.RepositoryInterfaces
{
    public interface IIdeaRepository
    {
        /// <summary>
        /// Get ideas by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Idea> GetIdeasByUser(int userId);

        /// <summary>
        /// Get idea by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns>Return an idea object</returns>
        Idea GetIdeaById(int ideaId);

        /// <summary>
        /// Get idea by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns>Return an idea object</returns>
        Idea GetIdeaByName(string IdeaName);

        /// <summary>
        /// Create an idea with its columns under a transaction.
        /// </summary>
        /// <param name="idea"></param>
        /// <returns></returns>
        Idea CreateIdea(Idea idea);

        /// <summary>
        /// Update an idea.
        /// </summary>
        /// <param name="idea"></param>
        /// <returns></returns>
        bool UpdateIdea(Idea idea);

        /// <summary>
        /// Delete an idea by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        bool DeleteIdeaById(int ideaId);

        /// <summary>
        /// Validate Idea Name Duplication.
        /// </summary>
        /// <param name="ideaName"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        bool ValidateIdeaNameDuplication(string ideaName, int userId);

        /// <summary>
        /// Get columns by idea.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        List<ColumnInIdea> GetColumnsByIdea(int ideaId);

        /// <summary>
        /// Get a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        ColumnInIdea GetColumnById(int columnId);

        /// <summary>
        /// Update a column by column entity.
        /// </summary>ML
        /// <param name="columnId"></param>
        /// <returns></returns>
        bool UpdateColumn(ColumnInIdea column);

        /// <summary>
        /// Delete a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        bool DeleteColumnById(int columnId);

        /// <summary>
        /// Gets the idea data.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        IdeaDetail GetIdeaData(Idea idea, string sidx = "R.RowId", string sord = "asc", int page = 0, int rows = 20, string filters = null);

        /// <summary>
        /// Counts the idea rows.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        int CountIdeaRows(Idea idea, string filters);

        /// <summary>
        /// Gets the ref columns by column id.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        List<ColumnInIdea> GetRefColumnsByColumnId(int columnId);

    }
}
