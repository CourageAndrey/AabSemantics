using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;
using Inventor.Core.Attributes;

namespace Inventor.Client.Controls
{
	public partial class ComparisonStatementControl : IStatementEditor
	{
		public ComparisonStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Where(c => c.HasAttribute<IsValueAttribute>()).Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxLeftValue.ItemsSource = wrappedConcepts;
			_comboBoxRightValue.ItemsSource = wrappedConcepts;
			_comboBoxComparisonSign.ItemsSource = knowledgeBase.Concepts.Where(c => c.HasAttribute<IsComparisonSignAttribute>()).Select(c => new ConceptItem(c, language)).ToList();

			_groupLeftValue.Header = language.Ui.Editing.PropertyLeftValue;
			_groupRightValue.Header = language.Ui.Editing.PropertyRightValue;
			_groupComparisonSign.Header = language.Ui.Editing.PropertyComparisonSign;
		}

		public ViewModels.Statements.ComparisonStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ComparisonStatement; }
			set { _contextControl.DataContext = value; }
		}

		public StatementViewModel Statement
		{ get { return EditValue as StatementViewModel; } }
	}
}
