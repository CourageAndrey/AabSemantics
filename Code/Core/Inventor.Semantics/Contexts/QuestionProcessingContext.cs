namespace Inventor.Semantics.Contexts
{
	public class QuestionProcessingContext<QuestionT> : DisposableProcessingContext, IQuestionProcessingContext<QuestionT>
		where QuestionT : IQuestion
	{
		#region Properties

		IQuestion IQuestionProcessingContext.Question
		{ get { return _question; } }

		public QuestionT Question
		{ get { return _question; } }

		private readonly QuestionT _question;

		#endregion

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