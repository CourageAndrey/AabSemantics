using System;

namespace Inventor.Core
{
	public interface ILanguage
	{
		String Name
		{ get; }

		String Culture
		{ get; }

		ILanguageCommon Common
		{ get; }

		ILanguageErrors Errors
		{ get; }

		ILanguageStatements StatementNames
		{ get; }

		ILanguageStatements StatementHints
		{ get; }

		ILanguageStatementFormatStrings TrueStatementFormatStrings
		{ get; }

		ILanguageStatementFormatStrings FalseStatementFormatStrings
		{ get; }

		ILanguageStatementFormatStrings QuestionStatementFormatStrings
		{ get; }

		ILanguageQuestionNames QuestionNames
		{ get; }

		ILanguageAnswers Answers
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

	public interface ILanguageCommon
	{
		#region Buttons

		String Close
		{ get; }

		String Ok
		{ get; }

		String Cancel
		{ get; }

		String Abort
		{ get; }

		String Ignore
		{ get; }

		String Save
		{ get; }

		String SaveFile
		{ get; }

		#endregion

		#region MainMenu

		String Exit
		{ get; }

		String SelectedLanguage
		{ get; }

		String Help
		{ get; }

		String About
		{ get; }

		String Configuration
		{ get; }

		#endregion

		String WaitPromt
		{ get; }

		String Question
		{ get; }

		String ViewPicture
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

	public interface ILanguageStatements
	{
		String SubjectArea
		{ get; }

		String Clasification
		{ get; }

		String HasSign
		{ get; }

		String SignValue
		{ get; }

		String Composition
		{ get; }
	}

	public interface ILanguageStatementFormatStrings
	{
		String SubjectArea
		{ get; }

		String Clasification
		{ get; }

		String HasSign
		{ get; }

		String SignValue
		{ get; }

		String Composition
		{ get; }
	}

	public interface ILanguageQuestionNames
	{
		String EnumerateChildrenQuestion
		{ get; }

		String IsQuestion
		{ get; }

		String WhatQuestion
		{ get; }

		String FindSubjectAreaQuestion
		{ get; }

		String DescribeSubjectAreaQuestion
		{ get; }

		String SignValueQuestion
		{ get; }

		String EnumerateSignsQuestion
		{ get; }

		String HasSignQuestion
		{ get; }

		String HasSignsQuestion
		{ get; }

		String IsSignQuestion
		{ get; }

		String IsValueQuestion
		{ get; }

		String IsPartOfQuestion
		{ get; }

		String EnumeratePartsQuestion
		{ get; }

		String EnumerateContainersQuestion
		{ get; }

		String IsSubjectAreaQuestion
		{ get; }

		String CheckStatementQuestion
		{ get; }

		String QuestionWithCondition
		{ get; }

		String ParamParent
		{ get; }

		String ParamChild
		{ get; }

		String ParamConcept
		{ get; }

		String ParamSign
		{ get; }

		String ParamRecursive
		{ get; }

		String ParamArea
		{ get; }

		String ParamStatement
		{ get; }

		String ParamConditions
		{ get; }

		String ParamQuestion
		{ get; }
	}

	public interface ILanguageAnswers
	{
		String Unknown
		{ get; }

		String IsTrue
		{ get; }

		String IsFalse
		{ get; }

		String IsSubjectAreaTrue
		{ get; }

		String IsSubjectAreaFalse
		{ get; }

		String SignTrue
		{ get; }

		String SignFalse
		{ get; }

		String ValueTrue
		{ get; }

		String ValueFalse
		{ get; }

		String HasSignTrue
		{ get; }

		String HasSignFalse
		{ get; }

		String HasSignsTrue
		{ get; }

		String HasSignsFalse
		{ get; }

		String IsDescription
		{ get; }

		String IsDescriptionWithSign
		{ get; }

		String IsDescriptionWithSignValue
		{ get; }

		String Enumerate
		{ get; }

		String SubjectArea
		{ get; }

		String SubjectAreaConcepts
		{ get; }

		String ConceptSigns
		{ get; }

		String SignValue
		{ get; }

		String RecursiveTrue
		{ get; }

		String RecursiveFalse
		{ get; }

		String IsPartOfTrue
		{ get; }

		String IsPartOfFalse
		{ get; }

		String EnumerateParts
		{ get; }

		String EnumerateContainers
		{ get; }

		String Explanation
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
		String NameKnowledgeBase
		{ get; }

		String NameCategoryConcepts
		{ get; }

		String NameCategoryStatements
		{ get; }

		String StrictEnumeration
		{ get; }

		String ClasificationSign
		{ get; }

		String NewKbName
		{ get; }

		String Rules
		{ get; }

		String Answer
		{ get; }

		String Required
		{ get; }

		String CheckResult
		{ get; }

		String CheckOk
		{ get; }

		String DialogKbOpenTitle
		{ get; }

		String DialogKbSaveTitle
		{ get; }

		String DialogKbFileFilter
		{ get; }

		String ConsistencyErrorDuplicate
		{ get; }

		String ConsistencyErrorCyclic
		{ get; }

		String ConsistencyErrorMultipleSubjectArea
		{ get; }

		String ConsistencyErrorMultipleSign
		{ get; }

		String ConsistencyErrorMultipleSignValue
		{ get; }

		String ConsistencyErrorSignWithoutValue
		{ get; }

		String True
		{ get; }

		String False
		{ get; }

		String TrueHint
		{ get; }

		String FalseHint
		{ get; }

		String Concept
		{ get; }
	}
}
