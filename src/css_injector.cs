define [], () -> class CSSInjector

	css: null

	constructor: ->
		@css     = {}

	injectCSS: (uri) ->
		if @css[uri] then return else @css[uri] = 1
		link = document.createElement "link"
		link.type = "text/css"
		link.rel = "stylesheet"
		link.href = uri
		document.getElementsByTagName("head")[0].appendChild(link)