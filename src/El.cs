define ["pi/Pi"], (aPi) -> class aEl extends aPi

   # props

   text: null

   # methods

   attr: -> super.concat ["uri", "get"]

   init: ->
      @load()

   load: ->
      if @a.uri == undefined
         return @set @e.html()
   
      uri = @parse_uri()
      if @a.get == "" || @a.get
         $.get uri.fragment() + ".html", uri.query(), (text) => @set text
      else
         @post uri.fragment(), {}, (text) => @set text

   set: (@text) ->
      @e.html @text
      @rt.pi @e
