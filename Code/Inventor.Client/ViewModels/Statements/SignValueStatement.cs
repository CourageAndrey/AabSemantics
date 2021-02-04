using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class SignValueStatement : StatementViewModel<Core.Statements.SignValueStatement>
	{
		#region Properties

		public ConceptItem Concept
		{ get; set; }

		public ConceptItem Sign
		{ get; set; }

		public ConceptItem Value
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueStatement(ILanguage language)
			: this(null as ConceptItem, null, null, language)
		{ }

		public SignValueStatement(Core.Statements.SignValueStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Concept, language), new ConceptItem(statement.Sign, language), new ConceptItem(statement.Value, language), language)
		{
			_boundObject = statement;
		}

		public SignValueStatement(ConceptItem concept, ConceptItem sign, ConceptItem value, ILanguage language)
			: base(language)
		{
			Concept = concept;
			Sign = sign;
			Value = value;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new SignValueStatementControl
			{
				Statement = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.SignValue,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.SignValueStatement CreateStatementImplementation()
		{
			return new Core.Statements.SignValueStatement(Concept.Concept, Sign.Concept, Value.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(Concept.Concept, Sign.Concept, Value.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new SignValueStatement(Concept, Sign, Value, _language);
		}
	}
}
