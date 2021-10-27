using System.Linq;

using Inventor.WPF.ViewModels;
using Inventor.Semantics;
using Inventor.Semantics.Attributes;
using Inventor.Processes.Attributes;

namespace Inventor.WPF.Controls
{
	public partial class ProcessesStatementControl : IStatementEditor
	{
		public ProcessesStatementControl()
		{
			InitializeComponent();

			_comboBoxProcessA.MakeAutoComplete();
			_comboBoxProcessB.MakeAutoComplete();
			_comboBoxSequenceSign.MakeAutoComplete();
		}

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Where(c => c.HasAttribute<IsProcessAttribute>()).Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxProcessA.ItemsSource = wrappedConcepts;
			_comboBoxProcessB.ItemsSource = wrappedConcepts;
			_comboBoxSequenceSign.ItemsSource = semanticNetwork.Concepts.Where(c => c.HasAttribute<IsSequenceSignAttribute>()).Select(c => new ConceptItem(c, language)).ToList();

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupProcessA.Header = languageEditing.PropertyProcessA;
			_groupProcessB.Header = languageEditing.PropertyProcessB;
			_groupSequenceSign.Header = languageEditing.PropertySequenceSign;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ProcessesStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Semantics.Contexts.SystemContext;
			}
		}
	}
}
