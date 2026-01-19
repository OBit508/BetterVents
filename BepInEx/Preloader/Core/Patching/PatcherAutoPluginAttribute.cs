using System;
using System.Diagnostics;

namespace BepInEx.Preloader.Core.Patching
{
	// Token: 0x0200003F RID: 63
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	[Conditional("CodeGeneration")]
	internal sealed class PatcherAutoPluginAttribute : Attribute
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00007847 File Offset: 0x00005A47
		public PatcherAutoPluginAttribute(string id = null, string name = null, string version = null)
		{
		}
	}
}
