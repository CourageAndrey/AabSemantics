using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;
using Inventor.Core.Attributes;

namespace Inventor.Client.Controls
{
	public partial class ProcessesStatementControl : IStatementEditor
	{
		public ProcessesStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Where(c => c.HasAttribute<IsProcessAttribute>()).Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxProcessA.ItemsSource = wrappedConcepts;
			_comboBoxProcessB.ItemsSource = wrappedConcepts;

			_groupProcessA.Header = language.Ui.Editing.PropertyProcessA;
			_groupProcessB.Header = language.Ui.Editing.PropertyProcessB;
		}

		public ViewModels.Statements.IProcessesStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.Statements.IProcessesStatement; }
			set { _contextControl.DataContext = value; }
		}

		public StatementViewModel Statement
		{ get { return EditValue as StatementViewModel; } }
	}
}
