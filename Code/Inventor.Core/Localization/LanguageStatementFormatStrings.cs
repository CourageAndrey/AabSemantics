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

		[XmlElement]
		public String IsNotEqualTo
		{ get; set; }

		[XmlElement]
		public String IsLessThan
		{ get; set; }

		[XmlElement]
		public String IsLessThanOrEqualTo
		{ get; set; }

		[XmlElement]
		public String IsGreaterThan
		{ get; set; }

		[XmlElement]
		public String IsGreaterThanOrEqualTo
		{ get; set; }

		[XmlElement]
		public String IsEqualTo
		{ get; set; }

		[XmlElement]
		public String Causes
		{ get; set; }

		[XmlElement]
		public String Meanwhile
		{ get; set; }

		[XmlElement]
		public String After
		{ get; set; }

		[XmlElement]
		public String Before
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
				IsNotEqualTo = string.Format("{0} ≠ {1}.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsLessThan = string.Format("{0} < {1}.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsLessThanOrEqualTo = string.Format("{0} ≤ {1}.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsGreaterThan = string.Format("{0} > {1}.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsGreaterThanOrEqualTo = string.Format("{0} ≥ {1}.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsEqualTo = string.Format("{0} = {1}.", Strings.ParamLeftValue, Strings.ParamRightValue),
				Causes = string.Format("{0} является причиной {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				Meanwhile = string.Format("{0} идёт одновременно с {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				After = string.Format("После {0} идёт {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				Before = string.Format("Перед {0} идёт {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
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
				IsNotEqualTo = string.Format("Выражение {0} ≠ {1} неверно.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsLessThan = string.Format("Выражение {0} < {1} неверно.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsLessThanOrEqualTo = string.Format("Выражение {0} ≤ {1} неверно.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsGreaterThan = string.Format("Выражение {0} > {1} неверно.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsGreaterThanOrEqualTo = string.Format("Выражение {0} ≥ {1} неверно.", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsEqualTo = string.Format("Выражение {0} = {1} неверно.", Strings.ParamLeftValue, Strings.ParamRightValue),
				Causes = string.Format("{0} не является причиной {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				Meanwhile = string.Format("{0} не идёт одновременно с {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				After = string.Format("После {0} не идёт {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				Before = string.Format("Перед {0} не идёт {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
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
				IsNotEqualTo = string.Format("Верно ли выражение {0} ≠ {1}?", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsLessThan = string.Format("Верно ли выражение {0} < {1}?", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsLessThanOrEqualTo = string.Format("Верно ли выражение {0} ≤ {1}?", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsGreaterThan = string.Format("Верно ли выражение {0} > {1}?", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsGreaterThanOrEqualTo = string.Format("Верно ли выражение {0} ≥ {1}?", Strings.ParamLeftValue, Strings.ParamRightValue),
				IsEqualTo = string.Format("Верно ли выражение {0} = {1}?", Strings.ParamLeftValue, Strings.ParamRightValue),
				Causes = string.Format("Является {0} ли причиной {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				Meanwhile = string.Format("Идёт {0} ли одновременно с {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				After = string.Format("Идёт после {0} ли {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
				Before = string.Format("Идёт перед {0} ли {1}.", Strings.ParamProcessA, Strings.ParamProcessB),
			};
		}
	}
}
