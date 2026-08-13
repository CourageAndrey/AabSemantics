namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "is part of" question.</summary>
	public class IsPartOfQuestion : QuestionViewModel<Modules.Set.Questions.IsPartOfQuestion>
	{
		/// <summary>The more general concept.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public IConcept Parent
		{ get; set; }

		/// <summary>The more specific concept.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public IConcept Child
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
