define ["pi/Pi", "pi/Promise"], (P, Promise) -> class aForm extends P

   attr: -> super.concat ["uri", "target"]

   serialize: (inputs) ->
      ret = {}
      for _i in inputs
         do (i = $ _i) =>
            group = i.data("group") || "form"
            ret[group] = [] if ! ret[group]
            if i.attr("type") == "checkbox"
               if i.prop("checked")
                  ret[group].push { name: i.attr("name"), value: true, class: i.attr("class") }
               else
                  ret[group].push { name: i.attr("name"), value: false, class: i.attr("class") }
            else if i.attr("type") == "radio"
               if i.prop("checked")
                  ret[group].push { name: i.attr("name"), value: i.attr("value"), class: i.attr("class") }
            else if i.attr("processed")
               tmp = @rpc_el i, "get", [], (r) -> return r
               ret[group].push tmp
            else
               tmp = i.serializeArray()
               if i.attr "item_map"
                  item_val = i.attr "item_val"
                  ret[group].push { name: i.attr("item_map"), class: i.attr("class"), value: if item_val == "null" then null else item_val }
               
               ret[group].push tmp[0]
      return ret

   deserialize: (inputs, data) ->
      hdata = {}
      hdata[d.name] = d for d in data
      for _i in inputs
         do (i = $ _i) ->
            if i.attr("type") == "checkbox" && hdata[i.attr("name")]
               i.attr("checked", 1)
            else
               if f = hdata[i.attr "name"]
                  i.val f.value
                  i.attr "item_val", f["item_val"] if f["item_val"]

   send: (a) ->
      data = @serialize $(a.selector)
      return @ppost(@a.uri, data).then (r) =>
         @rpc @a.target, r if @a.target
         @rpc a.target, r if a.target
         return r
