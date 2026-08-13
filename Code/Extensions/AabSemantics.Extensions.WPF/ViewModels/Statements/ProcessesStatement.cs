using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Processes.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	/// <summary>Editable view over the process sequence statement.</summary>
	public class ProcessesStatement : StatementViewModel<Modules.Processes.Statements.ProcessesStatement>
	{
		#region Properties

		/// <summary>The first process.</summary>
		public ConceptItem ProcessA
		{ get; set; }

		/// <summary>The second process.</summary>
		public ConceptItem ProcessB
		{ get; set; }

		/// <summary>The sequence sign.</summary>
		public ConceptItem SequenceSign
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new statement.</summary>
		/// <param name="language">Language used to render the statement's text.</param>
		public ProcessesStatement(ILanguage language)
			: this(string.Empty, null, null, null, language)
		{ }

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public ProcessesStatement(Modules.Processes.Statements.ProcessesStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.ProcessA, language), new ConceptItem(statement.ProcessB, language), new ConceptItem(statement.SequenceSign, language), language)
		{
			BoundObject = statement;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="processA">The first process.</param>
		/// <param name="processB">The second process.</param>
		/// <param name="sequenceSign">The sequence sign.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public ProcessesStatement(string id, ConceptItem processA, ConceptItem processB, ConceptItem sequenceSign, ILanguage language)
			: base(id, language)
		{
			ProcessA = processA;
			ProcessB = processB;
			SequenceSign = sequenceSign;
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>Builds the dialog used to edit this statement.</summary>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="semanticNetwork">Network supplying the pick lists.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>An unshown dialog.</returns>
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
				Title = language.GetExtension<ILanguageProcessesModule>().Statements.Names.Processes,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		/// <summary>Builds the statement from the edited values.</summary>
		/// <returns>The created statement.</returns>
		protected override AabSemantics.Modules.Processes.Statements.ProcessesStatement CreateStatementImplementation()
		{
			return new AabSemantics.Modules.Processes.Statements.ProcessesStatement(ID, ProcessA.Concept, ProcessB.Concept, SequenceSign.Concept);
		}

		/// <summary>Writes the edited values onto the bound statement.</summary>
		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, ProcessA.Concept, ProcessB.Concept, SequenceSign.Concept);
		}

		#endregion

		/// <summary>Copies the view model.</summary>
		/// <returns>An independent copy.</returns>
		public override StatementViewModel Clone()
		{
			return new ProcessesStatement(ID, ProcessA, ProcessB, SequenceSign, _language);
		}
	}
}
