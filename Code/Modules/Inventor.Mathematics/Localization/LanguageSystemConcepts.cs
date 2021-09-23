using System;
using System.Xml.Serialization;

namespace Inventor.Mathematics.Localization
{
	public interface ILanguageSystemConcepts
	{
		String IsEqualTo
		{ get; }

		String IsNotEqualTo
		{ get; }

		String IsGreaterThanOrEqualTo
		{ get; }

		String IsGreaterThan
		{ get; }

		String IsLessThanOrEqualTo
		{ get; }

		String IsLessThan
		{ get; }
	}

	[XmlType("MathematicsSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		[XmlElement]
		public String IsEqualTo
		{ get; set; }

		[XmlElement]
		public String IsNotEqualTo
		{ get; set; }

		[XmlElement]
		public String IsGreaterThanOrEqualTo
		{ get; set; }

		[XmlElement]
		public String IsGreaterThan
		{ get; set; }

		[XmlElement]
		public String IsLessThanOrEqualTo
		{ get; set; }

		[XmlElement]
		public String IsLessThan
		{ get; set; }

		#endregion

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
