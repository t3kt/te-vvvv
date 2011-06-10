using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Core.IO;

namespace Animator.Core.Model
{

	#region Schema

	internal static class Schema
	{

		public static readonly XName common_id = "id";
		public static readonly XName common_name = "name";
		public static readonly XName common_dur = "dur";
		public static readonly XName common_type = "type";

		public static readonly XName track_target = "target";

		public static readonly XName @params = "params";
		public static readonly XName params_param = "param";
		public static readonly XName params_param_key = "key";

		public static readonly XName anidoc = "anidoc";
		public static readonly XName anidoc_id = common_id;
		public static readonly XName anidoc_name = common_name;
		public static readonly XName anidoc_bpm = "bpm";
		public static readonly XName anidoc_outputs = "outputs";
		public static readonly XName anidoc_sequences = "sequences";
		public static readonly XName anidoc_sessions = "sessions";

		public static readonly XName transport = "transport";
		public static readonly XName transport_type = common_type;
		public static readonly XName transport_bpm = "bpm";
		public static readonly XName transport_params = @params;

		public static readonly XName output = Output.Export_ElementName;

		public static readonly XName traceoutput = TraceOutput.Export_ElementName;
		public static readonly XName traceoutput_category = "category";
		public static readonly XName traceoutput_prefix = "prefix";

		public static readonly XName logoutput = LogOutput.Export_ElementName;
		public static readonly XName logoutput_path = "path";
		public static readonly XName logoutput_append = "append";

		public static readonly XName target = "target";
		public static readonly XName target_key = "key";

		public static readonly XName target_prop = "prop";
		public static readonly XName target_prop_name = common_name;
		public static readonly XName target_prop_type = common_type;
		public static readonly XName target_prop_default = "default";

		public static readonly XName sequence = "sequence";
		public static readonly XName sequence_dur = common_dur;

		public static readonly XName seqtrack = "seqtrack";

		public static readonly XName session = "session";
		public static readonly XName session_rows = "rows";

		public static readonly XName sestrack = "sestrack";

		public static readonly XName constprop = Clips.ConstData.Export_ElementName;
		public static readonly XName constprop_value = "value";

		public static readonly XName stepprop = Clips.StepData.Export_ElementName;
		public static readonly XName stepprop_step = "step";

		public static readonly XName clipbase_props = "props";

		public static readonly XName sesclip = "sesclip";
		public static readonly XName sesclip_row = "row";
		public static readonly XName sesclip_state = "state";
		public static readonly XName sesclip_dur = common_dur;

		public static readonly XName seqclip = "seqclip";
		public static readonly XName seqclip_start = "start";
		public static readonly XName seqclip_dur = common_dur;

	}

	#endregion

}
