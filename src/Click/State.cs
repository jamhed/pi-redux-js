define ["pi/Pi"], (P) -> class aClickState extends P

   attr: -> super.concat ["target"]

   init: ->
      @e.click (ev) =>
         ev.preventDefault()
         @click()

   click: ->
      @rpc @a.target, [@data], (p) => p.then (r) => @default()

   default: ->
      @e.removeClass("btn-danger").addClass("btn-default")

   warn: ->
      @e.removeClass("btn-default").addClass("btn-danger")
