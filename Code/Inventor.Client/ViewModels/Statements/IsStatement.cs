﻿using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class IsStatement : StatementViewModel<Core.Statements.IsStatement>
	{
		#region Properties

		public ConceptItem Ancestor
		{ get; set; }

		public ConceptItem Descendant
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		public IsStatement(Core.Statements.IsStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Ancestor, language), new ConceptItem(statement.Descendant, language), language)
		{
			BoundObject = statement;
		}

		public IsStatement(string id, ConceptItem ancestor, ConceptItem descendant, ILanguage language)
			: base(id, language)
		{
			Ancestor = ancestor;
			Descendant = descendant;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new IsStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.Statements.StatementNames.Clasification,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.IsStatement CreateStatementImplementation()
		{
			return new Core.Statements.IsStatement(ID, Ancestor.Concept, Descendant.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Ancestor.Concept, Descendant.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new IsStatement(ID, Ancestor, Descendant, _language);
		}
	}
}
