using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	/// <summary>Asks whether one concept is part of another.</summary>
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsPartOfQuestion : Question
	{
		#region Properties

		/// <summary>Same as the containing side, under the generic hierarchy naming.</summary>
		public IConcept Parent
		{ get; }

		/// <summary>Same as the contained side, under the generic hierarchy naming.</summary>
		public IConcept Child
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="child">Same as the contained side, under the generic hierarchy naming.</param>
		/// <param name="parent">Same as the containing side, under the generic hierarchy naming.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public IsPartOfQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Child = child.EnsureNotNull(nameof(child));
			Parent = parent.EnsureNotNull(nameof(parent));
		}

		/// <summary>Derives the answer from the network's statements.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>The answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<IsPartOfQuestion, HasPartStatement>()
				.Where(s => s.Whole == Parent && s.Part == Child)
				.SelectBooleanAsync(
					statements => statements.Any(),
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.IsPartOfTrue,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.IsPartOfFalse,
					new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamParent, Parent },
						{ AabSemantics.Localization.Strings.ParamChild, Child },
					}).ConfigureAwait(false);
		}
	}
}
