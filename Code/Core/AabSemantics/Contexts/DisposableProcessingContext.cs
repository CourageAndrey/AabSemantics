using System;
using System.Linq;

namespace AabSemantics.Contexts
{
	/// <summary>
	/// A context whose statements live only as long as the context does. Disposing it withdraws
	/// them from the semantic network and detaches the context from its parent.
	/// </summary>
	public class DisposableProcessingContext : SemanticNetworkContext, IDisposable
	{
		/// <summary>Creates a child context whose statements are withdrawn when it is disposed.</summary>
		/// <param name="parent">Enclosing context.</param>
		internal DisposableProcessingContext(ISemanticNetworkContext parent)
			: base(parent.Language, parent, parent.SemanticNetwork)
		{ }

		private Boolean _disposed;

		/// <summary>
		/// Removes this context's statements from the network and unregisters it from its parent.
		/// Disposing twice is harmless.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// A child context is still alive. Children must be disposed first, otherwise their
		/// statements would outlive the scope they were added to.
		/// </exception>
		public void Dispose()
		{
			if (!_disposed)
			{
				foreach (var child in Children.OfType<DisposableProcessingContext>())
				{
					if (!child._disposed) throw new InvalidOperationException("Impossible to dispose question context because it has running child contexts.");
				}

				foreach (var knowledge in Scope)
				{
					SemanticNetwork.Statements.Remove(knowledge);
				}

				Parent.Children.Remove(this);

				_disposed = true;

				GC.SuppressFinalize(this);
			}
		}
	}
}