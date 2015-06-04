define ["pi/El"], (P) -> class aContent extends P
   
   attr: -> super.concat ["router"]

   # props

   cache: null
   ee: null

   # methods

   init: ->

      @sub "#{@a.router}@hash/change", (ev, args) => return @load(args)

      @load()
            
   active: (uri)  ->
      $(".navbar li").removeClass "active"
      $(".navbar a[href=\"#{uri.hash()}\"]").parent().addClass "active"
 
   set: (@text) ->
      @empty()
      @ee = $("<div>").html(@text).appendTo(@e)
      @rt.pi @ee
   
   load: (uri = @parse_uri()) ->
      @active uri
      super
