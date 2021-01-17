﻿using System.Windows;

using Inventor.Client.UI;

namespace Inventor.Client.ViewModels
{
	public class HasSignStatement : IViewModel
	{
		#region Properties

		public Core.IConcept Concept
		{ get; set; }

		public Core.IConcept Sign
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignStatement()
			: this(null, null)
		{ }

		public HasSignStatement(Core.Statements.HasSignStatement statement)
			: this(statement.Concept, statement.Sign)
		{
			_boundObject = statement;
		}

		public HasSignStatement(Core.IConcept concept, Core.IConcept sign)
		{
			Concept = concept;
			Sign = sign;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.HasSignStatement _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, Core.ILanguage language)
		{
			var dialog = new HasSignStatementDialog
			{
				Owner = owner,
				EditValue = this,
			};
			dialog.Initialize(knowledgeBase, language);
			return dialog;
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.HasSignStatement(Concept, Sign));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Concept, Sign);
		}

		#endregion
	}
}
