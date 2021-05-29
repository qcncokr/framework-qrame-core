using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Qrame.CoreFX.Helper
{
	/*
        // -size=100 /height:'400' -param1 "Nice stuff !" --debug
        ArgumentHelper CommandLine=new ArgumentHelper(Args);

        // Look for specific arguments values and display 
        // them if they exist (return null if they don't)
        if(CommandLine["param1"] != null) 
            Console.WriteLine("Param1 value: " + 
                CommandLine["param1"]);
        else
            Console.WriteLine("Param1 not defined !");
     */
	public class ArgumentHelper
	{
		private StringDictionary parameters;

		// {-,/,--}param{ ,=,:}((",')value(",'))
		// -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
		public ArgumentHelper(string[] args)
		{
			parameters = new StringDictionary();
			Regex spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string parameter = null;
			string[] tokens;

			foreach (string arg in args)
			{
				tokens = spliter.Split(arg, 3);

				switch (tokens.Length)
				{
					case 1:
						if (parameter != null)
						{
							if (parameters.ContainsKey(parameter) == false)
							{
								tokens[0] = remover.Replace(tokens[0], "$1");

								parameters.Add(parameter, tokens[0]);
							}
							parameter = null;
						}
						break;
					case 2:
						if (parameter != null)
						{
							if (parameters.ContainsKey(parameter) == false)
							{
								parameters.Add(parameter, "true");
							}
						}
						parameter = tokens[1];
						break;
					case 3:
						if (parameter != null)
						{
							if (parameters.ContainsKey(parameter) == false)
							{
								parameters.Add(parameter, "true");
							}
						}

						parameter = tokens[1];

						if (parameters.ContainsKey(parameter) == false)
						{
							tokens[2] = remover.Replace(tokens[2], "$1");
							parameters.Add(parameter, tokens[2]);
						}

						parameter = null;
						break;
				}
			}
			
			if (parameter != null)
			{
				if (parameters.ContainsKey(parameter) == false)
				{
					parameters.Add(parameter, "true");
				}
			}
		}

		public string this[string Param]
		{
			get
			{
				return (parameters[Param]);
			}
		}
	}
}
