define ["pi/Pi"], (aPi) -> class Button extends aPi

   attr: -> super.concat ["target", "click"]

   init: ->
      @rpc @a.target, @e.data(), (->) if @a.click
      @e.click (ev) =>
         ev.preventDefault()
         @rpc @a.target, @e.data(), -> 
