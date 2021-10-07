namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class WhatQuestion : QuestionViewModel<Set.Questions.WhatQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Set.Questions.WhatQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.WhatQuestion(Concept);
		}
	}
}
