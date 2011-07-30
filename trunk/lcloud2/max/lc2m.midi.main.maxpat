{
	"patcher" : 	{
		"fileversion" : 1,
		"rect" : [ 301.0, 452.0, 728.0, 480.0 ],
		"bglocked" : 0,
		"defrect" : [ 301.0, 452.0, 728.0, 480.0 ],
		"openrect" : [ 0.0, 0.0, 0.0, 0.0 ],
		"openinpresentation" : 0,
		"default_fontsize" : 12.0,
		"default_fontface" : 0,
		"default_fontname" : "Arial",
		"gridonopen" : 0,
		"gridsize" : [ 10.0, 10.0 ],
		"gridsnaponopen" : 0,
		"toolbarvisible" : 1,
		"boxanimatetime" : 200,
		"imprint" : 0,
		"enablehscroll" : 1,
		"enablevscroll" : 1,
		"devicewidth" : 0.0,
		"boxes" : [ 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "swap",
					"numoutlets" : 2,
					"fontsize" : 12.0,
					"outlettype" : [ "int", "int" ],
					"patching_rect" : [ 310.0, 290.0, 39.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 2,
					"id" : "obj-12"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "route",
					"numoutlets" : 2,
					"fontsize" : 12.0,
					"outlettype" : [ "", "" ],
					"patching_rect" : [ 290.0, 350.0, 38.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 1,
					"id" : "obj-28"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "zl rot 1",
					"numoutlets" : 2,
					"fontsize" : 12.0,
					"outlettype" : [ "", "" ],
					"patching_rect" : [ 110.0, 300.0, 47.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 2,
					"id" : "obj-24"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "live.dial",
					"varname" : "ctl.83",
					"numoutlets" : 2,
					"outlettype" : [ "", "float" ],
					"patching_rect" : [ 230.0, 80.0, 44.0, 47.0 ],
					"parameter_enable" : 1,
					"numinlets" : 1,
					"id" : "obj-20",
					"saved_attribute_attributes" : 					{
						"valueof" : 						{
							"parameter_mmax" : 127.0,
							"parameter_mmin" : 0.0,
							"parameter_type" : 0,
							"parameter_initial_enable" : 0,
							"parameter_shortname" : "#83",
							"parameter_modmax" : 127.0,
							"parameter_longname" : "ctl.83",
							"parameter_modmin" : 0.0,
							"parameter_linknames" : 0,
							"parameter_modmode" : 0,
							"parameter_info" : "",
							"parameter_units" : "",
							"parameter_order" : 0,
							"parameter_defer" : 0,
							"parameter_speedlim" : 1.0,
							"parameter_invisible" : 0,
							"parameter_steps" : 0,
							"parameter_annotation_name" : "",
							"parameter_exponent" : 1.0,
							"parameter_unitstyle" : 0
						}

					}

				}

			}
, 			{
				"box" : 				{
					"maxclass" : "live.dial",
					"varname" : "ctl.82",
					"numoutlets" : 2,
					"outlettype" : [ "", "float" ],
					"patching_rect" : [ 180.0, 80.0, 44.0, 47.0 ],
					"parameter_enable" : 1,
					"numinlets" : 1,
					"id" : "obj-21",
					"saved_attribute_attributes" : 					{
						"valueof" : 						{
							"parameter_mmax" : 127.0,
							"parameter_mmin" : 0.0,
							"parameter_type" : 0,
							"parameter_initial_enable" : 0,
							"parameter_shortname" : "#82",
							"parameter_modmax" : 127.0,
							"parameter_longname" : "#82",
							"parameter_modmin" : 0.0,
							"parameter_linknames" : 0,
							"parameter_modmode" : 0,
							"parameter_info" : "",
							"parameter_units" : "",
							"parameter_order" : 0,
							"parameter_defer" : 0,
							"parameter_speedlim" : 1.0,
							"parameter_invisible" : 0,
							"parameter_steps" : 0,
							"parameter_annotation_name" : "",
							"parameter_exponent" : 1.0,
							"parameter_unitstyle" : 0
						}

					}

				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "funnel 8 80",
					"numoutlets" : 1,
					"fontsize" : 12.0,
					"outlettype" : [ "list" ],
					"patching_rect" : [ 110.0, 270.0, 113.5, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 8,
					"id" : "obj-18"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "live.dial",
					"varname" : "ctl.81",
					"numoutlets" : 2,
					"outlettype" : [ "", "float" ],
					"patching_rect" : [ 130.0, 80.0, 44.0, 47.0 ],
					"parameter_enable" : 1,
					"numinlets" : 1,
					"id" : "obj-17",
					"saved_attribute_attributes" : 					{
						"valueof" : 						{
							"parameter_mmax" : 127.0,
							"parameter_mmin" : 0.0,
							"parameter_type" : 0,
							"parameter_initial_enable" : 0,
							"parameter_shortname" : "#81",
							"parameter_modmax" : 127.0,
							"parameter_longname" : "ctl.81",
							"parameter_modmin" : 0.0,
							"parameter_linknames" : 0,
							"parameter_modmode" : 0,
							"parameter_info" : "",
							"parameter_units" : "",
							"parameter_order" : 0,
							"parameter_defer" : 0,
							"parameter_speedlim" : 1.0,
							"parameter_invisible" : 0,
							"parameter_steps" : 0,
							"parameter_annotation_name" : "",
							"parameter_exponent" : 1.0,
							"parameter_unitstyle" : 0
						}

					}

				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "r lc2.midi.ctl.out",
					"numoutlets" : 1,
					"fontsize" : 12.0,
					"outlettype" : [ "" ],
					"patching_rect" : [ 510.0, 310.0, 94.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 0,
					"id" : "obj-14"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "s lc2.midi.ctl.out",
					"numoutlets" : 0,
					"fontsize" : 12.0,
					"patching_rect" : [ 110.0, 340.0, 96.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 1,
					"id" : "obj-8"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "coll lc2m.config.txt",
					"numoutlets" : 4,
					"fontsize" : 12.0,
					"outlettype" : [ "", "", "", "" ],
					"patching_rect" : [ 300.0, 240.0, 109.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 1,
					"id" : "obj-13",
					"saved_object_attributes" : 					{
						"embed" : 0
					}

				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "loadmess 1",
					"numoutlets" : 1,
					"fontsize" : 12.0,
					"outlettype" : [ "" ],
					"patching_rect" : [ 470.0, 50.0, 72.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 1,
					"id" : "obj-11"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "live.dial",
					"varname" : "ctl.80",
					"numoutlets" : 2,
					"outlettype" : [ "", "float" ],
					"patching_rect" : [ 80.0, 80.0, 44.0, 47.0 ],
					"parameter_enable" : 1,
					"numinlets" : 1,
					"id" : "obj-1",
					"saved_attribute_attributes" : 					{
						"valueof" : 						{
							"parameter_mmax" : 127.0,
							"parameter_mmin" : 0.0,
							"parameter_type" : 0,
							"parameter_initial_enable" : 0,
							"parameter_shortname" : "#80",
							"parameter_modmax" : 127.0,
							"parameter_longname" : "#80",
							"parameter_modmin" : 0.0,
							"parameter_linknames" : 0,
							"parameter_modmode" : 0,
							"parameter_info" : "",
							"parameter_units" : "",
							"parameter_order" : 0,
							"parameter_defer" : 0,
							"parameter_speedlim" : 1.0,
							"parameter_invisible" : 0,
							"parameter_steps" : 0,
							"parameter_annotation_name" : "",
							"parameter_exponent" : 1.0,
							"parameter_unitstyle" : 0
						}

					}

				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "s lc2.midi.out.name",
					"numoutlets" : 0,
					"fontsize" : 12.0,
					"patching_rect" : [ 530.0, 140.0, 114.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 1,
					"id" : "obj-10"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "r lc2.midi.out.name",
					"numoutlets" : 1,
					"fontsize" : 12.0,
					"outlettype" : [ "" ],
					"patching_rect" : [ 540.0, 340.0, 112.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 0,
					"id" : "obj-9"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "umenu",
					"numoutlets" : 3,
					"fontsize" : 11.595187,
					"outlettype" : [ "int", "", "" ],
					"patching_rect" : [ 470.0, 110.0, 142.0, 20.0 ],
					"items" : [ "Microsoft Synthesizer", ",", "Microsoft GS Wavetable Synth", ",", "Out To MIDI Yoke:  1", ",", "Out To MIDI Yoke:  2", ",", "Out To MIDI Yoke:  3", ",", "Out To MIDI Yoke:  4", ",", "Out To MIDI Yoke:  5", ",", "Out To MIDI Yoke:  6", ",", "Out To MIDI Yoke:  7", ",", "Out To MIDI Yoke:  8", ",", "LoopBe Internal MIDI", ",", "Automap MIDI" ],
					"fontname" : "Arial",
					"types" : [  ],
					"labelclick" : 1,
					"numinlets" : 1,
					"id" : "obj-2"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "midiinfo",
					"numoutlets" : 1,
					"fontsize" : 11.595187,
					"outlettype" : [ "" ],
					"patching_rect" : [ 470.0, 80.0, 50.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 2,
					"id" : "obj-3"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "newobj",
					"text" : "ctlout",
					"numoutlets" : 0,
					"fontsize" : 12.0,
					"patching_rect" : [ 510.0, 370.0, 46.0, 20.0 ],
					"fontname" : "Arial",
					"numinlets" : 3,
					"id" : "obj-4"
				}

			}
, 			{
				"box" : 				{
					"maxclass" : "message",
					"text" : "dump",
					"numoutlets" : 1,
					"fontsize" : 12.0,
					"outlettype" : [ "" ],
					"patching_rect" : [ 300.0, 210.0, 50.0, 18.0 ],
					"fontname" : "Arial",
					"numinlets" : 2,
					"id" : "obj-5"
				}

			}
 ],
		"lines" : [ 			{
				"patchline" : 				{
					"source" : [ "obj-5", 0 ],
					"destination" : [ "obj-13", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-11", 0 ],
					"destination" : [ "obj-3", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-9", 0 ],
					"destination" : [ "obj-4", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-2", 1 ],
					"destination" : [ "obj-10", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-3", 0 ],
					"destination" : [ "obj-2", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-14", 0 ],
					"destination" : [ "obj-4", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-1", 0 ],
					"destination" : [ "obj-18", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-17", 0 ],
					"destination" : [ "obj-18", 1 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-21", 0 ],
					"destination" : [ "obj-18", 2 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-20", 0 ],
					"destination" : [ "obj-18", 3 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-18", 0 ],
					"destination" : [ "obj-24", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
, 			{
				"patchline" : 				{
					"source" : [ "obj-24", 0 ],
					"destination" : [ "obj-8", 0 ],
					"hidden" : 0,
					"midpoints" : [  ]
				}

			}
 ],
		"parameters" : 		{
			"obj-1" : [ "#80", "#80", 0 ],
			"obj-20" : [ "ctl.83", "#83", 0 ],
			"obj-17" : [ "ctl.81", "#81", 0 ],
			"obj-21" : [ "#82", "#82", 0 ]
		}

	}

}
