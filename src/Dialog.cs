define ["a/Pi", "lib/jquery-ui"], (P) -> class aDialog extends P

   init: ->
      @e.dialog
         close: (ev, ui) => @e.remove()
         draggable: true
         width: @data.width || 500
         height: @data.height || "auto"
   
   close: ->
      @e.remove()