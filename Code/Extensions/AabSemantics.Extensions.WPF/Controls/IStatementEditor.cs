namespace AabSemantics.Extensions.WPF.Controls
{
	/// <summary>A control that edits one statement, so the edit dialog can host it generically.</summary>
	public interface IStatementEditor
	{
		/// <summary>The statement being edited.</summary>
		StatementViewModel Statement
		{ get; }
	}
}
