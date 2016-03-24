define [], () -> class Util

	@list2hash: (list) ->
		ret = {}
		for elem in list
			ret[elem.name] = elem.value
		return ret
