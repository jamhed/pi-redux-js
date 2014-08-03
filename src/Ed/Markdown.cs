define [
   "a/Ed/CodeMirror",
   "lib/markdown",
   "lib/codemirror/mode/markdown/markdown"
], (P, aMarkdown) -> class EdMarkdown extends P
 
   init: ->
      @a.mode = "markdown"
      super

   process: (text) ->
      @e.html aMarkdown.toHTML text
      @rt.pi @e