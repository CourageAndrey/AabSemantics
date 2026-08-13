using System.Linq;

using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Extensions.WPF.ViewModels;

namespace AabSemantics.Extensions.WPF.Controls
{
	/// <summary>Editor control for processes statements.</summary>
	public partial class ProcessesStatementControl : IStatementEditor
	{
		/// <summary>Creates the control.</summary>
		public ProcessesStatementControl()
		{
			InitializeComponent();

			_comboBoxProcessA.MakeAutoComplete();
			_comboBoxProcessB.MakeAutoComplete();
			_comboBoxSequenceSign.MakeAutoComplete();
		}

		/// <summary>Fills the control's pick lists and localizes its captions.</summary>
		/// <param name="semanticNetwork">Network supplying the selectable concepts.</param>
		/// <param name="language">Language the control is localized in.</param>
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

		/// <summary>The statement being edited.</summary>
		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ProcessesStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext;
			}
		}
	}
}
