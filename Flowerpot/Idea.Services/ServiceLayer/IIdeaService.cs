using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdeaDomain.DomainLayer.Entities;

namespace IdeaDomain.ServiceLayer
{
    public interface IIdeaService
    {
        /// <summary>
        /// Gets the idea by id.
        /// </summary>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        Idea GetIdeaById(int ideaId);

        /// <summary>
        /// Gets the idea by id.
        /// </summary>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        Idea GetIdeaByName(string ideaName);

        /// <summary>
        /// Gets the ideas by user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        List<Idea> GetIdeasByUser(int userId);

        /// <summary>
        /// Creates the idea.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        Idea CreateIdea(Idea idea);

        /// <summary>
        /// Updates the idea.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        bool UpdateIdea(Idea idea);

        /// <summary>
        /// Deletes the idea by id.
        /// </summary>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        bool DeleteIdeaById(int ideaId);

        /// <summary>
        /// Validate Idea Name Duplication.
        /// </summary>
        /// <param name="ideaName"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        bool ValidateIdeaNameDuplication(string ideaName, int userId);

        List<ColumnInIdea> GetColumnsByIdea(int ideaId);
        /// <summary>
        /// Gets the column by id.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        ColumnInIdea GetColumnById(int columnId);

        /// <summary>
        /// Update a column by column entity.
        /// </summary>
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
        /// <param name="ideaId">The idea id.</param>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sord">The sord.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        IdeaDetail GetIdeaData(int ideaId, string sidx = "R.RowId", string sord = "asc", int page = 0, int rows = 20, string filters = null);

        /// <summary>
        /// Gets the idea data.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sord">The sord.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        IdeaDetail GetIdeaData(Idea idea, string sidx = "R.RowId", string sord = "asc", int page = 0, int rows = 20, string filters = null);

        /// <summary>
        /// Counts the idea rows.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        int CountIdeaRows(Idea idea, string filters);

        /// <summary>
        /// Gets the ref columns by column id.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        List<ColumnInIdea> GetRefColumnsByColumnId(int columnId);

        List<DataSource> GetIdeasForChart(Analyzer analyzer);
    }
}
