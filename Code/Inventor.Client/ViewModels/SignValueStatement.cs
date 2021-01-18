using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	public class SignValueStatement : IViewModel
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

		public SignValueStatement()
			: this(null as ConceptItem, null, null)
		{ }

		public SignValueStatement(Core.Statements.SignValueStatement statement, ILanguage language)
			: this(new ConceptItem(statement.Concept, language), new ConceptItem(statement.Sign, language), new ConceptItem(statement.Value, language))
		{
			_boundObject = statement;
		}

		public SignValueStatement(ConceptItem concept, ConceptItem sign, ConceptItem value)
		{
			Concept = concept;
			Sign = sign;
			Value = value;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.SignValueStatement _boundObject;

		public Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new SignValueStatementControl
			{
				EditValue = this,
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

		public void ApplyCreate(IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.SignValueStatement(Concept.Concept, Sign.Concept, Value.Concept));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Concept.Concept, Sign.Concept, Value.Concept);
		}

		#endregion
	}
}
