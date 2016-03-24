define ["conf/logger.js"], (Conf) -> class Logger

	sys_debug: ->
	sys_error: ->

	debug: ->
		[module, args...] = arguments
		if not Conf[module]? or Conf[module]
			@sys_debug arguments...

	err: ->
		@sys_error arguments...
		err = new Error('')
		@sys_debug err.stack

	constructor: ->
		if window.console && Function.prototype.bind && (typeof console.log == "object" || typeof console.log == "function")
			@sys_debug = Function.prototype.bind.call(console.log, console)
			@sys_error = Function.prototype.bind.call(console.error, console)

		window.onerror = (msg, url, line) => @err "onerror()", url, msg, line