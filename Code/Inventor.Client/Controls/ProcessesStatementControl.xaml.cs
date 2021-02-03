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
			_comboBoxSequenceSign.ItemsSource = knowledgeBase.Concepts.Where(c => c.HasAttribute<IsSequenceSignAttribute>()).Select(c => new ConceptItem(c, language)).ToList();

			_groupProcessA.Header = language.Ui.Editing.PropertyProcessA;
			_groupProcessB.Header = language.Ui.Editing.PropertyProcessB;
			_groupSequenceSign.Header = language.Ui.Editing.PropertySequenceSign;
		}

		public ViewModels.Statements.ProcessesStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ProcessesStatement; }
			set { _contextControl.DataContext = value; }
		}

		public StatementViewModel Statement
		{ get { return EditValue as StatementViewModel; } }
	}
}
