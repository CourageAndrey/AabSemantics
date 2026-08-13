using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>A serializable snapshot of an exception, used to show and save error reports.</summary>
	public interface IExceptionWrapper
	{
		/// <summary>Full name of the exception type.</summary>
		String Class
		{ get; }

		/// <summary>The exception message.</summary>
		String Message
		{ get; }

		/// <summary>The captured stack trace.</summary>
		String StackTrace
		{ get; }

		/// <summary>Snapshot of the nested exception, or <c>null</c>.</summary>
		IExceptionWrapper InnerException
		{ get; }
	}

	/// <summary>Serializable snapshot of an exception, including its whole inner-exception chain.</summary>
	[Serializable]
	public class ExceptionWrapper : IExceptionWrapper, IEquatable<ExceptionWrapper>
	{
		#region Properties

		/// <summary>Full name of the exception type.</summary>
		[XmlElement]
		public String Class
		{ get; set; }

		/// <summary>The exception message.</summary>
		[XmlElement]
		public String Message
		{ get; set; }

		/// <summary>The captured stack trace.</summary>
		[XmlElement]
		public String StackTrace
		{ get; set; }

		/// <summary>Snapshot of the nested exception, or <c>null</c>.</summary>
		[XmlIgnore]
		public IExceptionWrapper InnerException
		{ get { return InnerExceptionXml; } }

		/// <summary>Snapshot of the nested exception, in serializable form.</summary>
		[XmlElement("InnerException")]
		public ExceptionWrapper InnerExceptionXml
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty snapshot, as required by the XML serializer.</summary>
		public ExceptionWrapper()
		{ }

		/// <summary>Captures an exception and, recursively, its inner exceptions.</summary>
		/// <param name="exception">Exception to capture.</param>
		public ExceptionWrapper(Exception exception)
		{
			Class = exception.GetType().FullName;
			Message = exception.Message;
			StackTrace = exception.StackTrace;
			if (exception.InnerException != null)
			{
				InnerExceptionXml = new ExceptionWrapper(exception.InnerException);
			}
		}

		#endregion

		/// <summary>Formats the snapshot as its type name and message.</summary>
		/// <returns>Diagnostic string.</returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} : {1}", Class, Message);
		}

		/// <summary>Compares two snapshots by type, message, stack trace and inner exception.</summary>
		/// <param name="other">Snapshot to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> when both describe the same failure.</returns>
		public System.Boolean Equals(ExceptionWrapper other)
		{
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			if (Class != other.Class)
			{
				return false;
			}
			if (Message != other.Message)
			{
				return false;
			}
			if (StackTrace != other.StackTrace)
			{
				return false;
			}
			if ((InnerException == null) != (other.InnerException == null))
			{
				return false;
			}
			return (InnerException == null) || InnerException.Equals(other.InnerException);
		}

		/// <summary>Returns a hash code consistent with <see cref="Equals(ExceptionWrapper)"/>.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int hash =	Class.GetHashCode() ^
						Message.GetHashCode() ^
						StackTrace.GetHashCode();

			if (InnerException != null)
			{
				hash ^= InnerException.GetHashCode();
			}

			return hash;
		}
	}
}
