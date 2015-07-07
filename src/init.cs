require ["conf/requirejs"], (Conf) ->
	require.config Conf
	require ["pi/processor"], (Processor) -> processor = new Processor