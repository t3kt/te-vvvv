using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace VVVV.Lib
{

	#region KeysUtil

	[TESharedAnnotations.Incomplete]
	internal static class KeysUtil
	{

		internal static readonly KeyConverter KeyConverter = new KeyConverter();
		internal static readonly ModifierKeysConverter ModifierKeysConverter = new ModifierKeysConverter();

	}

	#endregion

}
