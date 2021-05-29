using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;

namespace Qrame.CoreFX.Scheduler
{
    public interface ICronDaemon
    {
        void AddJob(string schedule, ThreadStart action);
        void Start();
        void Stop();
    }

    public class CronDaemon : ICronDaemon
    {
        private readonly System.Timers.Timer timer = new System.Timers.Timer(1000);
        private readonly List<ICronJob> cronJobs = new List<ICronJob>();
        private DateTime lastDateTime = DateTime.Now;

        public CronDaemon()
        {
            timer.AutoReset = true;
            timer.Elapsed += timer_Elapsed;
        }

        /*
        *    *    *    *    *    *  
        ==  ==    ==    ==    ==    ==
        ==  ==    ==    ==    ==    ==
        ==  ==    ==    ==    ==    ==
        ==  ==    ==    ==    ==    ============ day of week (0 - 6) (Sunday=0 )
        ==  ==    ==    ==    ====================== month (1 - 12)
        ==  ==    ==    ================================ day of month (1 - 31)
        ==  ==    ========================================== hour (0 - 23)
        ==  ==================================================== min (0 - 59)
        ============================================================ second (0 - 59)

        `* * * * * *`        Every second.
        `0 * * * * *`        Every minute.
        `* 0 * * * *`        Top of every hour.
        `* 0,1,2 * * * *`    Every hour at minutes 0, 1, and 2.
        `* * / 2 * * * *`    Every two minutes.
        `* 1-55 * * * *`     Every minute through the 55th minute.
        `* * 1,10,20 * * *`  Every 1st, 10th, and 20th hours.
        */
        public void AddJob(string schedule, ThreadStart action)
        {
            var cronJob = new CronJob(schedule, action);
            cronJobs.Add(cronJob);
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();

            foreach (CronJob job in cronJobs)
            {
                job.Abort();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Second != lastDateTime.Second)
            {
                lastDateTime = DateTime.Now;
                foreach (ICronJob job in cronJobs)
                {
                    job.Execute(DateTime.Now);
                }
            }
        }
    }
}
