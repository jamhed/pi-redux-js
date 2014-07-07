define ["a/Pi"], (aPi) -> class TablePager extends aPi

   page: 0
   page_count: 0
   key: 'page'

   attr: -> super.concat ["bind", "name"]

   init: ->
      @subscribe @a.bind, "data", (rs) => @set rs
      @rpc_to @a.bind, "get_id", [], (@key) => console.log @key
      @rpc_to @a.bind, "page", [@get_page()-1], ->
      @rpc_to @a.bind, "get_r", [], (rs) => if rs then @set rs

   set_page_to_uri: ->
      old_uri = @rt.uri.search()
      p = @rt.uri.search true
      p[@key] = 1 + @page
      @rt.uri.search p
      new_uri = @rt.uri.search()
      if old_uri != new_uri
         @rt.set_uri_without_event @rt.uri

   notify: ->
      @rpc_to @a.bind, "page", [@page], (->)

   get_page: ->
      return @rt.uri.search(true)[@key]
   
   forward: (n) ->
      @page = if @page + n >= @page_count then @page_count-1 else @page + n
      @set_page_to_uri()

   backward: (n) ->
      @page = if @page - n < 0 then 0 else @page - n
      @set_page_to_uri()

   nav: (name) ->
      switch name
         when "forward" then @forward(1)
         when "fastforward" then @forward(10)
         when "back" then @backward(1)
         when "fastback" then @backward(10)

   set: (pager) ->
      @tmpl = @rt.source @a.name
      
      @page = pager.page
      @page_count = pager.page_count
      @set_page_to_uri()

      @e.html @tmpl { e: this, pager: { page: @page+1, page_count: @page_count } }
      @rt.pi @e