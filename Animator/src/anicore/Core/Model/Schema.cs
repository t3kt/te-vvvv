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

		public static readonly XName @params = "params";
		public static readonly XName params_param = "param";
		public static readonly XName params_param_key = "key";

		public static readonly XName anidoc = "anidoc";
		public static readonly XName anidoc_id = "id";
		public static readonly XName anidoc_name = "name";
		public static readonly XName anidoc_bpm = "bpm";
		public static readonly XName anidoc_dur = "dur";
		public static readonly XName anidoc_align = "align";
		public static readonly XName anidoc_params = "params";
		public static readonly XName anidoc_outputs = "outputs";
		public static readonly XName anidoc_tracks = "tracks";
		public static readonly XName anidoc_clips = "clips";
		public static readonly XName anidoc_sequences = "sequences";
		public static readonly XName anidoc_ui_rows = "ui-rows";
		public static readonly XName anidoc_ui_cols = "ui-cols";

		public static readonly XName transport = "transport";
		public static readonly XName transport_type = "type";
		public static readonly XName transport_bpm = "bpm";
		public static readonly XName transport_dur = "dur";
		public static readonly XName transport_align = "align";
		public static readonly XName transport_params = "params";

		public static readonly XName output = "output";
		public static readonly XName output_id = "id";
		public static readonly XName output_name = "name";
		public static readonly XName output_type = "type";
		public static readonly XName output_params = "params";

		public static readonly XName track = "track";
		public static readonly XName track_id = "id";
		public static readonly XName track_name = "name";
		public static readonly XName track_output = "output";
		public static readonly XName track_target = "target";
		public static readonly XName track_params = "params";
		public static readonly XName track_clips = "clips";
		public static readonly XName track_clips_null = "null";

		public static readonly XName clip = "clip";
		public static readonly XName clip_id = "id";
		public static readonly XName clip_name = "name";
		public static readonly XName clip_dur = "dur";
		public static readonly XName clip_align = "align";
		public static readonly XName clip_target = "target";
		public static readonly XName clip_params = "params";
		public static readonly XName clip_output = "output";
		public static readonly XName clip_ui_row = "ui-row";
		public static readonly XName clip_ui_col = "ui-col";

		public static readonly XName stepclip = "stepclip";
		public static readonly XName stepclip_step = "step";

		public static readonly XName clipref = "clipref";
		public static readonly XName clipref_id = "id";
		public static readonly XName clipref_name = "name";
		public static readonly XName clipref_clip_id = "clip-id";

		public static readonly XName sequence = "sequence";
		public static readonly XName sequence_id = "id";
		public static readonly XName sequence_name = "name";
		public static readonly XName sequence_dur = "dur";
		public static readonly XName sequence_clips = "clips";

		public static readonly XName seqclipref = "seqclipref";
		public static readonly XName seqclipref_id = "id";
		public static readonly XName seqclipref_name = "name";
		public static readonly XName seqclipref_clip_id = "clip-id";
		public static readonly XName seqclipref_start = "start";
		public static readonly XName seqclipref_dur = "dur";

	}

	#endregion

}
