using System.Collections.ObjectModel;
using System.Linq;

namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	public class CustomStatementQuestion : QuestionViewModel<AabSemantics.Questions.CustomStatementQuestion>
	{
		[PropertyDescriptor(true, "WpfUiModule\\Ui.Editing.PropertyType")]
		public string Type
		{ get; set; }

		[PropertyDescriptor(true, "WpfUiModule\\Ui.Editing.PropertyConcepts")]
		public ObservableCollection<ConceptWithKey> Concepts
		{ get; set; }

		public CustomStatementQuestion()
		{
			Concepts = new ObservableCollection<ConceptWithKey>();
		}

		public override AabSemantics.Questions.CustomStatementQuestion BuildQuestionImplementation()
		{
			return new AabSemantics.Questions.CustomStatementQuestion(Type, Concepts.ToDictionary(
				c => c.Key,
				c => c.Concept as IConcept));
		}
	}
}
