﻿using Inventor.Semantics;

namespace Samples.Semantics.Sample07.CustomModule
{
	public class CustomAttribute : IAttribute
	{
		protected CustomAttribute()
		{ }

		public static readonly CustomAttribute Value = new CustomAttribute();
	}
}
