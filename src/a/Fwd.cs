define ["a/Pi"], (P) -> class aFwd extends P

   attr: -> super.concat ["uri"]

   go: -> window.location = @a.uri