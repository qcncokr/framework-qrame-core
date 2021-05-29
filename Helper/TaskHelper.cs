using Qrame.CoreFX.Scheduler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Qrame.CoreFX.Helper
{
    public class RetryTask
    {
        public int RetryCount { get; set; }
        public DateTime? InvokeDateTime { get; set; }
    }

    /*
public static class Task15
  {
    private static string TaskName = "Task15";
    private static string TaskID = null;
    private static Timer timer = new Timer();
    private static Dictionary<string, RetryTask> WaitingTaskInvokes = new Dictionary<string, RetryTask>();
    private static BackgroundWorker worker = null;

    private static bool Is305 = false;
    private static bool Is401 = false;
    private static bool Is404 = false;

    public delegate void Completed(object Result);
    public static event Completed WorkerCompleted;

    public static bool IsBusy
    {
      get
      {
        return worker.IsBusy;
      }
    }

    public static void WaitingTaskTimer()
    {
      if (timer.Enabled == false)
      {
        timer.AutoReset = true;
        timer.Interval = 300000; // 5분
        timer.Elapsed -= null;
        timer.Elapsed += (object sender, ElapsedEventArgs e) =>
        {
          if (IsBusy == false)
          {
            if (WaitingTaskInvokes.Count > 0)
            {
              var WaitingTaskInvoke = WaitingTaskInvokes.First();

              if (WaitingTaskInvoke.Value.RetryCount <= 3)
              {
                TaskID = WaitingTaskInvoke.Key;
                WaitingTaskInvoke.Value.RetryCount += 1;
                Start();
              }
              else
              {
                WaitingTaskInvokes.Remove(WaitingTaskInvoke.Key);
              }
            }
          }

          if (WaitingTaskInvokes.Count == 0)
          {
            timer.Stop();
          }
        };

        timer.Start();
      }
    }

    public static void Initialize()
    {
      // 점검설정 조회
      using (AccessClient accessClient = new AccessClient(ProviderUsage.Prefernce))
      using (DataTable C002 = accessClient.GetDataTable(string.Format(AccessSQL.PrivatePolicy_GetList, "C002")))
      {
        if (C002 != null && C002.Rows.Count > 0)
        {
          foreach (DataRow item in C002.Rows)
          {
            if (item["IsPolicy"].ToString() == "True")
            {
              switch (item["CodeID"].ToString())
              {
                case "305":
                  Is305 = true;
                  break;
                case "401":
                  Is401 = true;
                  break;
                case "404":
                  Is404 = true;
                  break;
              }
            }
          }
        }
      }
    }

    public static void Start()
    {
      if (worker == null)
      {
        worker = new BackgroundWorker();
        worker.WorkerReportsProgress = true;
        worker.WorkerSupportsCancellation = true;

        worker.DoWork += worker_DoWork;
      }

      if (worker.IsBusy == false)
      {
        if (TaskID == null)
        {
          TaskID = Guid.NewGuid().ToString();
        }
        else
        {
          WaitingTaskInvokes.Remove(TaskID);
        }

        Log.Trace("3", string.Format("{0} 작업 ID - {1}, 시작시간 - {2}", TaskName, TaskID, DateTime.Now.ToString()), true);

        worker.RunWorkerAsync();
      }
      else
      {
        return;
        if (TaskID == null)
        {
          WaitingTaskInvokes.Add(Guid.NewGuid().ToString(), new RetryTask()
          {
            RetryCount = 0,
            InvokeDateTime = DateTime.Now
          });
        }

        WaitingTaskTimer();
      }
    }

    private static void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      object Result = null;
      try
      {
        string AssetID = CryptographyHelper.Decrypt(UserComputer.AssetKeyID);
        Stopwatch stopwatch = new Stopwatch();

        using (AccessClient accessClient = new AccessClient(ProviderUsage.CMM0033))
        {
          if (Is305 == true)
          {
            // 305, 방화벽 설정 확인 (304, 방화벽설정)
            stopwatch.Restart();
            accessClient.ExecuteNonQuery(string.Format(AccessSQL.ProtectLog_Insert,
              "304",
              AssetID,
              "",
              "",
              "",
              RegistryHelper.IsFirewallEnabled() == false ? "1" : "0",
              stopwatch.ElapsedMilliseconds.ToString(),
              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            ));
          }
        }

        using (AccessClient accessClient = new AccessClient(ProviderUsage.CMM0034))
        {
          if (Is401 == true)
          {
            // 401, 화면보호기 활성화 여부 (401, 스크린세이버 설정 확인)
            stopwatch.Restart();
            accessClient.ExecuteNonQuery(string.Format(AccessSQL.ProtectLog_Insert,
              "401",
              AssetID,
              "",
              "",
              "",
              RegistryHelper.IsScreenSaverActive() == false ? "1" : "0",
              stopwatch.ElapsedMilliseconds.ToString(),
              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            ));
          }

          if (Is404 == true)
          {
            // 404, 윈도우 자동로그인 (402, 윈도우 자동 로그인 확인)
            stopwatch.Restart();
            accessClient.ExecuteNonQuery(string.Format(AccessSQL.ProtectLog_Insert,
              "402",
              AssetID,
              "",
              "",
              "",
              RegistryHelper.IsWindowsAutoLoginD() == true ? "1" : "0",
              stopwatch.ElapsedMilliseconds.ToString(),
              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            ));
          }
        }

        Log.Trace("4", string.Format("{0} 작업 ID - {1}, 종료시간 - {2}", TaskName, TaskID, DateTime.Now.ToString()), true);
        TaskID = null;
      }
      catch (Exception exception)
      {
        Result = exception;
      }

      if (WorkerCompleted != null)
      {
        WorkerCompleted(Result);
      }
    }
  }
         */
    public static class TaskHelper
    {
        private static readonly Dictionary<string, CronDaemon> cronDaemons = new Dictionary<string, CronDaemon>();

        public static void RestartTaskAll()
        {
            ClearTaskAll();

            /*
             <table1>
                <PrivatePolicyID>1</PrivatePolicyID>
                <PrivatePolicyGroupID>1</PrivatePolicyGroupID>
                <PolicyTypeCodeID>1</PolicyTypeCodeID>
                <CodeGroupID>C003</CodeGroupID>
                <CodeID>1</CodeID>
                <CodeValue>개인정보노출점검 수행</CodeValue>
                <IsPolicy>true</IsPolicy>
                <Custom1>1</Custom1>
                <Custom2>1</Custom2>
                <Custom3>* * 11,16 * * *</Custom3>
              </table1>
             */
            using (DataTable dataTable = new DataTable())
            {
                string CodeID = "";
                string TaskName = "";
                string IsStart = "";
                string IsRepeat = "";
                string CronPattern = "";

                foreach (DataRow item in dataTable.Rows)
                {
                    if (item["IsPolicy"].ToString() == "True")
                    {
                        CodeID = item["CodeID"].ToString();
                        TaskName = "QCN.SecurePrivacyClient.Task.Task" + CodeID;
                        IsStart = item["Custom1"].ToString();
                        IsRepeat = item["Custom2"].ToString();
                        CronPattern = item["Custom3"].ToString();
                        CronDaemon DaemonTask = null;

                        Type taskType = Type.GetType(TaskName);

                        if (taskType != null)
                        {
                            TaskConfiguration(TaskName);
                            DaemonTask = TaskRegister(IsStart, IsRepeat, CronPattern, Type.GetType(TaskName).GetMethod("Start"));
                            if (DaemonTask != null && string.IsNullOrEmpty(CronPattern) == false)
                            {
                                TaskHelper.AddTask(TaskName, DaemonTask);
                            }
                        }
                    }
                }
            }
            
            StartTaskAll();
        }

        private static CronDaemon TaskRegister(string IsStart, string IsRepeat, string CronPattern, MethodInfo methodInfo)
        {
            CronDaemon DaemonTask = null;
            if (IsRepeat == "1")
            {
                DaemonTask = new CronDaemon();
                ThreadStart threadMain = delegate () { methodInfo.Invoke(methodInfo, null); };
                DaemonTask.AddJob(CronPattern, threadMain);
            }

            if (IsStart == "1")
            {
                methodInfo.Invoke(methodInfo, null);
            }

            return DaemonTask;
        }

        public static void TaskConfiguration(string TaskName)
        {
            MethodInfo methodInfo = Type.GetType(TaskName).GetMethod("Initialize");

            if (methodInfo != null)
            {
                methodInfo.Invoke(methodInfo, null);
            }
        }

        public static void AddTask(string TaskName, CronDaemon Daemon)
        {
            cronDaemons.Add(TaskName, Daemon);
        }

        public static void ExecuteTask(string TaskName)
        {
            foreach (var item in cronDaemons)
            {
                if (item.Key == TaskName)
                {
                    MethodInfo methodInfo = Type.GetType(TaskName).GetMethod("Start");
                    methodInfo.Invoke(methodInfo, null);
                    break;
                }
            }
        }

        public static void StartTaskAll()
        {
            foreach (var item in cronDaemons)
            {
                CronDaemon cronDaemon = (CronDaemon)item.Value;
                cronDaemon.Start();
            }
        }

        public static void ClearTaskAll()
        {
            foreach (var item in cronDaemons)
            {
                CronDaemon cronDaemon = (CronDaemon)item.Value;
                cronDaemon.Stop();
            }

            cronDaemons.Clear();
        }

        public static void ClearTask(string TaskName)
        {
            foreach (var item in cronDaemons)
            {
                if (item.Key == TaskName)
                {
                    CronDaemon cronDaemon = (CronDaemon)item.Value;
                    cronDaemon.Stop();
                    break;
                }
            }

            cronDaemons.Remove(TaskName);
        }

        public static void StartTask(string TaskName)
        {
            foreach (var item in cronDaemons)
            {
                if (item.Key == TaskName)
                {
                    CronDaemon cronDaemon = (CronDaemon)item.Value;
                    cronDaemon.Start();
                    break;
                }
            }
        }

        public static void StopTask(string TaskName)
        {
            foreach (var item in cronDaemons)
            {
                if (item.Key == TaskName)
                {
                    CronDaemon cronDaemon = (CronDaemon)item.Value;
                    cronDaemon.Stop();
                    break;
                }
            }
        }
    }
}
