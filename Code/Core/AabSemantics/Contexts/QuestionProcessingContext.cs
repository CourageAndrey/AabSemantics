namespace AabSemantics.Contexts
{
	/// <summary>
	/// The context a single question is answered in. Instances are created through
	/// <see cref="SemanticNetworkContext.CreateQuestionContext"/> rather than directly.
	/// </summary>
	/// <typeparam name="QuestionT">Concrete question type being answered.</typeparam>
	public class QuestionProcessingContext<QuestionT> : DisposableProcessingContext, IQuestionProcessingContext<QuestionT>
		where QuestionT : IQuestion
	{
		#region Properties

		IQuestion IQuestionProcessingContext.Question
		{ get { return _question; } }

		/// <summary>
		/// The question being answered.
		/// </summary>
		public QuestionT Question
		{ get { return _question; } }

		private readonly QuestionT _question;

		#endregion

		/// <summary>Creates the context a single question is answered in.</summary>
		/// <param name="parent">Enclosing context.</param>
		/// <param name="question">Question being answered.</param>
		/// <param name="language">Language for the answer's text; keeps the parent's when <c>null</c>.</param>
		internal QuestionProcessingContext(ISemanticNetworkContext parent, QuestionT question, ILanguage language = null)
			: base(parent)
		{
			_question = question;
			if (language != null)
			{
				Language = language;
			}
		}
	}
}