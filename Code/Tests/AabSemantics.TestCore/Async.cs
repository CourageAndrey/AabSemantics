using System;
using System.Linq;

using NUnit.Framework;

namespace AabSemantics.TestCore
{
	public static class Async
	{
		public static ExceptionT Throws<ExceptionT>(Action method)
			where ExceptionT : Exception
		{
			var aggregateException = Assert.Throws<AggregateException>(() => method());
			var trueException = (ExceptionT) aggregateException.InnerExceptions.Single();
			Assert.That(trueException, Is.TypeOf<ExceptionT>());
			return trueException;
		}
	}
}
