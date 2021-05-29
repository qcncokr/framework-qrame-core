using System;
using System.Linq;
using System.Collections.Generic;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// ICollection 타입에 값이 중복되지 않도록 입력하며, 중복시 반환값이 false
        /// </summary>
        /// <typeparam name="T">제너릭 타입</typeparam>
        /// <param name="@this">컬렉션</param>
        /// <param name="value">입력값</param>
        /// <returns>중복여부</returns>
        public static bool AddUnique<T>(this ICollection<T> @this, T value)
        {
            if (@this.Contains(value) == false)
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// IList 타입에 값이 중복되지 않도록 입력하며, 중복시 반환값이 false
        /// </summary>
        /// <typeparam name="T">제너릭 타입</typeparam>
        /// <param name="@this">컬렉션</param>
        /// <param name="value">입력값</param>
        /// <returns>중복여부</returns>
        public static bool AddUnique<T>(this IList<T> @this, int index, T item)
        {
            if (@this.Contains(item) == false)
            {
                @this.Insert(index, item);
                return true;
            }

            return false;
        }

        public static string CharArrayToString(this char[] @this)
        {
            string result = "";
            Array.Sort(@this);
            foreach (char c in @this)
            {
                if (char.IsLetter(c))
                {
                    result += c;
                    result += ',';
                }
            }
            if (result.Length > 1)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        public static bool IsContains(this char[] @this, char character)
        {
            foreach (char c in @this)
            {
                if (c.Equals(character))
                {
                    return true;
                }
            }
            return false;
        }

        /* Pivot 구현 T-SQL
create table #t 
( 
    pointid [int], 
    doublevalue [float], 
    title [nvarchar](50) 
) 
 
insert into #t 
    select 
        distinct top 100 
        v.pointid, v.doublevalue, p.title 
    from [property] p 
        inner join pointvalue v on p.propertyid = v.propertyid 
        inner join point pt on v.pointid = pt.pointid 
    where v.pointid in (select top 5 p.pointid from point p where p.instanceid = 36132) 
 
declare @fields nvarchar(250) 
@this @fields = (select STUFF((SELECT N',[' + title + ']' FROM [property] FOR XML PATH('')), 1, 1, N'')) 
--select @fields 
 
declare @sql nvarchar(500) 
@this @sql = 'select * from #t 
pivot 
( 
    sum(doublevalue) 
    for [title] in ('+@fields+') 
) as alias' 
--select @sql 
 
exec (@sql) 
 
drop table #t 

         */
        /*
    internal class Employee
    {
        public string Name { get; @this; }
        public string Department { get; @this; }
        public string Function { get; @this; }
        public decimal Salary { get; @this; }
    }

            var l = new List<Employee>() {
            new Employee() { Name = "Fons", Department = "R&D", Function = "Trainer", Salary = 2000 },
            new Employee() { Name = "Jim", Department = "R&D", Function = "Trainer", Salary = 3000 },
            new Employee() { Name = "Ellen", Department = "Dev", Function = "Developer", Salary = 4000 },
            new Employee() { Name = "Mike", Department = "Dev", Function = "Consultant", Salary = 5000 },
            new Employee() { Name = "Jack", Department = "R&D", Function = "Developer", Salary = 6000 },
            new Employee() { Name = "Demy", Department = "Dev", Function = "Consultant", Salary = 2000 }};

            var result1 = l.Pivot(emp => emp.Department, emp2 => emp2.Function, lst => lst.Sum(emp => emp.Salary));

            foreach (var row in result1)
            {
                Console.WriteLine(row.Key);
                foreach (var column in row.Value)
                {
                    Console.WriteLine("  " + column.Key + "\t" + column.Value);
                }
            }
         */
        /// <summary>
        /// Linq에 대응하는 데이터 소스에서 Pivot을 구현합니다.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TFirstKey"></typeparam>
        /// <typeparam name="TSecondKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="@this"></param>
        /// <param name="firstKeySelector"></param>
        /// <param name="secondKeySelector"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> @this, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var result = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = @this.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                result.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }

            return result;
        }

        public static Dictionary<TFirstKey, TValue> Pivot<TSource, TFirstKey, TValue>(this IEnumerable<TSource> @this, Func<TSource, TFirstKey> firstKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var result = new Dictionary<TFirstKey, TValue>();

            var l = @this.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                result.Add(item.Key, aggregate(item));
            }

            return result;
        }
    }

    // Nested Types
    public enum SortingOrder
    {
        Asc,
        Desc
    }
}
