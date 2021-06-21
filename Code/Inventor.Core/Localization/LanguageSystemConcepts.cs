using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		#region Logical values

		[XmlElement]
		public String True
		{ get; set; }

		[XmlElement]
		public String False
		{ get; set; }

		#endregion

		#region Comparison signs

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

		#region Sequence signs

		[XmlElement]
		public String StartsAfterOtherStarted
		{ get; set; }

		[XmlElement]
		public String StartsWhenOtherStarted
		{ get; set; }

		[XmlElement]
		public String StartsBeforeOtherStarted
		{ get; set; }

		[XmlElement]
		public String FinishesAfterOtherStarted
		{ get; set; }

		[XmlElement]
		public String FinishesWhenOtherStarted
		{ get; set; }

		[XmlElement]
		public String FinishesBeforeOtherStarted
		{ get; set; }

		[XmlElement]
		public String StartsAfterOtherFinished
		{ get; set; }

		[XmlElement]
		public String StartsWhenOtherFinished
		{ get; set; }

		[XmlElement]
		public String StartsBeforeOtherFinished
		{ get; set; }

		[XmlElement]
		public String FinishesAfterOtherFinished
		{ get; set; }

		[XmlElement]
		public String FinishesWhenOtherFinished
		{ get; set; }

		[XmlElement]
		public String FinishesBeforeOtherFinished
		{ get; set; }

		[XmlElement]
		public String Causes
		{ get; set; }

		[XmlElement]
		public String IsCausedBy
		{ get; set; }

		[XmlElement]
		public String SimultaneousWith
		{ get; set; }

		#endregion

		#endregion

		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				True = "Да",
				False = "Нет",

				IsEqualTo = " = ",
				IsNotEqualTo = " ≠ ",
				IsGreaterThanOrEqualTo = " ≥ ",
				IsGreaterThan = " > ",
				IsLessThanOrEqualTo = " ≤ ",
				IsLessThan = " < ",

				StartsAfterOtherStarted = " начинается после начала ",
				StartsWhenOtherStarted = " начинается одновременно с началом ",
				StartsBeforeOtherStarted = " начинается до начала ",
				FinishesAfterOtherStarted = " завершается после начала ",
				FinishesWhenOtherStarted = " завершается одновременно с началом ",
				FinishesBeforeOtherStarted = " завершается до начала ",
				StartsAfterOtherFinished = " начинается после завершения ",
				StartsWhenOtherFinished = " начинается одновременно с завершением ",
				StartsBeforeOtherFinished = " начинается до завершения ",
				FinishesAfterOtherFinished = " завершается после завершения ",
				FinishesWhenOtherFinished = " завершается одновременно с завершением ",
				FinishesBeforeOtherFinished = " завершается до завершения ",
				Causes = " вызывает ",
				IsCausedBy = " вызывается ",
				SimultaneousWith = " идёт одновременно с ",
			};
		}

		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				True = "Логическое значение: истина.",
				False = "Логическое значение: ложь.",

				IsEqualTo = "Сравнение: равно.",
				IsNotEqualTo = "Сравнение: не равно.",
				IsGreaterThanOrEqualTo = "Сравнение: больше или равно.",
				IsGreaterThan = "Сравнение: больше.",
				IsLessThanOrEqualTo = "Сравнение: меньше или равно.",
				IsLessThan = "Сравнение: меньше.",

				StartsAfterOtherStarted = "Процессы: начинается после начала ...",
				StartsWhenOtherStarted = "Процессы: начинается одновременно с началом ...",
				StartsBeforeOtherStarted = "Процессы: начинается до начала ...",
				FinishesAfterOtherStarted = "Процессы: завершается после начала ...",
				FinishesWhenOtherStarted = "Процессы: завершается одновременно с началом ...",
				FinishesBeforeOtherStarted = "Процессы: завершается до начала ...",
				StartsAfterOtherFinished = "Процессы: начинается после завершения ...",
				StartsWhenOtherFinished = "Процессы: начинается одновременно с завершением ...",
				StartsBeforeOtherFinished = "Процессы: начинается до завершения ...",
				FinishesAfterOtherFinished = "Процессы: завершается после завершения ...",
				FinishesWhenOtherFinished = "Процессы: завершается одновременно с завершением ...",
				FinishesBeforeOtherFinished = "Процессы: завершается до завершения ...",
				Causes = "Процессы: вызывает ...",
				IsCausedBy = "Процессы: вызывается ...",
				SimultaneousWith = "Процессы: идёт одновременно с ...",
			};
		}
	}
}
