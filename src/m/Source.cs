define ["pi/lib/doT"], (doT) -> class mSource

   @registry: {}

   @get: (name) =>
      return if @registry[name] then @registry[name] else null

   constructor: (name, text) ->
      mSource.registry[name] = doT.template text
