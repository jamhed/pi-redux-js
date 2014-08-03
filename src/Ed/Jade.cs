define [ "a/Ed/CodeMirror", "lib/codemirror/mode/jade/jade" ], (P) -> class EdJade extends P
 
   init: ->
      @a.mode = "jade"
      super

   process: (text) ->
      @post "r/compile", text: text, type: "Jade", (r) =>
         @e.html r.compiled
         @rt.pi @e