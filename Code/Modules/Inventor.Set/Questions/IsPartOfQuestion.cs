using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics;
using Inventor.Semantics.Questions;
using Inventor.Set.Localization;
using Inventor.Set.Statements;

namespace Inventor.Set.Questions
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
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
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
