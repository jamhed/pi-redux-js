define ["pi/El"], (aEl) -> class aInclude extends aEl
   
   set: (text) ->
      div = $("<div style=\"display: none\">").appendTo $("body")
      div.html text
      @rt.pi div
      div.remove()
