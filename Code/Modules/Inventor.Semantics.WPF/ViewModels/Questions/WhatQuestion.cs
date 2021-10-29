namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class WhatQuestion : QuestionViewModel<Semantics.Set.Questions.WhatQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.WhatQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.WhatQuestion(Concept);
		}
	}
}
