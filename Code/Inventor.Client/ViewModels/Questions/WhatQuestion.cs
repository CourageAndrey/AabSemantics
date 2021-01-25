namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class WhatQuestion : QuestionViewModel<Core.Questions.WhatQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.WhatQuestion BuildQuestion()
		{
			return new Core.Questions.WhatQuestion(Concept);
		}
	}
}
