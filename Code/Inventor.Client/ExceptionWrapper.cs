using System;
using System.Xml.Serialization;

namespace Inventor.Client
{
	public interface IExceptionWrapper
	{
		String Class
		{ get; }

		String Message
		{ get; }

		String StackTrace
		{ get; }

		IExceptionWrapper InnerException
		{ get; }
	}

	[Serializable]
	public sealed class ExceptionWrapper : IExceptionWrapper, IEquatable<ExceptionWrapper>
	{
		#region Properties

		[XmlElement]
		public String Class
		{ get; set; }

		[XmlElement]
		public String Message
		{ get; set; }

		[XmlElement]
		public String StackTrace
		{ get; set; }

		[XmlIgnore]
		public IExceptionWrapper InnerException
		{ get { return InnerExceptionXml; } }

		[XmlElement("InnerException")]
		public ExceptionWrapper InnerExceptionXml
		{ get; set; }

		#endregion

		#region Constructors

		public ExceptionWrapper()
		{ }

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

		public override String ToString()
		{
			return String.Format("{0} : {1}", Class, Message);
		}

		public Boolean Equals(ExceptionWrapper other)
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
	}
}
