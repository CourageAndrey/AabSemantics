using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Localization.Modules;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
						{ Strings.ParamParent, Parent },
						{ Strings.ParamChild, Child },
					});
		}
	}
}
