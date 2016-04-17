define ["lib/URI/URI", "pi/m/Source", "pi/Logger"], (URI, mSource, Logger) -> class aPi

	a:          null
	data:       null
	e:          null
	uid:        null

	processor:  null

	cb_table:   null
	hn_table:   null

	waitTimeout: 5000
	retryTimeout: 100

	@init: -> # class method, called from processor on module load

	attr: -> ["uid"]

	_localget: (k) ->
		try
			localStorage[k]
		catch e
			@err "localstorage fail."

	_localset: (k,v) -> 
		try
			localStorage[k] = v
		catch e
			@err "localstorage fail set."

	localSet: (k,v) -> @_localset(@uid + "/" + k, v);
	localGet: (k)   -> @_localget(@uid + "/" + k)

	# [pi='Login/Status'][processed=2]
	_parse_uid: (uid) ->
		re = /\=\'(.+?)\'.*?=(\d+)/
		m = re.exec uid
		return "#{m[1]}[#{m[2]}]"
	
	self: -> @

	constructor: (@processor, @e, @uid) ->
		@cb_table = {}
		@hn_table = {}
		@a ?= {}
		@a[a] ?= @e.attr(a) for a in @attr()
		@data = $.extend({}, @e.data())
		@uid = @a.uid if @a.uid

		@e.data "pi", @

		@logger = new Logger

		# event handler to call object methods via events
		@handler "rpc", (e, args) =>
			if ! @[args.method]
				return @err "Method is not defined:", args.method, "for:", @uid
			r = @[args.method](args.args...)
			args.callback r if args.callback

		@handler "bound", (e, args) => @

		@init()

	debug: ->
		@logger.debug @.constructor.name, arguments...
		$("#log").append arguments, "\n"

	err: -> @logger.err arguments...
 
	init: ->

	# rpc: call method defined for pi-element defined by target
	rpc: (targets, args, callback) ->
		msgre = /\s*(.*?)\@(\S+)\s*/g
		seen = 0
		
		while m = msgre.exec targets
			seen = 1
			[selector, method] = [m[1], m[2]]
			@pub "#{selector}@rpc", { method: method, args: args, callback: callback }
		
		if seen == 0
			@err "Targets syntax error:", targets, @uid

	rpc_to: (target, method, args, callback) ->
		@pub "#{target}@rpc", { method: method, args: args, callback: callback }

	rpc_el: (el, method, args, callback) ->
		@msg_to el, "rpc", { method: method, args: args, callback: callback }

	handler: (ev, f) ->
		@e.on ev, (_e, args) => f(_e, args.args, args.caller)

	sub: (ev_full, f) ->
		@cb_table[ev_full] = f
		if m = /^(.*)\@(.*)$/.exec ev_full
			[target, ev_short] = [m[1], m[2]]
			@rpc_to target, "handler_table", [@uid, ev_full, ev_short]

	unsub: (ev_full) ->
		delete @cb_table[ev_full]
		if m = /^(.*)\@(.*)$/.exec ev_full
			[target, ev_short] = [m[1], m[2]]
			@rpc_to target, "unhandler_table", [@uid, ev_full, ev_short]

	unhandler_table: (sender_uid, ev_full, ev_short) ->
		# @debug "unregister callback for", ev_short, "to", @_parse_uid(sender_uid)
		if @hn_table[ev_short]
			delete @hn_table[ev_short][sender_uid]

	callback: (ev_full, _e, args) ->
		# @debug "execute callback on", ev_full
		@cb_table[ev_full](_e, args.args)

	handler_table: (sender_uid, ev_full, ev_short) ->
		# @debug "register callback for", ev_short, "to", @_parse_uid(sender_uid)
		if @hn_table[ev_short] == undefined
			@hn_table[ev_short] = {}
			@hn_table[ev_short][sender_uid] = ev_full
			@e.on ev_short, (_e, args) => @handler_table_call ev_short, _e, args
		else
			@hn_table[ev_short][sender_uid] = ev_full

	handler_table_call: (ev_short, _e, args) ->
		for sender_uid, ev_full of @hn_table[ev_short]
			@rpc "#{sender_uid}@callback", [ev_full, _e, args]

	pub: (targets, args) ->
		msgre = /\s*(.*?)\@(\S+)\s*/g
		while m = msgre.exec targets
			[selector, message] = [m[1] || "[pi]", m[2]]
			next if ! message
			if selector == "parent"
				@pub_to_parent message, args
			else if (cl = /^closest\((.*?)\)\s*(.*)$/.exec selector)
				@pub_to_closest cl, message, args
			else
				@pub_to_selector selector, message, args

	pub_to_parent: (message, args) ->
		el = @e.parent().closest("[pi]")
		if el.attr("processed")
			@msg_to el, message, args
		else
			@wait_to el, message, args

	pub_to_closest: (cl, message, args) ->
		el = @e.closest cl[1]
		el = el.find cl[2] if cl[2]
		if el.attr("processed")
			@msg_to el, message, args
		else
			@wait_to el, message, args

	pub_to_selector: (selector, message, args) ->
		if ! $(selector).length || ! $(selector).attr "processed"
			do (selector, message, args) =>
				# @debug "wait", selector, message, args
				@wait (() => @exists(selector)), (() => @send_message selector, message, args),
					"pub_to_selector() #{selector} #{message}"
		else
			@send_message selector, message, args

	wait_to: (el, message, args) ->
		@wait (() => el.attr "processed"), (() => @msg_to el, message, args), "wait_to() #{message}"

	send_message_to: (el, message, args) ->
		e = $ el
		if ! (e.data("events") || $._data(el, "events"))?[message]
			@err "No handler for message: #{message}, target: #{el}" + " dst: pi=" + e.attr("pi") + " src: pi=" + o.uid
		else
			@msg_to e, message, args
	
	send_message: (selector, message, args) ->
		$(selector).each (i, _e) => @send_message_to _e, message, args

	msg_to: (target, message, args) -> target.triggerHandler message, args: args, caller: @

	event: (message, args) -> @msg_to @e, message, args

	exists: (selector) -> $(selector).length > 0

	wait: (check, action, error = "") ->
		start = new Date().getTime()
		handler = setInterval (() =>
			status = check()
			if status
				clearInterval handler
				action()
			if (new Date().getTime() - start > @waitTimeout)
				@err "wait() timeout", error
				clearInterval handler
		), @retryTimeout

	wait_existance: (selector, action) -> @wait (() => @exists selector), action
	
	wait_ajax_done: (action) -> @wait_existance "#pi-status[ajax=0][run=0]", action

	tmpl: (name, r) -> 
		tmpl = mSource.get name
		if tmpl
			return if r then tmpl r else tmpl
		else
			@err "No template with name #{name}"

	process: (e = @e) -> @processor.pi e

	append: (tmpl, args) -> @process $("<div>").append @tmpl(tmpl, args) 

	parse_uri: ->
		@uri = URI window.location.hash.replace(/^\#\!/, "#").replace(/^(.*)\?(.*)$/,"?$2$1")
		@uri.hash("content") if @uri.hash() == "" or @uri.hash() == "index"
		return @uri

	get_uri: -> URI window.location

	redirect: (uri) ->
		window.location.hash = uri

	reload: (uri) ->
		window.location = uri

	die: ->
		for ev_full,v of @cb_table
			# @debug "instance gone", @_parse_uid(@uid), ev_full
			@unsub ev_full

	clear: (scope = @e) ->
		# time to die
		$("[pi]", scope).each (i,_e) =>
			e = $ _e
			@rpc_el e, "die"
		scope.empty()

	chain: (targets, args = @data) ->
		msgre = /^\s*(.*?)\@(\S+)\s*(.*)$/g

		while m = msgre.exec targets
			[selector, method, rest] = [m[1], m[2], m[3]]
			@pub "#{selector}@rpc",
				method: method,
				args: [args],
				callback: (r) =>
					if r["then"]
						r.then (rs) => @chain(rest, rs)
					else
						@chain(rest, r)
