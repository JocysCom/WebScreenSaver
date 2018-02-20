using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JocysCom.ClassLibrary.Configuration
{
	public class InstallContext
	{
		private StringDictionary parameters;
		string log_file;
		bool log = false;
		public bool CaseSensitive { get; set; }

		public InstallContext()
		{
			log_file = null;
			log = false;
			parameters = ParseCommandLine(new string[0]);
		}

		public InstallContext(string logFilePath, string[] commandLine)
		{
			log_file = logFilePath;
			parameters = ParseCommandLine(commandLine);
			log = IsParameterTrue("LogtoConsole");
		}

		public StringDictionary Parameters
		{
			get
			{
				return parameters;
			}
		}

		public bool IsParameterTrue(string paramName)
		{
			var value = (parameters[paramName] ?? "").Trim().ToLower();
			return new string[] { "yes", "true", "1" }.Contains(value);
		}

		public void LogMessage(string message)
		{
			if (log)
			{
				Console.WriteLine(message);
			}
		}

		public static StringDictionary ParseCommandLine(string commandLine)
		{
			var regex = new Regex(@"((-|--|/)(?<key>\w+)([:= ]?((['""](?<value>.*?)['""])|(?<value>\w+))?))");
			var matches = regex.Matches((commandLine ?? "").Trim());
			var parameters = new StringDictionary();
			foreach (Match match in matches)
			{
				var key = match.Groups["key"].Value;
				var value = match.Groups["value"].Success ? match.Groups["value"].Value : null;
				parameters.Add(key, value);
			}
			return parameters;
		}

		public static StringDictionary ParseCommandLine(string[] args)
		{
			return ParseCommandLine(string.Join(" ", args));
		}

	}
}
