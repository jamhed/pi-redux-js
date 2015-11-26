define [], -> class aPromise

	defer: null
	next:  null
	r:     null
	state: false

	done: (@r) ->
		@state = true

	success: (r) -> 
		@done r

		if @defer
			res = @defer r
			@next.success res if @next

	then: (@defer) ->

		@next = new aPromise()

		if @state
			@next.done @defer r

		return @next
