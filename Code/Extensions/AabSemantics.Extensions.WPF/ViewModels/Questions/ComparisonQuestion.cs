namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the comparison question.</summary>
	public class ComparisonQuestion : QuestionViewModel<Modules.Mathematics.Questions.ComparisonQuestion>
	{
		/// <summary>The left-hand value.</summary>
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public IConcept LeftValue
		{ get; set; }

		/// <summary>The right-hand value.</summary>
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public IConcept RightValue
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Mathematics.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Modules.Mathematics.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
