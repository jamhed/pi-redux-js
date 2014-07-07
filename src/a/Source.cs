define ["a/El", "m/Source"], (aEl, mSource) -> class aSource extends aEl
   
   attr: () -> super.concat ["name"]

   set: (text) ->
      @debug "a/Source", @a.name
      new mSource @a.name, text