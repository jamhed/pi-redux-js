require ["conf/requirejs"], (Conf) ->
	require.config Conf
	require ["pi/processor"], (Processor) ->
		require ["lib/bootstrap/js/collapse.js"], (Collapse) -> console.log Collapse
		processor = new Processor