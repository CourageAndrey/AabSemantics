using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.Processes.Localization
{
	/// <summary>Texts for the sequence sign concepts; reused for both names and hints.</summary>
	public interface ILanguageSystemConcepts
	{
		/// <summary>Text for the "starts after the other started" sign.</summary>
		String StartsAfterOtherStarted
		{ get; }

		/// <summary>Text for the "starts when the other starts" sign.</summary>
		String StartsWhenOtherStarted
		{ get; }

		/// <summary>Text for the "starts before the other starts" sign.</summary>
		String StartsBeforeOtherStarted
		{ get; }

		/// <summary>Text for the "finishes after the other started" sign.</summary>
		String FinishesAfterOtherStarted
		{ get; }

		/// <summary>Text for the "finishes when the other starts" sign.</summary>
		String FinishesWhenOtherStarted
		{ get; }

		/// <summary>Text for the "finishes before the other starts" sign.</summary>
		String FinishesBeforeOtherStarted
		{ get; }

		/// <summary>Text for the "starts after the other finished" sign.</summary>
		String StartsAfterOtherFinished
		{ get; }

		/// <summary>Text for the "starts when the other finishes" sign.</summary>
		String StartsWhenOtherFinished
		{ get; }

		/// <summary>Text for the "starts before the other finishes" sign.</summary>
		String StartsBeforeOtherFinished
		{ get; }

		/// <summary>Text for the "finishes after the other finished" sign.</summary>
		String FinishesAfterOtherFinished
		{ get; }

		/// <summary>Text for the "finishes when the other finishes" sign.</summary>
		String FinishesWhenOtherFinished
		{ get; }

		/// <summary>Text for the "finishes before the other finishes" sign.</summary>
		String FinishesBeforeOtherFinished
		{ get; }

		/// <summary>Text for the "causes the other" sign.</summary>
		String Causes
		{ get; }

		/// <summary>Text for the "is caused by the other" sign.</summary>
		String IsCausedBy
		{ get; }

		/// <summary>Text for the "runs simultaneously with the other" sign.</summary>
		String SimultaneousWith
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageSystemConcepts"/>, loaded from a language file.</summary>
	[XmlType("ProcessesSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

		/// <summary>Text for the "starts after the other started" sign.</summary>
		[XmlElement]
		public String StartsAfterOtherStarted
		{ get; set; }

		/// <summary>Text for the "starts when the other starts" sign.</summary>
		[XmlElement]
		public String StartsWhenOtherStarted
		{ get; set; }

		/// <summary>Text for the "starts before the other starts" sign.</summary>
		[XmlElement]
		public String StartsBeforeOtherStarted
		{ get; set; }

		/// <summary>Text for the "finishes after the other started" sign.</summary>
		[XmlElement]
		public String FinishesAfterOtherStarted
		{ get; set; }

		/// <summary>Text for the "finishes when the other starts" sign.</summary>
		[XmlElement]
		public String FinishesWhenOtherStarted
		{ get; set; }

		/// <summary>Text for the "finishes before the other starts" sign.</summary>
		[XmlElement]
		public String FinishesBeforeOtherStarted
		{ get; set; }

		/// <summary>Text for the "starts after the other finished" sign.</summary>
		[XmlElement]
		public String StartsAfterOtherFinished
		{ get; set; }

		/// <summary>Text for the "starts when the other finishes" sign.</summary>
		[XmlElement]
		public String StartsWhenOtherFinished
		{ get; set; }

		/// <summary>Text for the "starts before the other finishes" sign.</summary>
		[XmlElement]
		public String StartsBeforeOtherFinished
		{ get; set; }

		/// <summary>Text for the "finishes after the other finished" sign.</summary>
		[XmlElement]
		public String FinishesAfterOtherFinished
		{ get; set; }

		/// <summary>Text for the "finishes when the other finishes" sign.</summary>
		[XmlElement]
		public String FinishesWhenOtherFinished
		{ get; set; }

		/// <summary>Text for the "finishes before the other finishes" sign.</summary>
		[XmlElement]
		public String FinishesBeforeOtherFinished
		{ get; set; }

		/// <summary>Text for the "causes the other" sign.</summary>
		[XmlElement]
		public String Causes
		{ get; set; }

		/// <summary>Text for the "is caused by the other" sign.</summary>
		[XmlElement]
		public String IsCausedBy
		{ get; set; }

		/// <summary>Text for the "runs simultaneously with the other" sign.</summary>
		[XmlElement]
		public String SimultaneousWith
		{ get; set; }

		#endregion

		/// <summary>Builds the built-in English display names.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageSystemConcepts CreateDefaultNames()
		{
			return new LanguageSystemConcepts
			{
				StartsAfterOtherStarted = " starts after other started ",
				StartsWhenOtherStarted = " starts when other started ",
				StartsBeforeOtherStarted = " starts before other started ",
				FinishesAfterOtherStarted = " finishes after other started ",
				FinishesWhenOtherStarted = " finishes when other started ",
				FinishesBeforeOtherStarted = " finishes before other started ",
				StartsAfterOtherFinished = " starts after other finished ",
				StartsWhenOtherFinished = " starts when other finished ",
				StartsBeforeOtherFinished = " starts before other finished ",
				FinishesAfterOtherFinished = " finishes after other finished ",
				FinishesWhenOtherFinished = " finishes when other finished ",
				FinishesBeforeOtherFinished = " finishes before other finished ",
				Causes = " causes ",
				IsCausedBy = " is caused by ",
				SimultaneousWith = " is simultaneous with ",
			};
		}

		/// <summary>Builds the built-in English tooltip texts.</summary>
		/// <returns>A populated part.</returns>
		internal static LanguageSystemConcepts CreateDefaultHints()
		{
			return new LanguageSystemConcepts
			{
				StartsAfterOtherStarted = "Processes: starts after other started ...",
				StartsWhenOtherStarted = "Processes: starts when other started ...",
				StartsBeforeOtherStarted = "Processes: starts before other started ...",
				FinishesAfterOtherStarted = "Processes: finishes after other started ...",
				FinishesWhenOtherStarted = "Processes: finishes when other started ...",
				FinishesBeforeOtherStarted = "Processes: finishes before other started ...",
				StartsAfterOtherFinished = "Processes: starts after other finished ...",
				StartsWhenOtherFinished = "Processes: starts when other finished ...",
				StartsBeforeOtherFinished = "Processes: starts before other finished ...",
				FinishesAfterOtherFinished = "Processes: finishes after other finished ...",
				FinishesWhenOtherFinished = "Processes: finishes when other finished ...",
				FinishesBeforeOtherFinished = "Processes: finishes before other finished ...",
				Causes = "Processes: causes ...",
				IsCausedBy = "Processes: is caused by ...",
				SimultaneousWith = "Processes: is simultaneous with ...",
			};
		}
	}
}
