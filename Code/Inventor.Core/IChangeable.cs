using System;

namespace Inventor.Core
{
	public interface IChangeable
	{
		event EventHandler Changed;
	}
}
