using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Processes.Localization
{
	/// <summary>Wordings for the processes module's consistency problems.</summary>
	public interface ILanguageConsistency
	{
		/// <summary>Message reporting contradicting process sequence statements.</summary>
		String ErrorProcessesContradiction
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageConsistency"/>, loaded from a language file.</summary>
	[XmlType("ProcessesConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		/// <summary>Message reporting contradicting process sequence statements.</summary>
		[XmlElement]
		public String ErrorProcessesContradiction
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				ErrorProcessesContradiction = $"Impossible to detect sequence between {Strings.ParamProcessA} and {Strings.ParamProcessB}. Possible cases: ",
			};
		}
	}
}
