using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageStatements : ILanguageStatements
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

		public static LanguageStatements CreateDefaultNames()
		{
			return new LanguageStatements
			{
				SubjectArea = "Предметная область",
				Clasification = "Классификация",
				HasSign = "Признак",
				SignValue = "Значение признака",
				Composition = "Часть-целое",
				IsNotEqualTo = "Не равно",
				IsLessThan = "Меньше",
				IsLessThanOrEqualTo = "Меньше или равно",
				IsGreaterThan = "Больше",
				IsGreaterThanOrEqualTo = "Больше или равно",
				IsEqualTo = "Равно",
				Causes = "Вызывает",
				Meanwhile = "Одновременно с",
				After = "После",
				Before = "До",
			};
		}

		internal static LanguageStatements CreateDefaultHints()
		{
			return new LanguageStatements
			{
				SubjectArea = "Отношение описывает вхождение терминов в некоторую предметную область.",
				Clasification = "Отношение описывает связь между родительским и дочерним (подчинённым) понятиями.",
				HasSign = "Отношение описывает наличие некоторого признака, описывающего свойства понятия.",
				SignValue = "Отношение описывает значение признака, свойственного данному понятию.",
				Composition = "Отношение описывает вхождение одного понятия в другое качестве элемента структуры последнего.",
				IsNotEqualTo = "Отношение описывает два не равных значения.",
				IsLessThan = "Отношение описывает два значения, левое меньше правого.",
				IsLessThanOrEqualTo = "Отношение описывает два значения, левое меньше или равно правому.",
				IsGreaterThan = "Отношение описывает два значения, левое больше правого.",
				IsGreaterThanOrEqualTo = "Отношение описывает два значения, левое больше или равно правому.",
				IsEqualTo = "Отношение описывает два равных значения.",
				Causes = "Отношение описывает причинно-следственные связи.",
				Meanwhile = "Отношение описывает одновременность.",
				After = "Отношение описывает два процесса, второй следует за первым.",
				Before = "Отношение описывает два процесса, второй предшествует первому.",
			};
		}
	}
}
