using System.Collections.Generic;
using IdeaDomain.DomainLayer.Entities;

namespace IdeaDomain.ServiceLayer
{
    public interface IAnalyzerService
    {
        #region Analyzer

        /// <summary>
        /// Gets the analyzers by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        List<Analyzer> GetAnalyzersByUserId(int userId);

        /// <summary>
        /// Gets the analyzer by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        Analyzer GetAnalyzerById(int analyzerId);

        /// <summary>
        /// Creates the analyzer.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        Analyzer CreateAnalyzer(Analyzer analyzer);

        /// <summary>
        /// Updates the analyzer.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        Analyzer UpdateAnalyzer(Analyzer analyzer);

        /// <summary>
        /// Deletes the analyzer by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        bool DeleteAnalyzerById(int analyzerId);

        /// <summary>
        /// Gets the analyzer data.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        AnalyzerDetail GetAnalyzerData(Analyzer analyzer, string sidx = "", string sord = "asc", string filters = "");

        /// <summary>
        /// Gets the analyzer data by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        AnalyzerDetail GetAnalyzerDataById(int analyzerId, string sidx = "", string sord = "asc", string filters = "");

        #endregion

    }
}
