﻿using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;

namespace Inventor.Client.ViewModels
{
	public class GroupStatement : IViewModel
	{
		#region Properties

		public Core.IConcept Area
		{ get; set; }

		public Core.IConcept Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
			: this(null, null)
		{ }

		public GroupStatement(Core.Statements.GroupStatement statement)
			: this(statement.Area, statement.Concept)
		{
			_boundObject = statement;
		}

		public GroupStatement(Core.IConcept area, Core.IConcept concept)
		{
			Area = area;
			Concept = concept;
		}

		#endregion

		#region Implementation of IViewModel

		private Core.Statements.GroupStatement _boundObject;

		public Window CreateEditDialog(Window owner, Core.IKnowledgeBase knowledgeBase, Core.ILanguage language)
		{
			var control = new GroupStatementControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.SubjectArea,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		public void ApplyCreate(Core.IKnowledgeBase knowledgeBase)
		{
			knowledgeBase.Statements.Add(_boundObject = new Core.Statements.GroupStatement(Area, Concept));
		}

		public void ApplyUpdate()
		{
			_boundObject.Update(Area, Concept);
		}

		#endregion
	}
}
