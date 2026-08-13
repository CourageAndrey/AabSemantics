namespace AabSemantics.Extensions.WPF
{
	/// <summary>The application's main window, as seen by the UI extension.</summary>
	public interface IMainWindow
	{
		/// <summary>Binds the window to the hosting application.</summary>
		/// <param name="application">The hosting application.</param>
		void Initialize(IInventorApplication application);
	}
}
