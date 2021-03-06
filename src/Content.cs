define ["pi/El"], (P) -> class aContent extends P

	attr: -> super.concat ["router"]

	cache: null
	ee: null

	init: ->
		@sub "#{@a.router}@hash/change", (ev, args) => return @load(args)
		@load()

	active: (uri) ->
		$(".navbar li").removeClass "active"
		$(".navbar a[href=\"#{uri.hash()}\"]").parent().addClass "active"
 
	set: (@text) ->
		@clear()
		@ee = $("<div>").html(@text).appendTo(@e)
		@process @ee

	load: (uri = @parse_uri()) ->
		@active uri
		super
