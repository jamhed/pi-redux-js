define ["pi/Pi"], (P) -> class aClick extends P

   attr: -> super.concat ["uri", "target", "chain"]

   init: ->
      @e.click (ev) =>
         ev.preventDefault()
   
         if @a.chain
            return @chain @a.chain

         if @a.uri
            @send()
         else
            @click()

   click: -> @rpc @a.target, [@data]

   send: -> @post @a.uri, @data, (r) => @rpc @a.target, [r]
