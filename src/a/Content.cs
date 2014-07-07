define ["a/El"], (P) -> class aContent extends P
   
   # props

   cache: null
   ee: null

   # methods

   constructor: ->
      @cache = {}
      super

   init: ->

      @sub "cache/clear", (ev, args) =>
         if args?.k
            @cache[args.k] = {}
         else
            @cache = {}

      @sub "router@hash/change", (ev, args) =>
         @active args.uri
         return @load()
         
         k = args.ev.oldURL
         kk = args.ev.newURL

         @cache[k] = {} if ! @cache[k]
         @cache[k].ee = @ee.detach()
         @cache[k].text = @text
         @cache[k].uri = @uri

         if @cache[kk]
            @ee = @cache[kk].ee
            @text = @cache[kk].text
            @uri = @cache[kk].uri
            @ee.appendTo @e.empty()
         else
            @load()

      super
   
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