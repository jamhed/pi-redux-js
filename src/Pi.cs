define ["pi/Promise"], (Promise) -> class aPi
   
   # props (don't use {} as initializer, reference will be the same for all objects)

   a:          null
   data:       null
   e:          null
   rt:         null
   uid:        null
   
   waitTimeout: 5000
   retryTimeout: 100

   @init: ->

   attr: -> ["uid"]

   _localget: (k) ->
      try
         localStorage[k]
      catch e
         @rt.server_log "localstorage fail."
      
   _localset: (k,v) -> 
      try
         localStorage[k] = v
      catch e
         @rt.server_log "localstorage fail set."

   localSet: (k,v) -> @_localset(@uid + "/" + k, v);
   localGet: (k)   -> @_localget(@uid + "/" + k)

   globalSet: (k,v) -> @_localset(@rt.uri.fragment() + "/" + @uid + "/" + k, v)
   globalGet: (k)   -> @_localget(@rt.uri.fragment() + "/" + @uid + "/" + k)

   fragmentSet: (k,v) -> @_localset(@rt.uri.fragment() + "/" + k, v)
   fragmentGet: (k) -> @_localget(@rt.uri.fragment() + "/" + k)

   constructor: (@rt, @e, @uid) ->
      @a = {}
      @a[a] = @e.attr(a) for a in @attr()
      @data = $.extend({}, @e.data())
      @uid = @a.uid if @a.uid

      # must be
      @sub "rpc", (e, args) =>
         if ! @[args.method]
            @error "Method is not defined:", args.method, "for:", @uid
         r = @[args.method](args.args...)
         args.callback r if args.callback

      @init()

   init: ->

   debug: -> @rt.debug arguments...
   error: -> @rt.error arguments...

   post: (uri, args, callback) -> $.post uri, packet: JSON.stringify(args: args, query: @rt.uri.query(true), data: @data),
      (
         (r) => callback(r) if callback
      ), "json"

   ppost: (uri, args) ->
      p = new Promise()
      $.post uri, packet: JSON.stringify(args: args, query: @rt.uri.query(true), data: @data),
      (
         (r) => p.success(r)
      ), "json"
      return p

   # subscribe for an element events
   subscribe: (target, event, callback) -> @sub "#{target}@#{event}", (e, args) -> callback args

   # rpc: call method defined for pi-element defined by target
   rpc: (targets, args, callback) ->
      msgre = /\s*(.*?)\@(\S+)\s*/g
      seen = 0
      
      while m = msgre.exec targets
         seen = 1
         [selector, method] = [m[1], m[2]]
         @pub "#{selector}@rpc", { method: method, args: args, callback: callback }
      
      if seen == 0
         @error "Targets syntax error:", targets, @uid

   rpc_to: (target, method, args, callback) ->
      @pub "#{target}@rpc", { method: method, args: args, callback: callback }

   rpc_el: (el, method, args, callback) ->
      @msg_to el, "rpc", { method: method, args: args, callback: callback }

   sub: (ev, f) ->
      if m = /^(.*)\@(.*)$/.exec ev
         [source, ev] = [m[1], m[2]]
         if source == "server"
            @rt.sse.on ev, (_e, args) => f _e, args.args, args.caller
         else if source == "router"
            @rt.rte.on ev, (_e, args) => f _e, args.args, args.caller
         else
            $(source).on ev, (_e, args) => f _e, args.args, args.caller
      else
         @e.on ev, (_e, args) => f(_e, args.args, args.caller)

   pub: (targets, args) ->
      msgre = /\s*(.*?)\@(\S+)\s*/g
      @debug "pub()", @uid, "->", targets
      while m = msgre.exec targets
         [selector, message] = [m[1] || "[pi]", m[2]]
         return if ! message
         if selector == "parent"
            el = @e.parent().closest("[pi]")
            if el.attr "processed"
               @msg_to el, message, args
            else
               do (el, message, args) =>
                  @wait (() => el.attr "processed"), () => @msg_to el, message, args
         else if selector == "server"
            @post message, args
         else if selector == "router"
            @rt.rte.triggerHandler message, args: args, caller: @
         else if (cl = /^closest\((.*?)\)\s*(.*)$/.exec selector)
            el = @e.closest cl[1]
            el = el.find cl[2] if cl[2]
            if el.attr "processed"
               @msg_to el, message, args
            else
               do (el, message, args) =>
                  @wait (() => el.attr "processed"), () => @msg_to el, message, args
         else
            if ! $(selector).length || ! $(selector).attr "processed"
               do (selector, message, args) =>
                  @wait (() => @exists(selector)), () => @send_message selector, message, args
            else
               @send_message selector, message, args

   send_message: (selector, message, args) ->
      o = @
      $(selector).each (i, _e) ->
         e = $ _e
         if ! (e.data("events") || $._data(_e, "events"))?[message]
            o.rt.server_log "@pub() no handler on message: #{message}, target: #{selector}" + " dst: pi=" + e.attr("pi") + " src: pi=" + o.uid
         else
            o.msg_to $(e), message, args

   event: (message, args) -> @msg_to @e, message, args

   msg_to: (target, message, args) -> target.triggerHandler message, args: args, caller: @

   exists: (selector) -> $(selector).length > 0

   wait: (check, action) ->
      start = new Date().getTime()
      handler = setInterval (() =>
         status = check()
         if status
            clearInterval handler
            action()
         if (new Date().getTime() - start > @waitTimeout)
            @rt.server_log "wait() timeout", check, action
            clearInterval handler
      ), @retryTimeout

   wait_existance: (selector, action) -> @wait (() => @exists selector), action
   
   wait_ajax_done: (action) -> @wait_existance "#pi-status[ajax=0][run=0]", action

   unsub: -> @e.off arguments...

   append: (tmpl, args) -> @rt.append tmpl, args

   chain: (targets, args = @data) ->
      msgre = /^\s*(.*?)\@(\S+)\s*(.*)$/g

      while m = msgre.exec targets
         [selector, method, rest] = [m[1], m[2], m[3]]
         @pub "#{selector}@rpc",
            method: method,
            args: [args],
            callback: (r) =>
               if r["then"]
                  r.then (rs) => @chain(rest, rs)
               else
                  @chain(rest, r)