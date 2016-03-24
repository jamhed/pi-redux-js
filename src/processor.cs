define ["lib/jquery", "pi/Logger"], (jQuery, Logger) -> class Processor

	class: null
	count: null

	status_e: null # status element

	logger: null

	pi_run: 0
	pi_ajax: 0
	
	debug: -> @logger.debug "processor", arguments...
	err: -> @logger.err "processor", arguments...

	update_status: (@pi_run, @pi_ajax) ->
		# @debug "pi status: #{@pi_ajax} #{@pi_run}"
		@status_e.attr("run", @pi_run)
		@status_e.attr("ajax", @pi_ajax)

	constructor: ->
		@logger = new Logger

		@class   = {}
		@count   = {}

		$(document).ajaxSend (ev, xhr, r) =>
			@update_status @pi_run, @pi_ajax+1

		$(document).ajaxComplete (ev, xhr, s) =>
			@update_status @pi_run, @pi_ajax-1

		requirejs.onError = (err) =>
			@update_status @pi_run-1, @pi_ajax if @pi_run > 0
			@err "type:", err.requireType, "module:", err.requireModules, "err:", err
		
		@status_e = $("<div>").attr("id", "pi-status")
		$("body").append(@status_e)

		@pi document

	pi_bind: (name, e) ->
		# @debug "pi_bind", name, e
		if e.attr "processed"
			# @debug "pi already bound:", name, e
			return 
		@count[name] = if @count[name] then @count[name] + 1 else 1
		e.attr "processed", @count[name]
		new @class[name] @, e, "[pi='#{name}'][processed=#{@count[name]}]" # name + "/" + @count[name]

	pi: (context) ->
		@update_status @pi_run+1, @pi_ajax

		stack = []
		$("[pi]", context).each (i,_e) =>
			e = $ _e
			name = e.attr("pi")

			if @class[name]
				@pi_bind name, e
			else
				stack.push name: name, e: e

		seen = {}
		ustack = []
		for s in stack
			do (s) ->
				if (! seen[s.name])
					ustack.push s 
					seen[s.name] = 1
				
		unames = (s.name for s in ustack)

		require unames, (Names...) =>
			while s = ustack.shift()
				Name = Names.shift()
				if ! @class[name]
					# @debug "pi stacked", s.name
					@class[s.name] = Name
					Name.init @

			while s = stack.shift()
				@pi_bind s.name, s.e
			
			@update_status @pi_run-1, @pi_ajax
