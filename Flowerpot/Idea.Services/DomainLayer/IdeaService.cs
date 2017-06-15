using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.DomainLayer.RepositoryInterfaces;
using IdeaDomain.ServiceLayer;
using Container = GeneralUtilities.Utilities.Unity.Container;


namespace IdeaDomain.DomainLayer
{
    public class IdeaService : IIdeaService
    {
        public IIdeaRepository IdeaRepository { get; set; }

        public IdeaService()
        {
            IdeaRepository = Container.Resolve<IIdeaRepository>();
        }

        public IdeaService(IIdeaRepository ideaRepository)
        {
            IdeaRepository = ideaRepository;
        }

        /// <summary>
        /// Get an idea with its columns.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public Idea GetIdeaById(int ideaId)
        {
            return IdeaRepository.GetIdeaById(ideaId);
        }

        /// <summary>
        /// Get an idea with its columns.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public Idea GetIdeaByName(string ideaName)
        {
            return IdeaRepository.GetIdeaByName(ideaName);
        }

        /// <summary>
        /// Get all ideas by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Idea> GetIdeasByUser(int userId)
        {
            return IdeaRepository.GetIdeasByUser(userId);
        }

        /// <summary>
        /// Create an idea with its columns.
        /// </summary>
        /// <param name="idea"></param>
        /// <returns></returns>
        public Idea CreateIdea(Idea idea)
        {
            return IdeaRepository.CreateIdea(idea);
        }

        /// <summary>
        /// Update an idea.
        /// </summary>
        /// <param name="idea"></param>
        /// <returns></returns>
        public bool UpdateIdea(Idea idea)
        {
            return IdeaRepository.UpdateIdea(idea);
        }

        /// <summary>
        /// Delete an idea by idea id.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public bool DeleteIdeaById(int ideaId)
        {
            return IdeaRepository.DeleteIdeaById(ideaId);
        }

        /// <summary>
        /// Validate Idea Name Duplication.
        /// </summary>
        /// <param name="ideaName"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        public bool ValidateIdeaNameDuplication(string ideaName, int userId)
        {
            return IdeaRepository.ValidateIdeaNameDuplication(ideaName, userId);
        }

        /// <summary>
        /// Get columns by idea.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public List<ColumnInIdea> GetColumnsByIdea(int ideaId)
        {
            return IdeaRepository.GetColumnsByIdea(ideaId);
        }

        /// <summary>
        /// Get a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public ColumnInIdea GetColumnById(int columnId)
        {
            return IdeaRepository.GetColumnById(columnId);
        }

        /// <summary>
        /// Update a column by column entity.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public bool UpdateColumn(ColumnInIdea column)
        {
            return IdeaRepository.UpdateColumn(column);
        }

        /// <summary>
        /// Delete a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public bool DeleteColumnById(int columnId)
        {
            return IdeaRepository.DeleteColumnById(columnId);
        }

        /// <summary>
        /// Gets the idea data.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IdeaDetail GetIdeaData(int ideaId, string sidx, string sord, int page, int rows, string filters)
        {
            var idea = GetIdeaById(ideaId);
            if (idea == null)
            {
                throw new Exception("The Idea has't been created!");
            }
            return IdeaRepository.GetIdeaData(idea, sidx, sord, page, rows, filters);
        }

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
        public IdeaDetail GetIdeaData(Idea idea, string sidx, string sord, int page, int rows, string filters)
        {
            return IdeaRepository.GetIdeaData(idea, sidx, sord, page, rows, filters);
        }

        /// <summary>
        /// Counts the idea rows.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        public int CountIdeaRows(Idea idea, string filters)
        {
            return IdeaRepository.CountIdeaRows(idea, filters);
        }

        /// <summary>
        /// Gets the ref columns by column id.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        public List<ColumnInIdea> GetRefColumnsByColumnId(int columnId)
        {
            return IdeaRepository.GetRefColumnsByColumnId(columnId);
        }

        public List<DataSource> GetIdeasForChart(Analyzer analyzer)
        {
            throw new NotImplementedException();
        }
    }
}
