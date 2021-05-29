using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qrame.CoreFX.Scheduler
{
    public interface ICronSchedule
    {
        bool IsValid(string expression);
        bool IsDateTime(DateTime dateTime);
    }

    public class CronSchedule : ICronSchedule
    {
        readonly static Regex regexDivided = new Regex(@"(\*/\d+)");
        readonly static Regex regexRange = new Regex(@"(\d+\-\d+)\/?(\d+)?");
        readonly static Regex regexWild = new Regex(@"(\*)");
        readonly static Regex regexList = new Regex(@"(((\d+,)*\d+)+)");
        readonly static Regex regexValidation = new Regex(regexDivided + "|" + regexRange + "|" + regexWild + "|" + regexList);

        private readonly string cronExpression;
        public List<int> Seconds;
        public List<int> Minutes;
        public List<int> Hours;
        public List<int> DaysOfMonth;
        public List<int> Months;
        public List<int> DaysOfWeek;

        public CronSchedule()
        {
        }

        public CronSchedule(string expressions)
        {
            this.cronExpression = expressions;
            Generate();
        }

        private bool isValid()
        {
            return IsValid(this.cronExpression);
        }

        public bool IsValid(string expression)
        {
            MatchCollection matches = regexValidation.Matches(expression);
            return matches.Count > 0;//== 5;
        }

        public bool IsDateTime(DateTime date_time)
        {
            return Seconds.Contains(date_time.Second) &&
              Minutes.Contains(date_time.Minute) &&
              Hours.Contains(date_time.Hour) &&
              DaysOfMonth.Contains(date_time.Day) &&
              Months.Contains(date_time.Month) &&
              DaysOfWeek.Contains((int)date_time.DayOfWeek);
        }

        private void Generate()
        {
            if (!isValid()) return;

            MatchCollection matches = regexValidation.Matches(this.cronExpression);

            GenerateSeconds(matches[0].ToString());

            if (matches.Count > 1)
                GenerateMinutes(matches[1].ToString());
            else
                GenerateMinutes("*");

            if (matches.Count > 2)
                GenerateHours(matches[2].ToString());
            else
                GenerateHours("*");

            if (matches.Count > 3)
                GenerateDaysOfMonth(matches[3].ToString());
            else
                GenerateDaysOfMonth("*");

            if (matches.Count > 4)
                GenerateMonths(matches[4].ToString());
            else
                GenerateMonths("*");

            if (matches.Count > 5)
                GenerateDaysOfWeeks(matches[5].ToString());
            else
                GenerateDaysOfWeeks("*");
        }

        private void GenerateSeconds(string match)
        {
            this.Seconds = GenerateValues(match, 0, 60);
        }

        private void GenerateMinutes(string match)
        {
            this.Minutes = GenerateValues(match, 0, 60);
        }

        private void GenerateHours(string match)
        {
            this.Hours = GenerateValues(match, 0, 24);
        }

        private void GenerateDaysOfMonth(string match)
        {
            this.DaysOfMonth = GenerateValues(match, 1, 32);
        }

        private void GenerateMonths(string match)
        {
            this.Months = GenerateValues(match, 1, 13);
        }

        private void GenerateDaysOfWeeks(string match)
        {
            this.DaysOfWeek = GenerateValues(match, 0, 7);
        }

        private List<int> GenerateValues(string configuration, int start, int max)
        {
            if (regexDivided.IsMatch(configuration)) return Divided(configuration, start, max);
            if (regexRange.IsMatch(configuration)) return Range(configuration);
            if (regexWild.IsMatch(configuration)) return Wild(configuration, start, max);
            if (regexList.IsMatch(configuration)) return List(configuration);

            return new List<int>();
        }

        private List<int> Divided(string configuration, int start, int max)
        {
            if (!regexDivided.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("/".ToCharArray());
            int divisor = int.Parse(split[1]);

            for (int i = start; i < max; ++i)
                if (i % divisor == 0)
                    ret.Add(i);

            return ret;
        }

        private List<int> Range(string configuration)
        {
            if (!regexRange.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("-".ToCharArray());
            int start = int.Parse(split[0]);
            int end = 0;
            if (split[1].Contains("/"))
            {
                split = split[1].Split("/".ToCharArray());
                end = int.Parse(split[0]);
                int divisor = int.Parse(split[1]);

                for (int i = start; i < end; ++i)
                    if (i % divisor == 0)
                        ret.Add(i);
                return ret;
            }
            else
                end = int.Parse(split[1]);

            for (int i = start; i <= end; ++i)
                ret.Add(i);

            return ret;
        }

        private List<int> Wild(string configuration, int start, int max)
        {
            if (!regexWild.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();

            for (int i = start; i < max; ++i)
                ret.Add(i);

            return ret;
        }

        private List<int> List(string configuration)
        {
            if (!regexList.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();

            string[] split = configuration.Split(",".ToCharArray());

            foreach (string s in split)
                ret.Add(int.Parse(s));

            return ret;
        }
    }
}
