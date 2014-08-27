define ["pi/Pi"], (P) -> class aPost extends P

   attr: -> super.concat ["uri"]

   send: (p) -> return @ppost @a.uri, p