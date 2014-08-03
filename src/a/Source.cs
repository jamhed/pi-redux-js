define ["a/El", "m/Source"], (aEl, mSource) -> class aSource extends aEl
   
   tmpl: null

   attr: -> super.concat ["name"]

   set: (text) ->
      @debug "a/Source", @a.name
      new mSource @a.name, text

   up: (args) ->
      tmpl = @rt.source @a.name
      @rt.pi $("<div>").append tmpl o: args, data: @data