define ["pi/Pi", "pi/lib/URI/URI"], (P, URI) -> class Router extends P

   skipHashChangeOnce: false
   uri: null

   init: ->
      @uri = @parse_uri()

      window.onhashchange = (ev) =>
         if @skipHashChangeOnce
            @skipHashChangeOnce = false
            return
         @event "hash/change", @parse_uri()

   set_uri: (uri = @uri) ->
      window.location.hash = uri.hash() + uri.search()

   set_hash: (s) -> window.location.hash = s

   set_uri_without_event: ->
      @skipHashChangeOnce = true
      window.location.hash = @uri.hash() + @uri.search()


