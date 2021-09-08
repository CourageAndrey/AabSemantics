using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Inventor.Client.ViewModels;
using Inventor.Client.ViewModels.Questions;
using Inventor.Core;

namespace Inventor.Client
{
	public interface IQuestionViewModel
	{
		ICollection<StatementViewModel> Preconditions
		{ get; }

		Core.IQuestion BuildQuestion();
	}

	public interface IQuestionViewModel<out QuestionT> : IQuestionViewModel
		where QuestionT : Core.IQuestion
	{
		new QuestionT BuildQuestion();
	}

	public abstract class QuestionViewModel<QuestionT> : IQuestionViewModel<QuestionT>
		where QuestionT : Core.IQuestion
	{
		[PropertyDescriptor(true, "Names.ParamConditions")]
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

		Core.IQuestion IQuestionViewModel.BuildQuestion()
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
