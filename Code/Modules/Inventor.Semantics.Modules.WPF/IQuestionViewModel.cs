using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Inventor.Semantics.Modules.WPF.ViewModels;
using Inventor.Semantics.Modules.WPF.ViewModels.Questions;

namespace Inventor.Semantics.Modules.WPF
{
	public interface IQuestionViewModel
	{
		ICollection<StatementViewModel> Preconditions
		{ get; }

		IQuestion BuildQuestion();
	}

	public interface IQuestionViewModel<out QuestionT> : IQuestionViewModel
		where QuestionT : IQuestion
	{
		new QuestionT BuildQuestion();
	}

	public abstract class QuestionViewModel<QuestionT> : IQuestionViewModel<QuestionT>
		where QuestionT : IQuestion
	{
		[PropertyDescriptor(true, "Names.Conditions")]
		public ICollection<StatementViewModel> Preconditions
		{ get; } = new ObservableCollection<StatementViewModel>();

		public QuestionT BuildQuestion()
		{
			replaceDecoratorsWithOriginalConcepts();

			var question = BuildQuestionImplementation();
			foreach (var statement in Preconditions)
			{
				question.Preconditions.Add(statement.CreateStatement());
			}
			return question;
		}

		public abstract QuestionT BuildQuestionImplementation();

		IQuestion IQuestionViewModel.BuildQuestion()
		{
			return BuildQuestion();
		}

		private void replaceDecoratorsWithOriginalConcepts()
		{
			var conceptProperties = GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty)
				.Where(p => p.PropertyType == typeof(IConcept));
			foreach (var property in conceptProperties)
			{
				var decorator = property.GetValue(this) as ConceptDecorator;
				if (decorator != null)
				{
					property.SetValue(this, decorator.Concept);
				}
			}
		}
	}
}
