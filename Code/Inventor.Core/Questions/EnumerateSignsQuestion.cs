using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class EnumerateSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public EnumerateSignsQuestion(IConcept concept, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
			Recursive = recursive;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateSignsQuestion, HasSignStatement>()
				.WithTransitives(
					statements => Recursive,
					question => question.Concept,
					newSubject => new EnumerateSignsQuestion(newSubject, true))
				.Where(s => s.Concept == Concept)
				.AggregateTransitivesToStatements()
				.SelectAllConcepts(
					statement => statement.Sign,
					question => question.Concept,
					Strings.ParamConcept,
					language => language.Answers.ConceptSigns + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ": ")
				.AppendAdditionalTransitives()
				.Answer;
		}
	}
}
