define ["pi/lib/jquery", "pi/lib/doT", "pi/lib/URI/URI", "pi/m/Source"], (jQuery, doT, URI, mSource) -> class Router

   skipHashChangeOnce: false
   class: null
   count: null
   css: null
   uri: null
   sse: null   # server-side events proxy
   rte: null   # router-side events proxy
   
   status_e: null # status
   pi_run: 0
   pi_ajax: 0

   # logging
   
   debug: ->
   error: ->

   server_log: (err...) ->
      img = new Image()
      img.src = "/jserr?msg=" + encodeURIComponent( err.join(" ") )
      @error err.join " "

   status: (@pi_run, @pi_ajax) ->
      @debug "pi status: #{@pi_ajax} #{@pi_run}"
      @status_e.attr("run", @pi_run)
      @status_e.attr("ajax", @pi_ajax)

   constructor: () ->
      @class   = {}
      @count   = {}
      @css     = {}
      @uri     = @parse_uri()

      if window.console && Function.prototype.bind && (typeof console.log == "object" || typeof console.log == "function")
         @debug = Function.prototype.bind.call(console.log, console)
         @error = Function.prototype.bind.call(console.error, console)

      window.onhashchange = (ev) =>
         if @skipHashChangeOnce
            @skipHashChangeOnce = false
            return
            
         @rte.triggerHandler "hash/change", args: ev: ev, uri: @parse_uri()
      
      $(document).ajaxSend (ev, xhr, r) =>
         @status @pi_run, @pi_ajax+1

      $(document).ajaxComplete (ev, xhr, s) =>
         @status @pi_run, @pi_ajax-1

         uri = URI s.url
         try
            r = JSON.parse xhr.responseText
         catch e
            r = xhr.responseText
            nonjson = true
         
         @sse.triggerHandler uri.filename(), args: uri: uri, ev: ev, xhr: xhr, s: s, r: r, nonjson: nonjson

      window.onerror = (msg, url, line) => @server_log "onerror()", url, msg, line

      requirejs.onError = (err) =>
         @status @pi_run-1, @pi_ajax if @pi_run > 0
         @server_log "requirejs()", err.requireType, err.requireModules, err.message

      @sse = $("<div>").attr("id", "pi-sse")
      @rte = $("<div>").attr("id", "pi-rte")
      @status_e = $("<div>").attr("id", "pi-status")
      $("body").append(@status_e)
      $("body").append(@sse)
      $("body").append(@rte)

      @pi document
   
   source: (name, r) -> 
      tmpl = mSource.get name
      if tmpl
         return if r then tmpl r else tmpl
      else
         @server_log "No template with name #{name}"

   set_uri: (uri = @uri) ->
      window.location.hash = uri.hash() + uri.search()

   set_hash: (s) -> window.location.hash = s

   set_uri_without_event: ->
      @skipHashChangeOnce = true
      window.location.hash = @uri.hash() + @uri.search()

   parse_uri: ->
      @uri = URI window.location.hash.replace(/^\#\!/, "#").replace(/^(.*)\?(.*)$/,"?$2$1")
      @uri.hash("content") if @uri.hash() == "" or @uri.hash() == "index"
      return @uri

   pi_bind: (name, e) ->
      if e.attr "processed"
         return @debug "pi already bound:", name, e
      @count[name] = if @count[name] then @count[name] + 1 else 1
      e.attr "processed", @count[name]
      new @class[name] @, e, "[pi='#{name}'][processed=#{@count[name]}]" # name + "/" + @count[name]

   pi: (context, callback) =>
      @status @pi_run+1, @pi_ajax

      stack = []
      $("[pi]", context).each (i,_e) =>
         e = $ _e
         name = e.attr("pi")

         if @class[name]
            @pi_bind name, e
         else
            stack.push name: name, e: e

      seen = {}
      ustack = []
      for s in stack
         do (s) ->
            if (! seen[s.name])
               ustack.push s 
               seen[s.name] = 1
            
      unames = (s.name for s in ustack)

      require unames, (Names...) =>
         while s = ustack.shift()
            Name = Names.shift()
            if ! @class[name]
               @debug "pi stacked", s.name
               @class[s.name] = Name
               Name.init @

         while s = stack.shift()
            @pi_bind s.name, s.e
         
         @status @pi_run-1, @pi_ajax

         callback() if callback

   injectCSS: (uri) ->
      if @css[uri] then return else @css[uri] = 1
      link = document.createElement "link"
      link.type = "text/css"
      link.rel = "stylesheet"
      link.href = uri
      document.getElementsByTagName("head")[0].appendChild(link)

   append: (tmpl, args) ->
      @pi $("<div>").append mSource.get(tmpl) args
