define ["pi/Pi", "pi/lib/URI/URI"], (aPi, URI) -> class aEl extends aPi

   # props

   uri:  null
   text: null

   # methods

   attr: -> super.concat ["uri", "get"]

   init: ->
      @uri = URI "#" + @a.uri
      @load()

   load: ->
      if @a.uri == undefined
         return @set @e.html()
   
      if @a.get == "" || @a.get
         $.get @uri.fragment(), @uri.query(), (text) => @set text
      else
         @post @uri.fragment(), {}, (text) => @set text

   set: (@text) ->
      @e.html @text
      @rt.pi @e
