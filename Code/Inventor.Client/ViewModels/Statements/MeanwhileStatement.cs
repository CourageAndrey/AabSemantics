using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class MeanwhileStatement : StatementViewModel<Core.Statements.MeanwhileStatement>, IProcessesStatement
	{
		#region Properties

		public ConceptItem ProcessA
		{ get; set; }

		public ConceptItem ProcessB
		{ get; set; }

		#endregion

		#region Constructors

		public MeanwhileStatement(ILanguage language)
			: this(null as ConceptItem, null, language)
		{ }

		public MeanwhileStatement(Core.Statements.MeanwhileStatement statement, ILanguage language)
			: this(new ConceptItem(statement.ProcessA, language), new ConceptItem(statement.ProcessB, language), language)
		{
			_boundObject = statement;
		}

		public MeanwhileStatement(ConceptItem processA, ConceptItem processB, ILanguage language)
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
				Title = language.StatementNames.Meanwhile,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.MeanwhileStatement CreateStatementImplementation()
		{
			return new Core.Statements.MeanwhileStatement(ProcessA.Concept, ProcessB.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(ProcessA.Concept, ProcessB.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new MeanwhileStatement(ProcessA, ProcessB, _language);
		}
	}
}
