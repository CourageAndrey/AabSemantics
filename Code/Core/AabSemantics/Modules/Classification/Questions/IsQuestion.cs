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
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsQuestion : Question
	{
		#region Properties

		public IConcept Child
		{ get; }

		public IConcept Parent
		{ get; }

		#endregion

		public IsQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Child = child.EnsureNotNull(nameof(child));
			Parent = parent.EnsureNotNull(nameof(parent));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<IsQuestion, IsStatement>()
				.WithTransitives(
					async statements => await Task.FromResult(statements.Count == 0),
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
					});
		}
	}
}
