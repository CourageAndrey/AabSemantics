using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	/// <summary>
	/// Asks whether one concept is a kind of another. Superseded by
	/// <see cref="Boolean.Questions.CheckStatementQuestion"/> with an
	/// <see cref="IsStatement"/>; kept only until the WPF question dialog supports that.
	/// </summary>
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsQuestion : Question
	{
		#region Properties

		/// <summary>The concept asked about.</summary>
		public IConcept Child
		{ get; }

		/// <summary>The concept it is checked against.</summary>
		public IConcept Parent
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="child">The concept asked about.</param>
		/// <param name="parent">The concept it is checked against.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="ArgumentNullException">Either concept is <c>null</c>.</exception>
		public IsQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Child = child.EnsureNotNull(nameof(child));
			Parent = parent.EnsureNotNull(nameof(parent));
		}

		/// <summary>
		/// Answers yes or no. When no direct "is a" statement matches, the question is re-asked of
		/// each of the child's own ancestors, so the relation is found transitively.
		/// </summary>
		/// <param name="context">Context to search.</param>
		/// <returns>A yes/no answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<IsQuestion, IsStatement>()
				.WithTransitives(
					statements => Task.FromResult(statements.Count == 0),
					question => question.Child,
					newSubject => new IsQuestion(newSubject, Parent))
				.Where(s => s.Parent == Parent && s.Child == Child)
				.SelectBooleanIncludingChildrenAsync(
					statements => statements.Count > 0,
					language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Answers.IsTrue,
					language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Answers.IsFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamParent, Parent },
						{ Strings.ParamChild, Child },
					}).ConfigureAwait(false);
		}
	}
}
