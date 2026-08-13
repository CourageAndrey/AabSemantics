using System.Linq;

using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Extensions.WPF.ViewModels;

namespace AabSemantics.Extensions.WPF.Controls
{
	/// <summary>Editor control for comparison statements.</summary>
	public partial class ComparisonStatementControl : IStatementEditor
	{
		/// <summary>Creates the control.</summary>
		public ComparisonStatementControl()
		{
			InitializeComponent();

			_comboBoxLeftValue.MakeAutoComplete();
			_comboBoxRightValue.MakeAutoComplete();
			_comboBoxComparisonSign.MakeAutoComplete();
		}

		/// <summary>Fills the control's pick lists and localizes its captions.</summary>
		/// <param name="semanticNetwork">Network supplying the selectable concepts.</param>
		/// <param name="language">Language the control is localized in.</param>
		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Where(c => c.HasAttribute<IsValueAttribute>()).Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxLeftValue.ItemsSource = wrappedConcepts;
			_comboBoxRightValue.ItemsSource = wrappedConcepts;
			_comboBoxComparisonSign.ItemsSource = semanticNetwork.Concepts.Where(c => c.HasAttribute<IsComparisonSignAttribute>()).Select(c => new ConceptItem(c, language)).ToList();

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupLeftValue.Header = languageEditing.PropertyLeftValue;
			_groupRightValue.Header = languageEditing.PropertyRightValue;
			_groupComparisonSign.Header = languageEditing.PropertyComparisonSign;
		}

		/// <summary>The statement being edited.</summary>
		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ComparisonStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext;
			}
		}
	}
}
