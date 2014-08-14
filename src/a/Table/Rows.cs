define ["a/Pi"], (aPi) -> class aTableRows extends aPi

   attr: -> super.concat ["bind", "name"]

   init: ->
      @subscribe @a.bind, "data", (rs) => @append rs.rr

      @rpc_to @a.bind, "get_r", [], (rs) => if rs then @append rs.rr

   append: (rr) ->
      @tmpl = @rt.source @a.name
      @e.empty()
      @e.append @tmpl r for r in rr
