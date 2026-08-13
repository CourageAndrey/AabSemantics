using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>
	/// Texts for the two logical value concepts. The same type serves both names and hints,
	/// which is why the defaults come from two separate factory methods.
	/// </summary>
	public interface ILanguageSystemConcepts
	{
		/// <summary>Text for the logical "true" concept.</summary>
		String True
		{ get; }

		/// <summary>Text for the logical "false" concept.</summary>
		String False
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageSystemConcepts"/>, loaded from a language file.</summary>
	[XmlType("BooleanSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		/// <summary>Text for the logical "true" concept.</summary>
		[XmlElement]
		public String True
		{ get; set; }

		/// <summary>Text for the logical "false" concept.</summary>
		[XmlElement]
		public String False
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				True = "true",
				False = "false",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				True = "Boolean: true.",
				False = "Boolean: false.",
			};
		}
	}
}
