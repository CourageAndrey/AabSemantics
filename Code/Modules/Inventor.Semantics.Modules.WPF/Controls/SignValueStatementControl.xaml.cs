using System.Linq;

using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Set.Attributes;
using Inventor.Semantics.Modules.WPF.ViewModels;

namespace Inventor.Semantics.Modules.WPF.Controls
{
	public partial class SignValueStatementControl : IStatementEditor
	{
		public SignValueStatementControl()
		{
			InitializeComponent();

			_comboBoxConcept.MakeAutoComplete();
			_comboBoxSign.MakeAutoComplete();
			_comboBoxValue.MakeAutoComplete();
		}

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxConcept.ItemsSource = wrappedConcepts;
			_comboBoxSign.ItemsSource = wrappedConcepts.Where(c => c.Concept.HasAttribute<IsSignAttribute>()).ToList();
			_comboBoxValue.ItemsSource = wrappedConcepts.Where(c => c.Concept.HasAttribute<IsValueAttribute>()).ToList();

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupConcept.Header = languageEditing.PropertyConcept;
			_groupSign.Header = languageEditing.PropertySign;
			_groupValue.Header = languageEditing.PropertyValue;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.SignValueStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Semantics.Contexts.SystemContext;
			}
		}
	}
}
