using System;

namespace Inventor.Client
{
	public interface ILanguage : Core.ILanguage
	{
		ILanguageErrors Errors
		{ get; }

		ILanguageUi Ui
		{ get; }

		ILanguageErrorsInventor ErrorsInventor
		{ get; }

		ILanguageConfiguration Configuration
		{ get; }

		ILanguageMisc Misc
		{ get; }
	}

	public interface ILanguageErrors
	{
		String Warning
		{ get; }

		String InnerException
		{ get; }

		String DialogHeader
		{ get; }

		String DialogMessageCommon
		{ get; }

		String DialogMessageFatal
		{ get; }

		String DialogMessageInner
		{ get; }

		String DialogMessageView
		{ get; }

		String Class
		{ get; }

		String Message
		{ get; }

		String Stack
		{ get; }

		String SaveFilter
		{ get; }

		String LocalizationError
		{ get; }

		String TypeIsntEnumerable
		{ get; }
	}

	public interface ILanguageUi
	{
		String Ok
		{ get; }

		String Cancel
		{ get; }

		String Next
		{ get; }

		String Back
		{ get; }

		String StatementTypeDialogHeader
		{ get; }

		String CreateNewStatement
		{ get; }

		String CreateNewQuestion
		{ get; }

		ILanguageMainForm MainForm
		{ get; }

		ILanguageQuestionDialog QuestionDialog
		{ get; }

		ILanguageEditing Editing
		{ get; }
	}

	public interface ILanguageMainForm
	{
		String Title
		{ get; }

		String CreateNew
		{ get; }

		String Load
		{ get; }

		String Save
		{ get; }

		String SaveAs
		{ get; }

		String CreateTest
		{ get; }

		String DescribeKnowledge
		{ get; }

		String CheckKnowledge
		{ get; }

		String AskQuestion
		{ get; }

		String SelectLanguage
		{ get; }

		String Configuration
		{ get; }

		String ContextMenuRename
		{ get; }

		String ContextMenuKnowledgeAdd
		{ get; }

		String ContextMenuKnowledgeEdit
		{ get; }

		String ContextMenuKnowledgeDelete
		{ get; }
	}

	public interface ILanguageQuestionDialog
	{
		String Title
		{ get; }

		String SelectQuestion
		{ get; }
	}

	public interface ILanguageEditing
	{
		String PropertyConcept
		{ get; }

		String PropertySign
		{ get; }

		String PropertyValue
		{ get; }

		String PropertyWhole
		{ get; }

		String PropertyPart
		{ get; }

		String PropertyAncestor
		{ get; }

		String PropertyDescendant
		{ get; }

		String PropertyArea
		{ get; }

		String PropertyName
		{ get; }

		String PropertyHint
		{ get; }

		String PropertyLeftValue
		{ get; }

		String PropertyRightValue
		{ get; }

		String PropertyComparisonSign
		{ get; }

		String PropertyProcessA
		{ get; }

		String PropertyProcessB
		{ get; }

		String PropertySequenceSign
		{ get; }

		String PropertyAttributes
		{ get; }

		String ColumnHeaderLanguage
		{ get; }

		String ColumnHeaderValue
		{ get; }
	}

	public interface ILanguageErrorsInventor
	{
		String UnknownQuestion
		{ get; }
	}

	public interface ILanguageConfiguration
	{
		String AutoValidate
		{ get; }
	}

	public interface ILanguageMisc
	{
		String NameSemanticNetwork
		{ get; }

		String NameCategoryConcepts
		{ get; }

		String NameCategoryStatements
		{ get; }

		String StrictEnumeration
		{ get; }

		String ClasificationSign
		{ get; }

		String Rules
		{ get; }

		String Answer
		{ get; }

		String Required
		{ get; }

		String DialogKbOpenTitle
		{ get; }

		String DialogKbSaveTitle
		{ get; }

		String DialogKbFileFilter
		{ get; }

		String Concept
		{ get; }
	}
}
