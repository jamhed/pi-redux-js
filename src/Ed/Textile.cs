define [ "a/Ed/CodeMirror", "lib/textile" ], (P, Textile) -> class EdTextile extends P
 
   init: ->
      @a.mode = "textile"
      super

   process: (text) ->
      @e.html Textile.convert text
      @rt.pi @e