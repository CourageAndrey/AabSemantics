using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Set.Questions
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsPartOfQuestion : Question
	{
		#region Properties

		public IConcept Parent
		{ get; }

		public IConcept Child
		{ get; }

		#endregion

		public IsPartOfQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Child = child.EnsureNotNull(nameof(child));
			Parent = parent.EnsureNotNull(nameof(parent));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsPartOfQuestion, HasPartStatement>()
				.Where(s => s.Whole == Parent && s.Part == Child)
				.SelectBoolean(
					statements => statements.Any(),
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsPartOfTrue,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsPartOfFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Semantics.Localization.Strings.ParamParent, Parent },
						{ Semantics.Localization.Strings.ParamChild, Child },
					});
		}
	}
}
