using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>Wordings for the set module's consistency problems.</summary>
	public interface ILanguageConsistency
	{
		/// <summary>Message reporting the same sign declared more than once for a concept.</summary>
		String ErrorMultipleSign
		{ get; }

		/// <summary>Message reporting more than one value declared for the same sign.</summary>
		String ErrorMultipleSignValue
		{ get; }

		/// <summary>Message reporting a sign value inherited from several ancestors at once.</summary>
		String ErrorMultipleSignValueParents
		{ get; }

		/// <summary>Message reporting a declared sign that has no value.</summary>
		String ErrorSignWithoutValue
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageConsistency"/>, loaded from a language file.</summary>
	[XmlType("SetsConsistency")]
	public class LanguageConsistency : ILanguageConsistency
	{
		#region Properties

		/// <summary>Message reporting the same sign declared more than once for a concept.</summary>
		[XmlElement]
		public String ErrorMultipleSign
		{ get; set; }

		/// <summary>Message reporting more than one value declared for the same sign.</summary>
		[XmlElement]
		public String ErrorMultipleSignValue
		{ get; set; }

		/// <summary>Message reporting a sign value inherited from several ancestors at once.</summary>
		[XmlElement]
		public String ErrorMultipleSignValueParents
		{ get; set; }

		/// <summary>Message reporting a declared sign that has no value.</summary>
		[XmlElement]
		public String ErrorSignWithoutValue
		{ get; set; }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
