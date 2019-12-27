namespace Sef.Interfaces
{
	public interface IEditable<T>
	{
		void UpdateFrom(T other);
	}

    public static class EditableExtension
    {
        public static T Clone<T>(this T instance)
            where T: IEditable<T>, new()
        {
            var clone = new T();
            clone.UpdateFrom(instance);
            return clone;
        }
    }
}
