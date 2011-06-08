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
		public static readonly XName anidoc_dur = common_dur;
		public static readonly XName anidoc_outputs = "outputs";
		public static readonly XName anidoc_clips = "clips";
		public static readonly XName anidoc_sequences = "sequences";
		public static readonly XName anidoc_sessions = "sessions";
		public static readonly XName anidoc_ui_rows = "ui-rows";
		public static readonly XName anidoc_ui_cols = "ui-cols";

		public static readonly XName transport = "transport";
		public static readonly XName transport_type = common_type;
		public static readonly XName transport_bpm = "bpm";
		public static readonly XName transport_params = @params;

		public static readonly XName output = "output";

		public static readonly XName traceoutput = "traceoutput";
		public static readonly XName traceoutput_category = "category";
		public static readonly XName traceoutput_prefix = "prefix";

		public static readonly XName target = "target";
		public static readonly XName target_key = "key";

		public static readonly XName target_prop = "prop";
		public static readonly XName target_prop_name = common_name;
		public static readonly XName target_prop_type = common_type;
		public static readonly XName target_prop_default = "default";

		public static readonly XName clip = "clip";
		public static readonly XName clip_dur = common_dur;
		public static readonly XName clip_target = "target";
		public static readonly XName clip_output = "output";
		public static readonly XName clip_ui_row = "ui-row";
		public static readonly XName clip_ui_col = "ui-col";

		public static readonly XName stepclip = "stepclip";
		public static readonly XName stepclip_step = "step";

		public static readonly XName clipref = "clipref";
		public static readonly XName clipref_clip_id = "clip-id";

		public static readonly XName sequence = "sequence";
		public static readonly XName sequence_dur = common_dur;

		public static readonly XName seqtrack = "seqtrack";

		public static readonly XName seqclipref = "seqclipref";
		public static readonly XName seqclipref_start = "start";
		public static readonly XName seqclipref_dur = common_dur;

		public static readonly XName session = "session";
		public static readonly XName session_rows = "rows";

		public static readonly XName sestrack = "sestrack";

		public static readonly XName sesclipref = "sesclipref";
		public static readonly XName sesclipref_row = "row";
		public static readonly XName sesclipref_state = "state";

		public static readonly XName constprop = "constprop";
		public static readonly XName constprop_value = "value";

		public static readonly XName clipbase_props = "props";

	}

	#endregion

}
