using System;
using System.Collections.Generic;
using System.IO;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class TextReaderExtensions
    {
        /// <summary>
        /// 	The method provides an iterator through all lines of the text @this.
        /// </summary>
        /// <param name = "@this">The text @this.</param>
        /// <returns>The iterator</returns>
        /// <example>
        /// 	<code>
        /// 		using(var @this = fileInfo.OpenText()) {
        /// 		foreach(var line in @this.IterateLines()) {
        /// 		// ...
        /// 		}
        /// 		}
        /// 	</code>
        /// </example>
        /// <remarks>
        /// 	Contributed by OlivierJ
        /// </remarks>
        public static IEnumerable<string> IterateLines(this TextReader @this)
        {
            string line = null;
            while ((line = @this.ReadLine()) != null)
                yield return line;
        }

        /// <summary>
        /// 	The method executes the passed delegate /lambda expression) for all lines of the text @this.
        /// </summary>
        /// <param name = "@this">The text @this.</param>
        /// <param name = "action">The action.</param>
        /// <example>
        /// 	<code>
        /// 		using(var @this = fileInfo.OpenText()) {
        /// 		@this.IterateLines(l => Console.WriteLine(l));
        /// 		}
        /// 	</code>
        /// </example>
        /// <remarks>
        /// 	Contributed by OlivierJ
        /// </remarks>
        public static void IterateLines(this TextReader @this, Action<string> action)
        {
            foreach (var line in @this.IterateLines())
                action(line);
        }
    }
}
