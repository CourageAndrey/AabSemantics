using System;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Modules.Classification.Localization
{
	public interface ILanguageConsistency
	{
		String ErrorCyclic
		{ get; }
	}

	[XmlType("ClassificationConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public String ErrorCyclic
		{ get; set; }

		#endregion

		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				ErrorCyclic = $"Statement {Strings.ParamStatement} causes cyclic references.",
			};
		}
	}
}
