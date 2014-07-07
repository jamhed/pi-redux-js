require ["conf/requirejs"], (Conf) ->
   require.config Conf

   require ["router"], (Router) ->
      router = new Router