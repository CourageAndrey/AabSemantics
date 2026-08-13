namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "is a" question.</summary>
	public class IsQuestion : QuestionViewModel<Modules.Classification.Questions.IsQuestion>
	{
		/// <summary>The more specific concept.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public IConcept Child
		{ get; set; }

		/// <summary>The more general concept.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public IConcept Parent
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Classification.Questions.IsQuestion BuildQuestionImplementation()
		{
			return new Modules.Classification.Questions.IsQuestion(Child, Parent);
		}
	}
}
