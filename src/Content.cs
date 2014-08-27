define ["pi/El"], (P) -> class aContent extends P
   
   # props

   cache: null
   ee: null

   # methods

   init: ->

      @sub "router@hash/change", (ev, args) =>
         @active args.uri
         return @load()

      @load()
            
   active: (uri)  ->
      $(".navbar li").removeClass "active"
      $(".navbar a[href=\"#{uri.hash()}\"]").parent().addClass "active"
 
   set: (@text) ->
      @e.empty()
      @ee = $("<div>").html(@text).appendTo(@e)
      @rt.pi @ee
   
   load: ->
      @uri = @rt.uri
      @active @uri
      super
