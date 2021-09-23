using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Classification
{
	public interface ILanguageAnswers
	{
		String IsTrue
		{ get; }

		String IsFalse
		{ get; }

		String EnumerateAncestors
		{ get; }

		String EnumerateDescendants
		{ get; }
	}

	[XmlType("ClassificationAnswers")]
	public class LanguageAnswers : ILanguageAnswers
	{
		#region Properties

		[XmlElement]
		public String IsTrue
		{ get; set; }

		[XmlElement]
		public String IsFalse
		{ get; set; }

		[XmlElement]
		public String EnumerateAncestors
		{ get; set; }

		[XmlElement]
		public String EnumerateDescendants
		{ get; set; }

		#endregion

		internal static LanguageAnswers CreateDefault()
		{
			return new LanguageAnswers
			{
				IsTrue = $"Yes, {Strings.ParamChild} is {Strings.ParamParent}.",
				IsFalse = $"No, {Strings.ParamChild} is not {Strings.ParamParent}.",
				EnumerateAncestors = $"{Strings.ParamChild} is:",
				EnumerateDescendants = $"{Strings.ParamParent} can be following:",
			};
		}
	}
}
