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

		ILanguageStatements StatementNames
		{ get; }

		ILanguageStatements StatementHints
		{ get; }

		ILanguageStatements TrueStatementFormatStrings
		{ get; }

		ILanguageStatements FalseStatementFormatStrings
		{ get; }

		ILanguageStatements QuestionStatementFormatStrings
		{ get; }

		ILanguageQuestionNames QuestionNames
		{ get; }

		ILanguageAnswers Answers
		{ get; }

		ILanguageAttributes Attributes
		{ get; }

		ILanguageSystemConcepts SystemConceptNames
		{ get; }

		ILanguageSystemConcepts SystemConceptHints
		{ get; }

		ILanguageConsistency Consistency
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

		String Comparison
		{ get; }

		String Processes
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

		String ComparisonQuestion
		{ get; }

		String ProcessesQuestion
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

		String ParamLeftValue
		{ get; }

		String ParamRightValue
		{ get; }

		String ParamProcessA
		{ get; }

		String ParamProcessB
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

		String ProcessesSequence
		{ get; }

		String Explanation
		{ get; }
	}

	public interface ILanguageAttributes
	{
		String None
		{ get; }

		String IsSign
		{ get; }

		String IsValue
		{ get; }

		String IsProcess
		{ get; }

		String IsBoolean
		{ get; }

		String IsComparisonSign
		{ get; }

		String IsSequenceSign
		{ get; }
	}

	public interface ILanguageConsistency
	{
		String CheckResult
		{ get; }

		String CheckOk
		{ get; }

		String ErrorDuplicate
		{ get; }

		String ErrorCyclic
		{ get; }

		String ErrorMultipleSubjectArea
		{ get; }

		String ErrorMultipleSign
		{ get; }

		String ErrorMultipleSignValue
		{ get; }

		String ErrorSignWithoutValue
		{ get; }
	}

	public interface ILanguageSystemConcepts
	{
		#region Logical values

		String True
		{ get; }

		String False
		{ get; }

		#endregion

		#region Comparison signs

		String IsEqualTo
		{ get; }

		String IsNotEqualTo
		{ get; }

		String IsGreaterThanOrEqualTo
		{ get; }

		String IsGreaterThan
		{ get; }

		String IsLessThanOrEqualTo
		{ get; }

		String IsLessThan
		{ get; }

		#endregion

		#region Sequence signs

		String StartsAfterOtherStarted
		{ get; }

		String StartsWhenOtherStarted
		{ get; }

		String StartsBeforeOtherStarted
		{ get; }

		String FinishesAfterOtherStarted
		{ get; }

		String FinishesWhenOtherStarted
		{ get; }

		String FinishesBeforeOtherStarted
		{ get; }

		String StartsAfterOtherFinished
		{ get; }

		String StartsWhenOtherFinished
		{ get; }

		String StartsBeforeOtherFinished
		{ get; }

		String FinishesAfterOtherFinished
		{ get; }

		String FinishesWhenOtherFinished
		{ get; }

		String FinishesBeforeOtherFinished
		{ get; }

		String Causes
		{ get; }

		String IsCausedBy
		{ get; }

		String SimultaneousWith
		{ get; }

		#endregion
	}
}
