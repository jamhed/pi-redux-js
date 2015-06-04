define ["pi/Promise", "pi/lib/URI/URI"], (Promise, URI) -> class aPi
   
   # props (don't use {} as initializer, reference will be the same for all objects)

   a:          null
   data:       null
   e:          null
   rt:         null
   uid:        null

   cb_table:   null
   hn_table:   null
   
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

   constructor: (@rt, @e, @uid) ->
      @cb_table = {}
      @hn_table = {}
      @a = {}
      @a[a] = @e.attr(a) for a in @attr()
      @data = $.extend({}, @e.data())
      @uid = @a.uid if @a.uid

      # must be

      @handler "rpc", (e, args) =>
         if ! @[args.method]
            return @error "Method is not defined:", args.method, "for:", @uid
         r = @[args.method](args.args...)
         args.callback r if args.callback

      @handler "bound", (e, args) => @

      @init()

   init: ->

   debug: ->
      $("#log").append arguments, "\n"
      @rt.debug arguments...
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

   handler: (ev, f) ->
      @e.on ev, (_e, args) => f(_e, args.args, args.caller)

   sub: (ev_full, f) ->
      @cb_table[ev_full] = f
      if m = /^(.*)\@(.*)$/.exec ev_full
         [target, ev_short] = [m[1], m[2]]
         @rpc_to target, "handler_table", [@uid, ev_full, ev_short]

   unsub: (ev_full) ->
      delete @cb_table[ev_full]
      if m = /^(.*)\@(.*)$/.exec ev_full
         [target, ev_short] = [m[1], m[2]]
         @rpc_to target, "unhandler_table", [@uid, ev_full, ev_short]

   unhandler_table: (sender_uid, ev_full, ev_short) ->
      # @debug "unhandler_table", sender_uid, ev_full, ev_short
      if @hn_table[ev_short]
         delete @hn_table[ev_short][sender_uid]

   callback: (ev_full, _e, args) ->
      # @debug "callback", ev_full, _e, args
      @cb_table[ev_full](_e, args.args)

   handler_table: (sender_uid, ev_full, ev_short) ->
      # @debug "handler_table", ev_full, ev_short
      if @hn_table[ev_short] == undefined
         @hn_table[ev_short] = {}
         @hn_table[ev_short][sender_uid] = ev_full
         @e.on ev_short, (_e, args) => @handler_table_call ev_short, _e, args
      else
         @hn_table[ev_short][sender_uid] = ev_full

   handler_table_call: (ev_short, _e, args) ->
      for sender_uid, ev_full of @hn_table[ev_short]
         @rpc "#{sender_uid}@callback", [ev_full, _e, args]

   pub: (targets, args) ->
      msgre = /\s*(.*?)\@(\S+)\s*/g
      @debug "pub()", @uid, "->", targets, args
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

   append: (tmpl, args) -> @rt.append tmpl, args

   parse_uri: ->
      @uri = URI window.location.hash.replace(/^\#\!/, "#").replace(/^(.*)\?(.*)$/,"?$2$1")
      @uri.hash("content") if @uri.hash() == "" or @uri.hash() == "index"
      return @uri

   die: ->
      for ev_full,v of @cb_table
         @debug "DEAD", @uid, ev_full
         @unsub ev_full

   empty: (scope = @e) ->
      # time to die
      $("[pi]", scope).each (i,_e) =>
         e = $ _e
         @rpc_el e, "die"
      scope.empty()


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
