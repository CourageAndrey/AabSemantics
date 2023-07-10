using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	public interface ILanguageConsistency
	{
		String ErrorMultipleSign
		{ get; }

		String ErrorMultipleSignValue
		{ get; }

		String ErrorMultipleSignValueParents
		{ get; }

		String ErrorSignWithoutValue
		{ get; }
	}

	[XmlType("SetsConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		[XmlElement]
		public String ErrorMultipleSign
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSignValue
		{ get; set; }

		[XmlElement]
		public String ErrorMultipleSignValueParents
		{ get; set; }

		[XmlElement]
		public String ErrorSignWithoutValue
		{ get; set; }

		#endregion

		internal static LanguageConsistency CreateDefault()
		{
			return new LanguageConsistency
			{
				ErrorMultipleSign = $"Statement {AabSemantics.Localization.Strings.ParamStatement} cause sign value overload.",
				ErrorMultipleSignValue = $"Value of {Strings.ParamSign} sign of {AabSemantics.Localization.Strings.ParamConcept} concept is uncertain, because its value set multiple times.",
				ErrorMultipleSignValueParents = $"Value of {Strings.ParamSign} sign of {AabSemantics.Localization.Strings.ParamConcept} concept is uncertain, because many ancestors define their own values.",
				ErrorSignWithoutValue = $"{AabSemantics.Localization.Strings.ParamStatement} defines value of sign, which does not belong to concept.",
			};
		}
	}
}
