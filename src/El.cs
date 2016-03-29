define ["pi/Pi"], (aPi) -> class aEl extends aPi

	text: null

	attr: -> super.concat ["uri", "get", "template"]

	init: ->
		@load()

	load: ->
		if @a.template
			return @set @tmpl @a.template, {}

		if @a.uri == undefined
			return @set @e.html()

		uri = @parse_uri()
		if @a.get == "" || @a.get
			$.get uri.fragment() + ".html", uri.query(), (text) => @set text
		else
			$.post uri.fragment(), (text) => @set text

	set: (@text) ->
		@e.html @text
		@process()
