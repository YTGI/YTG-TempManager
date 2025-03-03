// --------------------------------------------------------------------------------
/*  Copyright © 2025, Yasgar Technology Group, Inc.
    Any unauthorized review, use, disclosure or distribution is prohibited.

    Purpose: Main Background worker class

    Description: 

*/
// --------------------------------------------------------------------------------

using Microsoft.Extensions.Options;

using System.Timers;

using YTG.TempManager.Services;

using System.Diagnostics;

namespace YTG.TempManager
{

    /// <summary>
    /// Main Background worker class
    /// </summary>
    public class Worker : BackgroundService
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tfService"></param>
        public Worker(ITFService tfService)
        {
            TFSvc = tfService;
        }

        #endregion Constructors

        #region Fields

        private System.Timers.Timer? m_TempTimer = null;
        private CancellationTokenSource _stoppingCts;
        private bool _hasRun = false;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets instance of ITFService from DI
        /// </summary>
        private ITFService TFSvc { get; }

        /// <summary>
        /// Gets the timer to start checking every 1 minute
        /// </summary>
        private System.Timers.Timer TempTimer
        {
            get
            {
                if (m_TempTimer == null)
                {
                    m_TempTimer = new(60000); // Every minute
                }
                return m_TempTimer;
            }
        }

        /// <summary>
        /// Gets or sets the cancellation token for this instance
        /// </summary>
        CancellationToken CancelToken { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Main Execute task
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken cancelToken)
        {
            if (OperatingSystem.IsWindows())
            {
                using (EventLog eventLog = new("Application"))
                {
                    eventLog.Source = "YTG Temp Manager Service";
                }
            }

            CancelToken = cancelToken;

            if (!CancelToken.IsCancellationRequested)
            {
                TempTimer.Elapsed += TempTimerElapsedAsync;
                TempTimer.Start();

                await RunProcessesAsync();

            }
        }


        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Timer elapsed process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TempTimerElapsedAsync(object? sender, ElapsedEventArgs e)
        {
            // Run again if it is midnight
            if (!CancelToken.IsCancellationRequested)
            {
                DateTime _now = DateTime.Now;
                if (_now.Hour == 0 && _now.Minute == 0)
                {
                    await RunProcessesAsync();
                }

                // Run every five minutes to execute after boot
                if (_now.Minute % 5 == 0 && !_hasRun)
                {
                    await RunProcessesAsync();
                    _hasRun = true;
                }
            }
        }

        /// <summary>
        /// Run the child process
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        private async Task RunProcessesAsync()
        {
            await TFSvc.RunProcessesAsync(CancelToken);
        }

        #endregion Private Methods

        #region Events

        #endregion Events



    }
}
