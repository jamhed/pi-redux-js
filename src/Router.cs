define ["pi/Pi", "lib/URI/URI"], (P, URI) -> class Router extends P

	skipHashChangeOnce: false
	uri: null

	init: ->
		@uri = @parse_uri()

		window.onhashchange = (ev) =>
			if @skipHashChangeOnce
				@skipHashChangeOnce = false
				return
			@event "hash/change", @parse_uri()

	reset: ->
		@skipHashChangeOnce = true
		window.location.search = ""

	set_uri: (uri = @uri, skip = false) ->
		if skip
			@skipHashChangeOnce = true
		window.location.hash = uri.hash() + uri.search()

	set_uri_text: (text, skip = false) ->
		if skip
			@skipHashChangeOnce = true   
		window.location = text

	set_hash: (s, skip) ->
		if skip
			@skipHashChangeOnce = true  
		window.location.hash = s
