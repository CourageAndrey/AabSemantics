﻿using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IContext
	{
		ILanguage Language
		{ get; }

		ICollection<IStatement> Scope
		{ get; }

		IContext Parent
		{ get; }

		ICollection<IContext> Children
		{ get; }

		Boolean IsSystem
		{ get; }
	}

	public interface ISystemContext : IContext
	{
		IKnowledgeBaseContext Instantiate(IKnowledgeBase knowledgeBase, IQuestionRepository questionRepository);
	}

	public interface IKnowledgeBaseContext : IContext
	{
		IKnowledgeBase KnowledgeBase
		{ get; }

		IQuestionRepository QuestionRepository
		{ get; }

		IQuestionProcessingContext AskQuestion(IQuestion question);
	}

	public interface IQuestionProcessingContext : IKnowledgeBaseContext, IDisposable
	{
		IQuestion Question
		{ get; }
	}

	public interface IQuestionProcessingContext<out QuestionT> : IQuestionProcessingContext
		where QuestionT : IQuestion
	{
		QuestionT QuestionX
		{ get; }
	}

	public static class ContextHelper
	{
		public static ICollection<IContext> GetHierarchy(this IContext context)
		{
			var hierarchy = new HashSet<IContext>();
			while (context != null)
			{
				hierarchy.Add(context);
				context = context.Parent;
			}
			return hierarchy;
		}
	}
}