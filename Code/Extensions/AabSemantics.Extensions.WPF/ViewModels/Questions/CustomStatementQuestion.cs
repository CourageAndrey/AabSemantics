using System.Collections.ObjectModel;
using System.Linq;

namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the custom statement lookup question.</summary>
	public class CustomStatementQuestion : QuestionViewModel<AabSemantics.Questions.CustomStatementQuestion>
	{
		/// <summary>Identifier of the declared statement kind.</summary>
		[PropertyDescriptor(true, "WpfUiModule\\Ui.Editing.PropertyType")]
		public string Type
		{ get; set; }

		/// <summary>Concepts filling the kind's roles.</summary>
		[PropertyDescriptor(true, "WpfUiModule\\Ui.Editing.PropertyConcepts")]
		public ObservableCollection<ConceptWithKey> Concepts
		{ get; set; }

		/// <summary>Creates an empty view model.</summary>
		public CustomStatementQuestion()
		{
			Concepts = new ObservableCollection<ConceptWithKey>();
		}

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override AabSemantics.Questions.CustomStatementQuestion BuildQuestionImplementation()
		{
			return new AabSemantics.Questions.CustomStatementQuestion(Type, Concepts.ToDictionary(
				c => c.Key,
				c => c.Concept as IConcept));
		}
	}
}
