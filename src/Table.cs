define ["a/Pi"], (aPi) -> class Table extends aPi

   r: null

   attr: -> super.concat ["uri"]

   get_r: -> return @r

   get_id: ->
      return @e.attr 'id'

   page: (page) -> @load page: page

   load: (p) ->
      @post @a.uri, p, (@r) =>
         @event "data", r