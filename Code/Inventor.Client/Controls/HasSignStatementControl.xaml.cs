using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;
using Inventor.Core.Attributes;

namespace Inventor.Client.Controls
{
	public partial class HasSignStatementControl : IStatementEditor
	{
		public HasSignStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxConcept.ItemsSource = wrappedConcepts;
			_comboBoxSign.ItemsSource = wrappedConcepts.Where(c => c.Concept.HasAttribute<IsSignAttribute>()).ToList();

			_groupConcept.Header = language.Ui.Editing.PropertyConcept;
			_groupSign.Header = language.Ui.Editing.PropertySign;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.HasSignStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
