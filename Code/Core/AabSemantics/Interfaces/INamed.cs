namespace AabSemantics
{
	/// <summary>
	/// Anything that carries a user-facing name. The name is localized, so the same item
	/// reads correctly in every language the semantic network supports.
	/// </summary>
	public interface INamed
	{
		/// <summary>
		/// Localized display name of the item.
		/// </summary>
		ILocalizedString Name
		{ get; }
	}
}
