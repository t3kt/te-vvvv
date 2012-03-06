/// <reference path="iannix_stubs.js"/>

var INX = (function ()
{

	function NOT_IMPLEMENTED(name)
	{
		throw new Error('Not implemented ' + name || '');
	}

	function extend(obj, ext)
	{
		obj = obj || {};
		if(ext)
		{
			for(var key in ext)
			{
				if(ext.hasOwnProperty(key))
					obj[key] = ext[key];
			}
		}
		return obj;
	}

	function runCmd(cmd, args)
	{
		if(args && args.length > 0)
		{
			args = cleanArgs(args);
			args.unshift(cmd);
			cmd = args.join(' ');
		}
		run(cmd);
	}

	function cleanArgs(args)
	{
		var cleaned = [],
		    arg;
		if(args)
		{
			for(var i = 0; i < args.length; i++)
			{
				arg = args[i];
				if(typeof (arg) !== 'undefined')
					cleaned.push(arg);
			}
		}
		return cleaned;
	}

	var commandNames = 'add autosize center fastrewind play registerColor registerColor2 registerTexture remove rotate ' +
		'setActive setBoundsSource setBoundsTarget setColorActive setColorActive2 setColorActiveMessage setColorActiveMessage2 ' +
			'setColorInactive setColorInactive2 setColorInactiveMessage setColorInactiveMessage2 setCurve setGroup setLabel setLine ' +
				'setMessage setOffset setPattern setPointAt setPointsEllipse setPointsImg setPointsSVG setPointsSVG2 setPointsTxt setPos ' +
					'setResize setSize setSpeed setSpeedF setTextureActive setTextureActiveMessage setTextureInactive setTextureInactiveMessage ' +
						'setTime setTriggerOff setWidth speed zoom'.split(' ');
	function mkCmd(name)
	{
		return function ()
		{
			runCmd(name, arguments);
		};
	}

	var lastId = 1;
	function nextId() { return lastId++; }

	var I = {
		runCmd: runCmd,
		nextId: nextId,
		clear: mkCmd('clear'),
		autosize: function (enabled)
		{
			I.cmd.autosize(enabled ? '1' : '0');
		},

		Group: IGroup,
		Obj: IObj,
		Trigger: ITrigger,
		Curve: ICurve,
		Cursor: ICursor
	};
	I.cmd = {
		add: mkCmd('add'),
		autosize: mkCmd('autosize'),
		center: mkCmd('center'),
		fastrewind: mkCmd('fastrewind'),
		play: mkCmd('play'),
		registerColor: mkCmd('registerColor'),
		registerColor2: mkCmd('registerColor2'),
		registerTexture: mkCmd('registerTexture'),
		remove: mkCmd('remove'),
		rotate: mkCmd('rotate'),
		setActive: mkCmd('setActive'),
		setBoundsSource: mkCmd('setBoundsSource'),
		setBoundsTarget: mkCmd('setBoundsTarget'),
		setColorActive: mkCmd('setColorActive'),
		setColorActive2: mkCmd('setColorActive2'),
		setColorActiveMessage: mkCmd('setColorActiveMessage'),
		setColorActiveMessage2: mkCmd('setColorActiveMessage2'),
		setColorInactive: mkCmd('setColorInactive'),
		setColorInactive2: mkCmd('setColorInactive2'),
		setColorInactiveMessage: mkCmd('setColorInactiveMessage'),
		setColorInactiveMessage2: mkCmd('setColorInactiveMessage2'),
		setCurve: mkCmd('setCurve'),
		setGroup: mkCmd('setGroup'),
		setLabel: mkCmd('setLabel'),
		setLine: mkCmd('setLine'),
		setMessage: mkCmd('setMessage'),
		setOffset: mkCmd('setOffset'),
		setPattern: mkCmd('setPattern'),
		setPointAt: mkCmd('setPointAt'),
		setPointsEllipse: mkCmd('setPointsEllipse'),
		setPointsImg: mkCmd('setPointsImg'),
		setPointsSVG: mkCmd('setPointsSVG'),
		setPointsSVG2: mkCmd('setPointsSVG2'),
		setPointsTxt: mkCmd('setPointsTxt'),
		setPos: mkCmd('setPos'),
		setResize: mkCmd('setResize'),
		setSize: mkCmd('setSize'),
		setSpeed: mkCmd('setSpeed'),
		setSpeedF: mkCmd('setSpeedF'),
		setTextureActive: mkCmd('setTextureActive'),
		setTextureActiveMessage: mkCmd('setTextureActiveMessage'),
		setTextureInactive: mkCmd('setTextureInactive'),
		setTextureInactiveMessage: mkCmd('setTextureInactiveMessage'),
		setTime: mkCmd('setTime'),
		setTriggerOff: mkCmd('setTriggerOff'),
		setWidth: mkCmd('setWidth'),
		speed: mkCmd('speed'),
		zoom: mkCmd('zoom')
	};
	I.play = I.cmd.play;
	I.fastrewind = I.cmd.fastrewind;

	I.SUPPRESS = 'suppress';

	function IGroup(name)
	{
		this.name = name || 'group' + nextId();
		this.objects = [];
		this.addObject = function (target)
		{
			if(target && target.id)
				target = target.id;
			I.cmd.setGroup(target, name);
			this.objects.push(target);
		};
	}

	function IObj(id, type)
	{
		if(id === false)
			return;
		if(!id || id == IObj.AUTO)
			id = (type || '') + I.nextId();
		this.id = id;
		if(type)
			I.cmd.add(type, id);
		this.setPos = function (x, y, z)
		{
			if(arguments.length == 1)
				I.cmd.setPos(this.id, x.x, x.y, x.z);
			else
				I.cmd.setPos(this.id, x, y, z);
		};
		this.setActive = function (active)
		{
			if(arguments.length == 0)
				active = true;
			I.cmd.setActive(this.id, active ? '1' : '0');
		};
		this.setLabel = function (label)
		{
			I.cmd.setLabel(this.id, label);
		};
		this.setSize = function (size)
		{
			I.cmd.setSize(this.id, size);
		};
		this.remove = function ()
		{
			I.cmd.remove(this.id);
		};
		this.setMessages = function (period, mesg1, mesg2)
		{
			var messages,
			    args = [period],
			    message;
			if(arguments.length == 2 && Array.isArray(mesg1))
				messages = mesg1;
			else
				messages = Array.prototype.slice.call(arguments, 1);
			for(var i = 0; i < messages.length; i++)
			{
				message = messages[i];
				if(typeof (message) == 'string')
				{
					args.push(message);
				}
				else if(Array.isArray(message))
				{
					args.push(cleanArgs(message).join(' '));
				}
				else
				{
					args.push(cleanArgs([message.msg].concat(message.args || [])).join(' '));
				}
			}
			args = args.join(', ');
			I.cmd.setMessage(this.id, args);
		};
	}
	IObj.AUTO = 'auto';
	IObj.CURRENT = 'current';
	IObj.TRIGGER = 'trigger';
	IObj.CURSOR = 'cursor';
	IObj.CURVE = 'curve';

	function ITrigger(id)
	{
		IObj.call(this, id, IObj.TRIGGER);
		this.setTriggerOff = function (delay)
		{
			I.cmd.setTriggerOff(this.id, delay);
		};
	}
	ITrigger.prototype = new IObj(false);
	ITrigger.ID = 'trigger_id';
	ITrigger.GROUP_ID = 'trigger_group_id';
	ITrigger.DOCUMENT_ID = 'tigger_document_id';
	ITrigger.X_POS = 'trigger_xPos';
	ITrigger.Y_POS = 'trigger_yPos';
	ITrigger.DISTANCE = 'trigger_distance';
	ITrigger.VALUE = 'trigger_value';
	ITrigger.SIDE = 'trigger_side';

	function ICursor(id)
	{
		IObj.call(this, id, IObj.CURSOR);
		this.setCurve = function (curve)
		{
			this.curveId = (curve && curve.id) || curve;
			I.cmd.setCurve(this.id, this.curveId);
		};
		this.setWidth = function (width)
		{
			this.width = width;
			I.cmd.setWidth(this.id, width);
		};
		this.setTime = function (time)
		{
			I.cmd.setTime(this.id, time);
		};
		this.resetTime = function ()
		{
			this.setTime(0);
		};
		this.setOffest = function (initial, start, end)
		{
			I.cmd.setOffset(this.id, initial, start, end);
		};
		this.setBoundsSource = function (bounds)
		{
			I.cmd.setBoundsSource(this.id, bounds.x[0], bounds.y[0], bounds.x[1], bounds.y[1]);
		};
		this.setBoundsTarget = function (bounds)
		{
			I.cmd.setBoundsTarget(this.id, bounds.x[0], bounds.y[0], bounds.x[1], bounds.y[1]);
		};
		this.setBounds = function (bounds)
		{
			this.setBoundsSource(bounds.source);
			this.setBoundsTarget(bounds.target);
		};
	}
	ICursor.prototype = new IObj(false);
	ICursor.ID = 'cursor_id';
	ICursor.GROUP_ID = 'cursor_group_id';
	ICursor.DOCUMENT_ID = 'cursor_document_id';
	ICursor.X_POS = 'cursor_xPos';
	ICursor.Y_POS = 'cursor_yPos';
	ICursor.TIME = 'cursor_time';
	ICursor.TIME_PERCENT = 'cursor_time_percent';
	ICursor.VALUE_X = 'cursor_value_x';
	ICursor.VALUE_Y = 'cursor_value_y';
	ICursor.ANGLE = 'cursor_angle';
	ICursor.NB_LOOP = 'cursor_nb_loop';
	ICursor.COLLISION_CURVE_ID = 'collision_curve_id';
	ICursor.COLLISION_X_POS = 'collision_xPos';
	ICursor.COLLISION_Y_POS = 'collision_yPos';
	ICursor.COLLISION_VALUE_X = 'collision_value_x';
	ICursor.COLLISION_VALUE_Y = 'collision_value_y';
	ICursor.COLLISION_DISTANCE = 'collision_distance';

	function makeSetPointAtArgs(id, index, pos, delta)
	{
		var args = [id, index, pos.x, pos.y];
		if(pos.hasOwnProperty('z'))
			args.push(pos.z);
		if(delta)
		{
			args.push(delta.x[0]);
			args.push(delta.y[0]);
			if(delta.z)
				args.push(delta.z[0]);
			args.push(delta.x[1]);
			args.push(delta.y[1]);
			if(delta.z)
				args.push(delta.z[1]);
		}
		return args;
	}
	function ICurve(id)
	{
		IObj.call(this, id, IObj.CURVE);
		var lastPoint = -1;
		this.setPoint = function (index, pos, delta)
		{
			I.cmd.setPointAt.apply(I.cmd, makeSetPointAtArgs(this.id, index, pos, delta));
			if(index > lastPoint)
				lastPoint = index;
			return index;
		};
		this.setNextPoint = function (pos, delta)
		{
			return this.setPoint(++lastPoint, pos, delta);
		};
		this.setPointsEllipse = function (width, height)
		{
			I.cmd.setPointsEllipse(this.id, width, height);
		};
		this.setPoints = function (points)
		{
			var i;
			lastPoint = -1;
			if(arguments.length === 1 && Array.isArray(points))
			{
				for(i = 0; i < points.length; i++)
					this.setNextPoint(points[i].pos, points[i].delta);
			}
			else
			{
				for(i = 0; i < arguments.length; i++)
					this.setNextPoint(arguments[i].pos, arguments[i].delta);
			}
			return lastPoint;
		};
		this.setPointsTxt = function (scaling, font, text)
		{
			I.cmd.setPointsTxt(this.id, scaling, font, text);
		};
		this.setResize = function (width, height)
		{
			I.cmd.setResize(this.id, width, height);
		};
	}
	ICurve.prototype = new IObj(false);
	ICurve.ID = 'curve_id';
	ICurve.GROUP_ID = 'curve_group_id';
	ICurve.DOCUMENT_ID = 'curve_document_id';
	ICurve.X_POS = 'curve_xPos';
	ICurve.Y_POS = 'curve_yPos';
	ICurve.LAST = 'lastCurve';


	return I;
})();
