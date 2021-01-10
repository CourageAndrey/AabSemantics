using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageStatementFormatStrings : ILanguageStatementFormatStrings
	{
		#region Properties

		[XmlElement]
		public String SubjectArea
		{ get; set; }

		[XmlElement]
		public String Clasification
		{ get; set; }

		[XmlElement]
		public String HasSign
		{ get; set; }

		[XmlElement]
		public String SignValue
		{ get; set; }

		[XmlElement]
		public String Composition
		{ get; set; }

		#endregion

		internal static LanguageStatementFormatStrings CreateDefaultTrue()
		{
			return new LanguageStatementFormatStrings
			{
				SubjectArea = string.Format("Понятие {0} входит в предметную область {1}.", Strings.ParamConcept, Strings.ParamArea),
				Clasification = string.Format("{0} есть {1}.", Strings.ParamChild, Strings.ParamParent),
				HasSign = string.Format("{0} имеет признак {1}.", Strings.ParamConcept, Strings.ParamSign),
				SignValue = string.Format("{0} имеет значение признака {1} равным {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = string.Format("{0} является частью {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		internal static LanguageStatementFormatStrings CreateDefaultFalse()
		{
			return new LanguageStatementFormatStrings
			{
				SubjectArea = string.Format("Понятие {0} не входит в предметную область {1}.", Strings.ParamConcept, Strings.ParamArea),
				Clasification = string.Format("{0} не есть {1}.", Strings.ParamChild, Strings.ParamParent),
				HasSign = string.Format("У {0} отсутствует признак {1}.", Strings.ParamConcept, Strings.ParamSign),
				SignValue = string.Format("{0} не имеет значение признака {1} равным {2}.", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = string.Format("{0} не является частью {1}.", Strings.ParamChild, Strings.ParamParent),
			};
		}

		internal static LanguageStatementFormatStrings CreateDefaultQuestion()
		{
			return new LanguageStatementFormatStrings
			{
				SubjectArea = string.Format("Входит ли понятие {0} в предметную область {1}?", Strings.ParamConcept, Strings.ParamArea),
				Clasification = string.Format("На самом ли деле {0} есть {1}?", Strings.ParamChild, Strings.ParamParent),
				HasSign = string.Format("Есть ли у {0} признак {1}?", Strings.ParamConcept, Strings.ParamSign),
				SignValue = string.Format("Является ли {2} значением признака {1} у {0}?", Strings.ParamConcept, Strings.ParamSign, Strings.ParamValue),
				Composition = string.Format("Является ли {0} частью {1}?", Strings.ParamChild, Strings.ParamParent),
			};
		}
	}
}
