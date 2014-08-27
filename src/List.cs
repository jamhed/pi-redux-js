define ["pi/Pi"], (P) -> class piList extends P

   attr: -> super.concat ["uri", "row", "current", "name"]

   init: ->
      return @debug "no uri attr", @e[0] if ! @a.uri
      return @debug "no row attr", @e[0] if ! @a.row
      @load()

   get: -> return name: @a.name, value: @e.val()

   load: (data = @data) -> @post @a.uri, data, (r) => @onData(r)

   onData: (@r) ->
      row_tmpl = @rt.source @a.row
      @e.empty()
      @e.append row_tmpl { o: row, current: @a.current } for row in r.rr
      @rt.pi @e