using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using AabSemantics.Extensions.WPF.ViewModels;
using AabSemantics.Extensions.WPF.ViewModels.Questions;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>An editable view over a question: its parameters plus any hypothetical preconditions.</summary>
	public interface IQuestionViewModel
	{
		/// <summary>Hypothetical statements the question assumes while being answered.</summary>
		ICollection<StatementViewModel> Preconditions
		{ get; }

		/// <summary>Builds the question from the edited values.</summary>
		/// <returns>The created question.</returns>
		IQuestion BuildQuestion();
	}

	/// <summary>A question view model that builds its question in the concrete type.</summary>
	/// <typeparam name="QuestionT">Question type being edited.</typeparam>
	public interface IQuestionViewModel<out QuestionT> : IQuestionViewModel
		where QuestionT : IQuestion
	{
		/// <summary>Builds the question from the edited values.</summary>
		/// <returns>The created question, strongly typed.</returns>
		new QuestionT BuildQuestion();
	}

	/// <summary>
	/// Base view model for questions. It collects the preconditions and reflects over the
	/// subclass's properties, which carry <c>PropertyDescriptor</c> attributes describing how the
	/// dialog should present them.
	/// </summary>
	/// <typeparam name="QuestionT">Question type being edited.</typeparam>
	public abstract class QuestionViewModel<QuestionT> : IQuestionViewModel<QuestionT>
		where QuestionT : IQuestion
	{
		/// <summary>Hypothetical statements the question assumes while being answered.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Conditions")]
		public ICollection<StatementViewModel> Preconditions
		{ get; } = new ObservableCollection<StatementViewModel>();

		/// <summary>Builds the question and attaches the edited preconditions to it.</summary>
		/// <returns>The created question.</returns>
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

		/// <summary>Builds the question from the subclass's own properties, without preconditions.</summary>
		/// <returns>The created question.</returns>
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
