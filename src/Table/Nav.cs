define ["pi/Pi"], (aPi) -> class aTableNav extends aPi

   attr: -> super.concat ["bind", "name"]

   init: ->
      @e.click (ev) =>
         ev.preventDefault()
         @rpc_to @a.bind, "nav", [@a.name], ->
