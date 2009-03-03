//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Threading;

namespace Oxite.BackgroundServices
{
    public class BackgroundServiceExecutor
    {
        private IBackgroundService backgroundService;
        private Timer timer;

        public BackgroundServiceExecutor(IBackgroundService backgroundService)
        {
            this.backgroundService = backgroundService;
        }

        public void Start()
        {
            timer = new Timer(new TimerCallback(timerCallback), null, Timeout.Infinite, Timeout.Infinite);

            if (backgroundService.Interval.TotalMilliseconds > 0)
            {
#if DEBUG
                backgroundService.Run();
#endif
                runTimer();
            }
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
        }

        private void timerCallback(object sender)
        {
            backgroundService.Run();

            runTimer();
        }

        private void runTimer()
        {
            try
            {
                lock (timer)
                {
                    timer.Change(backgroundService.Interval, new TimeSpan(0, 0, 0, 0, -1));
                }
            }
            catch
            {
            }
        }
    }
}