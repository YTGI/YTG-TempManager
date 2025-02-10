// --------------------------------------------------------------------------------
/*  Copyright © 2024, Yasgar Technology Group, Inc.
	Any unauthorized review, use, disclosure or distribution is prohibited.

	Purpose: Temp Folder Manager Service methods

	Description: 

*/
// --------------------------------------------------------------------------------

using Microsoft.Extensions.Options;
using System.Timers;

namespace YTG.TempManager.Services
{

    /// <summary>
    /// Temp Folder Manager Service methods
    /// </summary>
    public class TFService : ITFService
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options"></param>
        public TFService(IOptions<YTGAppSettings> options)
        {
            AppSettings = options.Value;
        }

        #endregion Constructors

        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the cancellation token for this instance
        /// </summary>
        CancellationToken CancelToken { get; set; }

        /// <summary>
        /// Gets the Appsettings valuse
        /// </summary>
        YTGAppSettings AppSettings { get; }

        #endregion Properties

        #region Public Methods


        /// <summary>
        /// Runs the required processes
        /// </summary>
        /// <returns></returns>
        public async Task RunProcessesAsync(CancellationToken cancelToken)
        {

            CancelToken = CancelToken;

            await CreateTempFolderAsync();
            await ArchiveTempFolderAsync();

        }


        /// <summary>
        /// Create the temp directory for today
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateTempFolderAsync()
        {
            bool blnSuccess = false;
            try
            {
                string _sourceFolder = Path.GetFullPath(AppSettings.SourceFolder ?? "C:\\Temp");
                string _subFolderName = DateTime.Now.ToString("yyyyMMdd");

                if (!Directory.Exists(Path.Combine(_sourceFolder, _subFolderName)))
                {
                    Directory.CreateDirectory(Path.Combine(_sourceFolder, _subFolderName));
                    blnSuccess = true;
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(blnSuccess);
            }
            return await Task.FromResult(blnSuccess);
        }

        /// <summary>
        /// Archive the temp folder to the archive folder under the temp folder
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ArchiveTempFolderAsync()
        {
            bool blnSuccess = false;

            string _sourceFolder = Path.GetFullPath(AppSettings.SourceFolder ?? "C:\\Temp");
            string _destFolder = Path.GetFullPath(AppSettings.DestinationFolder ?? "C:\\Temp");

            if (!(Directory.Exists(_destFolder)))
            { Directory.CreateDirectory(_destFolder); }

            foreach (string directory in Directory.GetDirectories(_sourceFolder))
            {
                DateTime _folderDate = FolderToDate(directory);

                if ((_folderDate < DateTime.Now.AddDays(AppSettings.ArchiveLookbackDays * -1))
                    && (_folderDate > DateTime.MinValue))
                {
                    if ((Directory.GetFiles(directory).Count() == 0)
                        && (Directory.GetDirectories(directory).Count() == 0))
                    {
                        // Remove read-only etc.
                        SetAttributesNormal(new DirectoryInfo(directory));

                        // The folder has nothing in it
                        Directory.Delete(directory);
                        continue;
                    }

                    string strFolderName = _folderDate.ToString("yyyyMMdd");

                    if (!Directory.Exists(Path.Combine(_destFolder, strFolderName)))
                    {
                        Directory.Move(directory, Path.Combine(_destFolder, strFolderName));
                    }
                }
            }

            blnSuccess = true;

            return await Task.FromResult(blnSuccess);

        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Set attributes for a directory
        /// </summary>
        /// <param name="dir"></param>
        private void SetAttributesNormal(DirectoryInfo dir)
        {

            foreach (var subDir in dir.GetDirectories())
            {
                SetAttributesNormal(subDir);
            }

            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }

            dir.Attributes = FileAttributes.Normal;

        }

        /// <summary>
        /// Convert the folder to a date
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private DateTime FolderToDate(string folderName)
        {
            DateTime _return = DateTime.MinValue;

            int _folderNameAsInt;

            // Make sure it doesn't end with a backslash
            int _lastBSlashIndex = folderName.LastIndexOf('\\');

            string _folderName = folderName.Substring(_lastBSlashIndex + 1, (folderName.Length - (_lastBSlashIndex + 1)));

            if (_folderName.Trim().Length == 8)
            {
                // See if the folder name is numeric
                if (int.TryParse(_folderName, out _folderNameAsInt))
                {
                    string _dateString = _folderName.Substring(4, 2) + "-";
                    _dateString += _folderName.Substring(6, 2) + "-";
                    _dateString += _folderName.Substring(0, 4);
                    DateTime.TryParse(_dateString, out _return);
                }
            }

            return _return;

        }



        #endregion Private Methods

    }
}
