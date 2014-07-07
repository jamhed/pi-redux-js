define ["a/El"], (aEl) -> class aTemplate extends aEl

   attr: -> super.concat ["name", "replace"]

   set: (r) ->
      tmpl = @rt.source @a.name
      if tmpl
         @real_set(r, tmpl)
      else
         @wait (() => return @rt.source @a.name), () => 
            tmpl = @rt.source @a.name
            @real_set(r, tmpl)

   real_set: (r, tmpl) ->
      if @a.replace == ""
         @ee = $(tmpl r: r, o: @)
         @e.replaceWith @ee
         @rt.pi @ee
      else
         @e.html tmpl r: r, o: @
         @rt.pi @e
