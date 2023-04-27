﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Classification.Questions
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsQuestion, IsStatement>()
				.WithTransitives(
					statements => statements.Count == 0,
					question => question.Child,
					newSubject => new IsQuestion(newSubject, Parent))
				.Where(s => s.Parent == Parent && s.Child == Child)
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => language.GetExtension<ILanguageClassificationModule>().Questions.Answers.IsTrue,
					language => language.GetExtension<ILanguageClassificationModule>().Questions.Answers.IsFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamParent, Parent },
						{ Strings.ParamChild, Child },
					});
		}
	}
}
