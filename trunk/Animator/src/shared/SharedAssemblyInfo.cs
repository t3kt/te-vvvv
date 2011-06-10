using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("TE")]
[assembly: AssemblyProduct("Animator")]
[assembly: AssemblyCopyright("Copyright © TE 2011")]

[assembly: ComVisible(false)]

//[assembly: AssemblyVersion("1.1.*")]
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]

#if ANIMATOR_TESTS_ASSEMBLY
#else
[assembly: InternalsVisibleTo(TEShared.AssemblyRef.AnimatorTests)]
#endif
