using System;

namespace AabSemantics
{
	public interface IIdentifiable
	{
		String ID
		{ get; }
	}

	public static class IdHelper
	{
		public static String EnsureIdIsSet(this String value)
		{
			return !String.IsNullOrEmpty(value)
				? value
				: Guid.NewGuid().ToString();
		}

		public static String GetTypeWithId(this IIdentifiable instance)
		{
			return $"{instance.GetType().Name} [{instance.ID}]";
		}
	}
}
