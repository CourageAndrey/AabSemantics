using System.Windows;

using Inventor.Client.Controls;
using Inventor.Client.Dialogs;
using Inventor.Core;

namespace Inventor.Client.ViewModels.Statements
{
	public class ProcessesStatement : StatementViewModel<Core.Statements.ProcessesStatement>
	{
		#region Properties

		public ConceptItem ProcessA
		{ get; set; }

		public ConceptItem ProcessB
		{ get; set; }

		public ConceptItem SequenceSign
		{ get; set; }

		#endregion

		#region Constructors

		public ProcessesStatement(ILanguage language)
			: this(null as ConceptItem, null, null, language)
		{ }

		public ProcessesStatement(Core.Statements.ProcessesStatement statement, ILanguage language)
			: this(new ConceptItem(statement.ProcessA, language), new ConceptItem(statement.ProcessB, language), new ConceptItem(statement.SequenceSign, language), language)
		{
			_boundObject = statement;
		}

		public ProcessesStatement(ConceptItem processA, ConceptItem processB, ConceptItem sequenceSign, ILanguage language)
			: base(language)
		{
			ProcessA = processA;
			ProcessB = processB;
			SequenceSign = sequenceSign;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var control = new ProcessesStatementControl
			{
				Statement = this,
			};
			control.Initialize(knowledgeBase, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.StatementNames.Processes,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		protected override Core.Statements.ProcessesStatement CreateStatementImplementation()
		{
			return new Core.Statements.ProcessesStatement(ProcessA.Concept, ProcessB.Concept, SequenceSign.Concept);
		}

		public override void ApplyUpdate()
		{
			_boundObject.Update(ProcessA.Concept, ProcessB.Concept, SequenceSign.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new ProcessesStatement(ProcessA, ProcessB, SequenceSign, _language);
		}
	}
}
