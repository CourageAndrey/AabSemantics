using System;

namespace Sef.Program
{
    public interface IChangeable
    {
        event EventHandler Changed;
    }
}
