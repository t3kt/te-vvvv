using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Animator.Core.Model
{

	#region Schema

	internal static class Schema
	{
		public static readonly XName id = "id";
		public static readonly XName name = "name";

		public static readonly XName anidoc = "anidoc";
		public static readonly XName anidoc_id = "id";
		public static readonly XName anidoc_name = "name";
		public static readonly XName anidoc_bpm = "bpm";
		public static readonly XName anidoc_dur = "dur";
		public static readonly XName anidoc_align = "align";
		public static readonly XName anidoc_params = "params";

		public static readonly XName output = "output";
		public static readonly XName output_id = "id";
		public static readonly XName output_name = "name";
		public static readonly XName output_type = "type";
		public static readonly XName output_params = "params";

		public static readonly XName track = "track";
		public static readonly XName track_id = "id";
		public static readonly XName track_name = "name";
		public static readonly XName track_output = "output";
		public static readonly XName track_params = "params";

		public static readonly XName clip = "clip";
		public static readonly XName clip_id = "id";
		public static readonly XName clip_name = "name";
		public static readonly XName clip_dur = "dur";
		public static readonly XName clip_align = "align";
		public static readonly XName clip_type = "type";
		public static readonly XName clip_params = "params";

		public static readonly XName @params = "params";
		public static readonly XName params_param = "param";
		public static readonly XName params_param_key = "key";

	}

	#endregion

}
