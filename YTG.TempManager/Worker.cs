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

        System.Timers.Timer? m_TempTimer = null;
        private CancellationTokenSource _stoppingCts;

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
                    m_TempTimer = new(6000); // Every minute
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
                DateTime now = DateTime.Now;
                if (now.Hour == 0 && now.Minute == 0 && now.Second == 0)
                {
                    await RunProcessesAsync();
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
