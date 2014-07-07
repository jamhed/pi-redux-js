define ["a/Pi", "lib/markdown"], (aPi, Markdown) -> class elMarkdown extends aPi

   init: ->
      @text = @e.html()
      @e.html Markdown.toHTML(@text)
      @e.show()
