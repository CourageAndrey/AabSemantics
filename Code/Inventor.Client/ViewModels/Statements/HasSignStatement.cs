using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class HasSignStatement : StatementViewModel<Core.Statements.HasSignStatement>
	{
		#region Properties

		public ConceptItem Concept
		{ get; set; }

		public ConceptItem Sign
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public HasSignStatement(Core.Statements.HasSignStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Concept, language), new ConceptItem(statement.Sign, language), language)
		{
			_boundObject = statement;
		}

		public HasSignStatement(ConceptItem concept, ConceptItem sign, ILanguage language)
			: base(language)
		{
			Concept = concept;
			Sign = sign;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new HasSignStatementControl
			{
				Statement = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.HasSign,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.HasSignStatement CreateStatementImplementation()
		{
			return new Core.Statements.HasSignStatement(Concept.Concept, Sign.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(Concept.Concept, Sign.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new HasSignStatement(Concept, Sign, _language);
		}
	}
}
