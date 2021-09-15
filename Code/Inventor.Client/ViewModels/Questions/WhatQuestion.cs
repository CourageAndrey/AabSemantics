namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class WhatQuestion : QuestionViewModel<Core.Questions.WhatQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.WhatQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.WhatQuestion(Concept);
		}
	}
}
