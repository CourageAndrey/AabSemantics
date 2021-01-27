using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class ConsistsOfStatement : StatementViewModel<Core.Statements.ConsistsOfStatement>
	{
		#region Properties

		public ConceptItem Whole
		{ get; set; }

		public ConceptItem Part
		{ get; set; }

		#endregion

		#region Constructors

		public ConsistsOfStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public ConsistsOfStatement(Core.Statements.ConsistsOfStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Whole, language), new ConceptItem(statement.Part, language), language)
		{
			_boundObject = statement;
		}

		public ConsistsOfStatement(ConceptItem whole, ConceptItem part, ILanguage language)
			: base(language)
		{
			Whole = whole;
			Part = part;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new ConsistsOfStatementControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.Composition,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.ConsistsOfStatement CreateStatementImplementation()
		{
			return new Core.Statements.ConsistsOfStatement(Whole.Concept, Part.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(Whole.Concept, Part.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new ConsistsOfStatement(Whole, Part, _language);
		}
	}
}
