// --------------------------------------------------------------------------------
/*  Copyright © 2024, Yasgar Technology Group, Inc.
	Any unauthorized review, use, disclosure or distribution is prohibited.

	Purpose: Temp Folder Manager Service methods

	Description: 

*/
// --------------------------------------------------------------------------------

namespace YTG.TempManager.Services
{
    public interface ITFService
    {

        /// <summary>
        /// Runs the required processes
        /// </summary>
        /// <returns></returns>
        Task RunProcessesAsync(CancellationToken cancelToken);

        /// <summary>
        /// Archive the temp folder to the archive folder under the temp folder
        /// </summary>
        /// <returns></returns>
        Task<bool> ArchiveTempFolderAsync();

        /// <summary>
        /// Create the temp directory for today
        /// </summary>
        /// <returns></returns>
        Task<bool> CreateTempFolderAsync();
    }
}