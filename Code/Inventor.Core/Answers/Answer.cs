using System;

using Inventor.Core.Base;

namespace Inventor.Core.Answers
{
	public class Answer : IAnswer
	{
		#region Properties

		public IText Description
		{ get; }

		public IExplanation Explanation
		{ get; }

		public Boolean IsEmpty
		{ get; }

		#endregion

		public Answer(IText description, IExplanation explanation, Boolean isEmpty)
		{
			Description = description;
			Explanation = explanation;
			IsEmpty = isEmpty;
		}

		public static IAnswer CreateUnknown()
		{
			return new Answer(
				new Text.FormattedText(language => language.Answers.Unknown),
				new Explanation(Array.Empty<IStatement>()),
				true);
		}
	}
}
