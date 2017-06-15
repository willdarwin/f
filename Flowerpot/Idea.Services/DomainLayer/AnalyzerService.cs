using System.Collections.Generic;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.DomainLayer.RepositoryInterfaces;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.ServiceLayer;

namespace IdeaDomain.DomainLayer
{
    public class AnalyzerService : IAnalyzerService
    {
        public IAnalyzerRepository AnalyzerRepository { get; set; }

        public AnalyzerService()
        {
            AnalyzerRepository = Container.Resolve<IAnalyzerRepository>();
        }

        public AnalyzerService(IAnalyzerRepository analyzerRepository)
        {
            AnalyzerRepository = analyzerRepository;
        }

        #region Analyzer

        /// <summary>
        /// Gets the analyzers by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Analyzer> GetAnalyzersByUserId(int userId)
        {
            return AnalyzerRepository.GetAnalyzersByUserId(userId);
        }

        /// <summary>
        /// Gets the analyzer by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        public Analyzer GetAnalyzerById(int analyzerId)
        {
            return AnalyzerRepository.GetAnalyzerById(analyzerId);
        }

        /// <summary>
        /// Creates the analyzer.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        public Analyzer CreateAnalyzer(Analyzer analyzer)
        {
            return AnalyzerRepository.CreateAnalyzer(analyzer);
        }

        /// <summary>
        /// Updates the analyzer.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        public Analyzer UpdateAnalyzer(Analyzer analyzer)
        {
            return AnalyzerRepository.UpdateAnalyzer(analyzer);
        }

        /// <summary>
        /// Deletes the analyzer by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        public bool DeleteAnalyzerById(int analyzerId)
        {
            return AnalyzerRepository.DeleteAnalyzerById(analyzerId);
        }

        /// <summary>
        /// Gets the analyzer data.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sord">The sord.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        public AnalyzerDetail GetAnalyzerData(Analyzer analyzer, string sidx = "", string sord = "asc", string filters = "")
        {
            return AnalyzerRepository.GetAnalyzerData(analyzer, sidx, sord, filters);
        }

        /// <summary>
        /// Gets the analyzer data by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sord">The sord.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        public AnalyzerDetail GetAnalyzerDataById(int analyzerId, string sidx = "", string sord = "asc", string filters = "")
        {
            var analyzer = GetAnalyzerById(analyzerId);
            return AnalyzerRepository.GetAnalyzerData(analyzer, sidx, sord, filters);
        }

        #endregion

    }
}

