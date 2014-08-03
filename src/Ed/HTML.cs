define [ "a/Ed/CodeMirror", "lib/codemirror/mode/htmlmixed/htmlmixed" ], (P) -> class EdHTML extends P
 
   init: ->
      @a.mode = "htmlmixed"
      super