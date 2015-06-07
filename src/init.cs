require ["../conf/requirejs"], (Conf) ->
   require.config Conf

   require ["router"], (Router, JQuery) -> router = new Router
