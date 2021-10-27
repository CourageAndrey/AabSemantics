﻿namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSubjectAreaQuestion : QuestionViewModel<Set.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Area")]
		public Semantics.IConcept Area
		{ get; set; }

		public override Set.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
