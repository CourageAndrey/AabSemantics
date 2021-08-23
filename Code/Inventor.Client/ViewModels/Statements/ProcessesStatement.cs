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
			: this(string.Empty, null, null, null, language)
		{ }

		public ProcessesStatement(Core.Statements.ProcessesStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.ProcessA, language), new ConceptItem(statement.ProcessB, language), new ConceptItem(statement.SequenceSign, language), language)
		{
			BoundObject = statement;
		}

		public ProcessesStatement(string id, ConceptItem processA, ConceptItem processB, ConceptItem sequenceSign, ILanguage language)
			: base(id, language)
		{
			ProcessA = processA;
			ProcessB = processB;
			SequenceSign = sequenceSign;
		}

		#endregion

		#region Implementation of IViewModel

		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new ProcessesStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
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
			return new Core.Statements.ProcessesStatement(ID, ProcessA.Concept, ProcessB.Concept, SequenceSign.Concept);
		}

		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, ProcessA.Concept, ProcessB.Concept, SequenceSign.Concept);
		}

		#endregion

		public override StatementViewModel Clone()
		{
			return new ProcessesStatement(ID, ProcessA, ProcessB, SequenceSign, _language);
		}
	}
}
