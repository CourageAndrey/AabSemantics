using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>Texts for the comparison sign concepts; reused for both names and hints.</summary>
	public interface ILanguageSystemConcepts
	{
		/// <summary>Text for the "equal to" sign.</summary>
		String IsEqualTo
		{ get; }

		/// <summary>Text for the "not equal to" sign.</summary>
		String IsNotEqualTo
		{ get; }

		/// <summary>Text for the "greater than or equal to" sign.</summary>
		String IsGreaterThanOrEqualTo
		{ get; }

		/// <summary>Text for the "greater than" sign.</summary>
		String IsGreaterThan
		{ get; }

		/// <summary>Text for the "less than or equal to" sign.</summary>
		String IsLessThanOrEqualTo
		{ get; }

		/// <summary>Text for the "less than" sign.</summary>
		String IsLessThan
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageSystemConcepts"/>, loaded from a language file.</summary>
	[XmlType("MathematicsSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		/// <summary>Text for the "equal to" sign.</summary>
		[XmlElement]
		public String IsEqualTo
		{ get; set; }

		/// <summary>Text for the "not equal to" sign.</summary>
		[XmlElement]
		public String IsNotEqualTo
		{ get; set; }

		/// <summary>Text for the "greater than or equal to" sign.</summary>
		[XmlElement]
		public String IsGreaterThanOrEqualTo
		{ get; set; }

		/// <summary>Text for the "greater than" sign.</summary>
		[XmlElement]
		public String IsGreaterThan
		{ get; set; }

		/// <summary>Text for the "less than or equal to" sign.</summary>
		[XmlElement]
		public String IsLessThanOrEqualTo
		{ get; set; }

		/// <summary>Text for the "less than" sign.</summary>
		[XmlElement]
		public String IsLessThan
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				IsEqualTo = " = ",
				IsNotEqualTo = " ≠ ",
				IsGreaterThanOrEqualTo = " ≥ ",
				IsGreaterThan = " > ",
				IsLessThanOrEqualTo = " ≤ ",
				IsLessThan = " < ",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				IsEqualTo = "Comparison: is equal to.",
				IsNotEqualTo = "Comparison: is not equal to.",
				IsGreaterThanOrEqualTo = "Comparison: greater than or equal to.",
				IsGreaterThan = "Comparison: greater than.",
				IsLessThanOrEqualTo = "Comparison: less than or equal to.",
				IsLessThan = "Comparison: less than.",
			};
		}
	}
}
