using System;
using System.Xml.Serialization;

namespace Inventor.Processes.Localization
{
	public interface ILanguageSystemConcepts
	{
		String StartsAfterOtherStarted
		{ get; }

		String StartsWhenOtherStarted
		{ get; }

		String StartsBeforeOtherStarted
		{ get; }

		String FinishesAfterOtherStarted
		{ get; }

		String FinishesWhenOtherStarted
		{ get; }

		String FinishesBeforeOtherStarted
		{ get; }

		String StartsAfterOtherFinished
		{ get; }

		String StartsWhenOtherFinished
		{ get; }

		String StartsBeforeOtherFinished
		{ get; }

		String FinishesAfterOtherFinished
		{ get; }

		String FinishesWhenOtherFinished
		{ get; }

		String FinishesBeforeOtherFinished
		{ get; }

		String Causes
		{ get; }

		String IsCausedBy
		{ get; }

		String SimultaneousWith
		{ get; }
	}

	[XmlType("ProcessesSystemConcepts")]
	public class LanguageSystemConcepts : ILanguageSystemConcepts
	{
		#region Properties

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
