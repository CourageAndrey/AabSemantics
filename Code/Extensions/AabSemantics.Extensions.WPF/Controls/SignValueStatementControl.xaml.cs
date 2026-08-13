using System.Linq;

using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Extensions.WPF.ViewModels;

namespace AabSemantics.Extensions.WPF.Controls
{
	/// <summary>Editor control for sign value statements.</summary>
	public partial class SignValueStatementControl : IStatementEditor
	{
		/// <summary>Creates the control.</summary>
		public SignValueStatementControl()
		{
			InitializeComponent();

			_comboBoxConcept.MakeAutoComplete();
			_comboBoxSign.MakeAutoComplete();
			_comboBoxValue.MakeAutoComplete();
		}

		/// <summary>Fills the control's pick lists and localizes its captions.</summary>
		/// <param name="semanticNetwork">Network supplying the selectable concepts.</param>
		/// <param name="language">Language the control is localized in.</param>
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

		/// <summary>The statement being edited.</summary>
		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.SignValueStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext;
			}
		}
	}
}
