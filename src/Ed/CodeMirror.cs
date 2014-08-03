define [
   "a/Pi",
   "lib/codemirror/lib/codemirror"
], (P, aCodeMirror) -> class EdCodeMirror extends P

   text:   ""
   editor: null
   state:  null

   attr: -> super.concat ["name", "height", "save", "change"]

   init: ->
      @sub "preview", () => @preview()
      @sub "edit", () => @edit()
      @a.name = "content" if ! @a.name
      @a.height = 20 if ! @a.height

      @text = @e.html()
      @edit()
      @e.css "visibility", "visible"

   get: -> return name: @a.name, value: @editor.getValue()

   flip: ->
      if @state == "preview"
         @edit()
      else
         @preview()
      return @state

   process: (text) -> 
      @e.html text
      @rt.pi @e

   preview: ->
      @state = "preview"
      @e.empty()
      @text = @get().value
      @process @text

   edit: ->
      @state = "edit"
      @e.empty()
      @editor = new aCodeMirror @e[0],
         value: @text,
         mode: @a.mode,
         lineNumbers: true,
         extraKeys:
            "Ctrl-S": (i) =>
               @rpc @a.save, [@data] if @a.save
               return false
      
      @editor.on "change", (ev) => @rpc @a.change, {} if @a.change

      line_h = @editor.defaultTextHeight()
      $(".CodeMirror", @e).css("height", (@a.height+1)*line_h)