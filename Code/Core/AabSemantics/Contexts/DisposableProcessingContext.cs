using System;
using System.Linq;

namespace AabSemantics.Contexts
{
	public class DisposableProcessingContext : SemanticNetworkContext, IDisposable
	{
		internal DisposableProcessingContext(ISemanticNetworkContext parent)
			: base(parent.Language, parent, parent.SemanticNetwork)
		{ }

		private Boolean _disposed;

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