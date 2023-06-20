using System;

namespace AabSemantics.Contexts
{
	public class SystemContext : Context, ISystemContext
	{
		#region Properties

		public override Boolean IsSystem
		{ get { return true; } }

		#endregion

		public SystemContext(ILanguage language)
			: base(language, null)
		{ }

		public ISemanticNetworkContext Instantiate(ISemanticNetwork semanticNetwork)
		{
			if (Children.Count > 0) throw new InvalidOperationException("Impossible to instantiate system context more than once.");

			return new SemanticNetworkContext(Language, this, semanticNetwork);
		}
	}
}