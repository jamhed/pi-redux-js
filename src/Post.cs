define ["a/Pi"], (P) -> class aPost extends P

   attr: -> super.concat ["uri", "target"]

   send: (data) -> @post @a.uri, data, (r) => @rpc @a.target, r, ->
