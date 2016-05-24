using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCopyright("Copyright © Miguel Hasse de Oliveira")]
[assembly: AssemblyProduct("Hasseware .NET Framework Extensions")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

#if !PORTABLE
[assembly: ComVisible(false)]
#endif
[assembly: CLSCompliant(false)]
