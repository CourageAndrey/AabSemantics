using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules.Set
{
	public interface ILanguageQuestionParameters
	{
		String ParamSign
		{ get; }

		String ParamArea
		{ get; }

		String ParamConcept1
		{ get; }

		String ParamConcept2
		{ get; }
	}

	[XmlType("SetsQuestionParameters")]
	public class LanguageQuestionParameters : ILanguageQuestionParameters
	{
		#region Properties

		[XmlElement]
		public String ParamSign
		{ get; set; }

		[XmlElement]
		public String ParamArea
		{ get; set; }

		[XmlElement]
		public String ParamConcept1
		{ get; set; }

		[XmlElement]
		public String ParamConcept2
		{ get; set; }

		#endregion

		internal static LanguageQuestionParameters CreateDefault()
		{
			return new LanguageQuestionParameters
			{
				ParamSign = "SIGN",
				ParamArea = "SUBJECT_AREA",
				ParamConcept1 = "Concept 1",
				ParamConcept2 = "Concept 2",
			};
		}
	}
}
