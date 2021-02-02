using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class CausesStatement : StatementViewModel<Core.Statements.CausesStatement>, IProcessesStatement
	{
		#region Properties

		public ConceptItem ProcessA
		{ get; set; }

		public ConceptItem ProcessB
		{ get; set; }

		#endregion

		#region Constructors

		public CausesStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public CausesStatement(Core.Statements.CausesStatement statement, ILanguage language)
			: this(new ConceptItem(statement.ProcessA, language), new ConceptItem(statement.ProcessB, language), language)
		{
			_boundObject = statement;
		}

		public CausesStatement(ConceptItem processA, ConceptItem processB, ILanguage language)
			: base(language)
		{
			ProcessA = processA;
			ProcessB = processB;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new ProcessesStatementControl
			{
				EditValue = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.Causes,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.CausesStatement CreateStatementImplementation()
		{
			return new Core.Statements.CausesStatement(ProcessA.Concept, ProcessB.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(ProcessA.Concept, ProcessB.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new CausesStatement(ProcessA, ProcessB, _language);
		}
	}
}
