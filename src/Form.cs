define ["pi/Pi"], (aPi) -> class aForm extends aPi

	attr: -> super.concat ["el", "target"]

	serialize: (inputs = $(@a.el)) ->
		ret = {}
		for _i in inputs
			do (i = $ _i) ->
				group = i.data("group") || "form"
				ret[group] = [] if ! ret[group]
				if i.attr("type") == "checkbox"
					if i.prop("checked")
						ret[group].push { name: i.attr("name"), value: true }
					else
						ret[group].push { name: i.attr("name"), value: false }
				else if i.attr("type") == "radio"
					if i.prop("checked")
						ret[group].push { name: i.attr("name"), value: i.attr("value"), class: i.attr("class") }
				else
					tmp = name: i.attr("id"), value: i.val()
					if i.attr "item_map"
						item_val = i.attr "item_val"
						ret[group].push { name: i.attr("item_map"), class: i.attr("class"), value: if item_val == "null" then null else item_val }
					
					ret[group].push tmp
		return ret

	deserialize: (data) ->
		inputs = $(@a.el)
		hdata = data
		for _i in inputs
			do (i = $ _i) ->
				if i.attr("type") == "checkbox" && hdata[i.attr("id")]
					i.attr("checked", 1)
				else
					if f = hdata[i.attr "id"]
						i.val f

	reset: (inputs = $(@a.el)) ->
		for _i in inputs
			do (i = $ _i) ->
				if i.attr("type") == "checkbox"
					i.removeAttr "checked"
				else
					if i.attr "item_map"
						i.attr "item_val", ""
					i.val "" 

	init: ->
		super
		@e.click (ev) => @onClick ev

	save: ->
		data = @serialize()
		@rpc @a.target, [data.form], ->
			
	onClick: (ev) ->
		ev.preventDefault()
		data = @serialize()
		@rpc @a.target, [data.form], ->