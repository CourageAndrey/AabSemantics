using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.Processes.Localization
{
	public interface ILanguageConsistency
	{
		String ErrorProcessesContradiction
		{ get; }
	}

	[XmlType("ProcessesConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public String ErrorProcessesContradiction
		{ get; set; }

		#endregion

		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				ErrorProcessesContradiction = $"Impossible to detect sequence between {Strings.ParamProcessA} and {Strings.ParamProcessB}. Possible cases: ",
			};
		}
	}
}
