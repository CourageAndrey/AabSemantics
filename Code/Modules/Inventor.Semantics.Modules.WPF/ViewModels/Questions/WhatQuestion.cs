namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class WhatQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.WhatQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.WhatQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.WhatQuestion(Concept);
		}
	}
}
