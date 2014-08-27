define ["pi/Pi"], (P) -> class aClick extends P

   attr: -> super.concat ["target"]

   init: ->
      @e.click (ev) =>
         ev.preventDefault()
         @click()

   click: -> @rpc @a.target, [@data]
