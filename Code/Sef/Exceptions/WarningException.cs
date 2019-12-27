using System;
using System.Drawing;

using Sef.Interfaces;

namespace Sef.Exceptions
{
	public class WarningException : Exception, IHasImage
	{
        public Image Image
        { get { return Properties.Resources.Warning; } }

        public WarningException(String message)
            : base(message)
        { }

        public WarningException(String message, Exception innerException)
            : base(message, innerException)
        { }
	}
}
