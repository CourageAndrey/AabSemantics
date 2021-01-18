using System.Windows;

using Inventor.Client.Dialogs;

namespace Inventor.Client.ViewModels
{
	public class SignValueStatement : IViewModel
	{
		#region Properties

		public Core.IConcept Concept
		{ get; set; }

		public Core.IConcept Sign
		{ get; set; }

		public Core.IConcept Value
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueStatement()
			: this(null, null, null)
		{ }

		public SignValueStatement(Core.Statements.SignValueStatement statement)
			: this(statement.Concept, statement.Sign, statement.Value)
		{
			_boundObject = statement;
		}

		public SignValueStatement(Core.IConcept concept, Core.IConcept sign, Core.IConcept value)
		{
			Concept = concept;
			Sign = sign;
			Value = value;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.SignValueStatement _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, Core.ILanguage language)
		{
			var dialog = new SignValueStatementDialog
			{
				Owner = owner,
				EditValue = this,
			};
			dialog.Initialize(knowledgeBase, language);
			return dialog;
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.SignValueStatement(Concept, Sign, Value));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Concept, Sign, Value);
		}

		#endregion
	}
}
