using System;
using System.Diagnostics;

namespace BepInEx
{
	// Token: 0x0200003E RID: 62
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[Conditional("CodeGeneration")]
	internal sealed class BepInAutoPluginAttribute : Attribute
	{
		// Token: 0x0600011A RID: 282 RVA: 0x0000783F File Offset: 0x00005A3F
		public BepInAutoPluginAttribute(string id = null, string name = null, string version = null)
		{
		}
	}
}
