define ["pi/Pi"], (aPi) -> class aTable extends aPi

   r: null

   attr: -> super.concat ["uri"]

   get_r: -> return @r

   get_id: ->
      return @e.attr 'id'

   page: (page) -> @load page: page

   load: (p) -> @post @a.uri + "/table", p, (@r) => @event "data", r

   update: (p) -> return @ppost @a.uri + "/update", p

   delete: (pp) ->
      @post @a.uri + "/delete", id: p.value if p.name == "id" for p in pp.form