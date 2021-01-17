using System.Windows;

using Inventor.Core;

namespace Inventor.Client
{
	public interface IViewModel
	{
		Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language);

		void ApplyCreate(IKnowledgeBase knowledgeBase);

		void ApplyUpdate();
	}
}
