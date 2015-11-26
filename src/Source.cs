define ["pi/El", "pi/m/Source"], (aEl, mSource) -> class aSource extends aEl

	attr: -> super.concat ["name"]

	set: (text) ->
		@debug @a.name
		new mSource @a.name, text

	up: (args) ->
		tmpl = @tmpl @a.name
		@append tmpl o: args, data: @data