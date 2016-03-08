require ["conf/requirejs"], (Conf) ->
	require.config Conf
	require ["pi/processor"], (Processor) ->
		require ["lib/bootstrap/js/collapse.js"], -> 
		require ["lib/bootstrap/js/dropdown.js"], -> 
		processor = new Processor