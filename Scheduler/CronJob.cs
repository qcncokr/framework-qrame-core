using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Qrame.CoreFX.Scheduler
{
    public interface ICronJob
    {
        void Execute(DateTime dateTime);
        void Abort();
    }

    public class CronJob : ICronJob
    {
        private readonly ICronSchedule cronSchedule = new CronSchedule();
        private readonly ThreadStart threadStart;
        private Thread thread;

        public CronJob(string schedule, ThreadStart action)
        {
            cronSchedule = new CronSchedule(schedule);
            threadStart = action;
            thread = new Thread(action);
        }

        private object lockObject = new object();
        public void Execute(DateTime dateTime)
        {
            lock (lockObject)
            {
                if (!cronSchedule.IsDateTime(dateTime))
                {
                    return;
                }

                if (thread.ThreadState == ThreadState.Running)
                {
                    return;
                }

                thread = new Thread(threadStart);
                thread.Start();
            }
        }

        public void Abort()
        {
            thread.Abort();
        }

    }
}
