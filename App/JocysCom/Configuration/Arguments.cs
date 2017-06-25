using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;

namespace JocysCom.ClassLibrary.Configuration
{
	/// <summary>
	/// Arguments class.
	/// </summary>
	public class Arguments : StringDictionary
	{

		public Arguments(string[] args)
		{
			Regex spliter = new Regex(@"^-{1,2}|^/|=|:",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

			Regex remover = new Regex(@"^['""]?(.*?)['""]?$",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string Parameter = null;
			string[] Parts;

			// Valid parameters forms:
			// (-|--|/)param( |=|:)[("|')]value[("|')]
			// Examples: 
			//   -param1 value1 --param2 /param3:"Test-:-work" 
			//   /param4=happy -param5 '--=nice=--'
			foreach (string Txt in args)
			{
				// Look for new parameters (-,/ or --) and a
				// possible enclosed value (=,:)
				Parts = spliter.Split(Txt, 3);
				switch (Parts.Length)
				{
					// Found a value (for the last parameter 
					// found (space separator))
					case 1:
						if (Parameter != null)
						{
							if (!base.ContainsKey(Parameter))
							{
								Parts[0] = remover.Replace(Parts[0], "$1");
								base.Add(Parameter, Parts[0]);
							}
							Parameter = null;
						}
						// else Error: no parameter waiting for a value (skipped)
						break;

					// Found just a parameter
					case 2:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if (Parameter != null)
						{
							if (!base.ContainsKey(Parameter))
								base.Add(Parameter, null);
						}
						Parameter = Parts[1];
						break;

					// Parameter with enclosed value
					case 3:
						// The last parameter is still waiting. 
						// With no value, set it to true.
						if (Parameter != null)
						{
							if (!base.ContainsKey(Parameter))
								base.Add(Parameter, null);
						}

						Parameter = Parts[1];

						// Remove possible enclosing characters (",')
						if (!base.ContainsKey(Parameter))
						{
							Parts[2] = remover.Replace(Parts[2], "$1");
							base.Add(Parameter, Parts[2]);
						}
						Parameter = null;
						break;
				}
			}
			// In case a parameter is still waiting
			if (Parameter != null)
			{
				if (!base.ContainsKey(Parameter))
					base.Add(Parameter, null);
			}
		}

		/// <summary>
		/// Get value by specific key.
		/// </summary>
		/// <param name="key">The key to locate in the Arguments.</param>
		/// <param name="ignoreCase">True to ignore case during key comparison; otherwise, false.</param>
		/// <returns>String value.</returns>
		public string GetValue(string key, bool ignoreCase = false)
		{
			var keyValue = Keys.Cast<string>().FirstOrDefault(x => string.Compare(x, key, ignoreCase) == 0);
			return keyValue == null ? null : this[keyValue];
		}

		/// <summary>
		/// Determines if the Arguments contains a specific key.
		/// </summary>
		/// <param name="key">The key to locate in the Arguments.</param>
		/// <param name="ignoreCase">True to ignore case during key comparison; otherwise, false.</param>
		/// <returns>True if the Arguments contains an entry with the specified key; otherwise, false.</returns>
		public bool ContainsKey(string key, bool ignoreCase)
		{
			return Keys
				.Cast<string>()
				.Any(x => string.Compare(x, key, ignoreCase) == 0);
		}

		public T GetValue<T>(string key, bool ignoreCase = false) where T : struct
		{
			var valueString = GetValue(key, ignoreCase);
			T result;
			TryParse(valueString, out result);
			return result;
		}

		/// <summary>
		/// Tries to convert the specified string representation of a logical value to
		/// its type T equivalent. A return value indicates whether the conversion
		/// succeeded or failed.
		/// </summary>
		/// <typeparam name="T">The type to try and convert to.</typeparam>
		/// <param name="value">A string containing the value to try and convert.</param>
		/// <param name="result">If the conversion was successful, the converted value of type T.</param>
		/// <returns>If value was converted successfully, true; otherwise false.</returns>
		static bool TryParse<T>(string value, out T result) where T : struct
		{
			var tryParseMethod = typeof(T).GetMethod("TryParse",
				BindingFlags.Static | BindingFlags.Public, null,
				new[] { typeof(string), typeof(T).MakeByRefType() }, null);
			var parameters = new object[] { value, null };
			var retVal = (bool)tryParseMethod.Invoke(null, parameters);
			result = (T)parameters[1];
			return retVal;
		}

	}
}