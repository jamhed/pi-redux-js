require ["/conf/requirejs"], (Conf) ->
   require.config Conf

   require ["router", "lib/jquery"], (Router, JQuery) ->
      router = new Router
