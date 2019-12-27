using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inventor.Core.Localization;

namespace Inventor.Core
{
    public abstract class QuestionProcessor
    {
        public static FormattedText Process(KnowledgeBase knowledgeBase, Question question)
        {
            QuestionProcessor processor;
            if (allProcessors.TryGetValue(question.GetType(), out processor))
            {
                return processor.ProcessInternal(knowledgeBase, question);
            }
            else
            {
                throw new KeyNotFoundException(LanguageEx.CurrentEx.ErrorsInventor.UnknownQuestion);
            }
        }

        private static readonly IDictionary<Type, QuestionProcessor> allProcessors = new Dictionary<Type, QuestionProcessor>(); 

        static QuestionProcessor()
        {
            foreach (var processorType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(QuestionProcessor).IsAssignableFrom(t) && !t.IsAbstract))
            {
                var type = processorType;
                while (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(QuestionProcessor<>))
                {
                    type = type.BaseType;
                }
                allProcessors[type.GetGenericArguments()[0]] = Activator.CreateInstance(processorType) as QuestionProcessor;
            }
        }

        protected abstract FormattedText ProcessInternal(KnowledgeBase knowledgeBase, Question question);
    }

    public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
        where QuestionT : Question
    {
        protected abstract FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, QuestionT question);

        protected override FormattedText ProcessInternal(KnowledgeBase knowledgeBase, Question question)
        {
            if (question.GetType() == typeof(QuestionT))
            {
                return ProcessImplementation(knowledgeBase, question as QuestionT);
            }
            else
            {
                throw new ArrayTypeMismatchException();
            }
        }
    }

    public static class AnswerHelper
    {
        public static FormattedText CreateUnknown()
        {
            return new FormattedText(() => LanguageEx.CurrentEx.Answers.Unknown, new Dictionary<string, INamed>());
        }
    }
}