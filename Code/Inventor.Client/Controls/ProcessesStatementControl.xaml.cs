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

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Where(c => c.HasAttribute<IsProcessAttribute>()).Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxProcessA.ItemsSource = wrappedConcepts;
			_comboBoxProcessB.ItemsSource = wrappedConcepts;
			_comboBoxSequenceSign.ItemsSource = semanticNetwork.Concepts.Where(c => c.HasAttribute<IsSequenceSignAttribute>()).Select(c => new ConceptItem(c, language)).ToList();

			_groupProcessA.Header = language.Ui.Editing.PropertyProcessA;
			_groupProcessB.Header = language.Ui.Editing.PropertyProcessB;
			_groupSequenceSign.Header = language.Ui.Editing.PropertySequenceSign;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ProcessesStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
