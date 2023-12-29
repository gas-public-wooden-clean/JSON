using CER.Json;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("defdd47f-c3a2-4f28-aed9-663ec7cb0656")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyVersion(AssemblyInfo.VersionString)]
[assembly: AssemblyFileVersion(AssemblyInfo.VersionString)]

namespace CER.Json
{
	/// <summary>
	/// Information about this .NET assembly. The constants in this class can be used to determine
	/// which version of this assembly was used at compile-time.
	/// </summary>
	public static class AssemblyInfo
	{
		/// <summary>
		/// Major version number. Will be incremented for breaking changes.
		/// </summary>
		public const int Major = 2;

		/// <summary>
		/// Minor version number. Will be incremented for significant changes that are intended to be
		/// backwards compatible.
		/// </summary>
		public const int Minor = 1;

		/// <summary>
		/// Build number. A difference in build number represents a recompilation of the same source.
		/// Different build numbers might be used when the processor, platform, or compiler changes.
		/// </summary>
		public const int Build = 0;

		/// <summary>
		/// Revision number. Will be incremented for insignificant changes or changes that have low
		/// risk, but high impact e.g. security vulnerabilities.
		/// </summary>
		public const int Revision = 0;

		/// <summary>
		/// Version number, formatted as "Major.Minor.Build.Revision".
		/// </summary>
		public const string VersionString = "2.1.0.0";
	}
}
