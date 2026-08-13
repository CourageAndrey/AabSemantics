namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the concept description question.</summary>
	public class WhatQuestion : QuestionViewModel<Modules.Set.Questions.WhatQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.WhatQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.WhatQuestion(Concept);
		}
	}
}
