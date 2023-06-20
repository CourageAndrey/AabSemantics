namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class WhatQuestion : QuestionViewModel<Modules.Set.Questions.WhatQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.WhatQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.WhatQuestion(Concept);
		}
	}
}
