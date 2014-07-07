# PI library

Processing Instructions. Make html elements live.

* package management with bower
* modularization with AMD and require.js
* coffee-script classes
* messages to server and to other pi-elements

# Design

Allows to bind any html-element to an instance of a Pi-derived class. The content
of html element could be used as a template or whatever. Pi-elements can
send messages to each other (and also to the server), and define message handlers.

# How to install

```
$ bower install pi
```

Then point your web-server to serve files from sample folder as root.
There is a complete sample project based on Perl Dancer: https://github.com/jamhed/pi

# How to configure requirejs

PI relays on requirejs for module loading. Requirejs should be configured. The configuration
sample file is located in conf/requirejs.js file. There we define relative paths mapping to real paths
mapping and requirejs "shims" -- workarounds for non-AMD modules.

Example:

```
paths: {
   "lib/URI":        "/lib/uri.js/src",
   "lib/jquery":     "/lib/jquery/dist/jquery.min",
   "lib/jquery-ui":  "/lib/jquery-ui/ui/minified/jquery-ui.min",
   "lib/doT":        "/lib/doT/doT",
   "lib/bootstrap":  "/lib/bootstrap/dist/js/bootstrap.min",
   "lib/markdown":   "/lib/markdown/lib/markdown"
},
```

Shims for non-AMD modules force requirejs to assign a variable with module for later use and dependency
for proper loading sequence:

```
shim: {
   "lib/jquery": {
      exports: "jQuery"
   },
   "lib/jquery-ui": {
      deps: ["lib/jquery"]
   },
   "lib/bootstrap": {
      deps: ["lib/jquery"]
   },
   "lib/markdown": {
      exports: "markdown"
   },
}
```

# Sample site

There is a sample folder with basic file layout. You can setup it with:

```
$ bower_components/pi/sample-setup.sh
```

Then just point a web-server to serve pages from sample as root folder.

# How to plug in

The router.cs is the main processor, you need to load it with requirejs:

```
<script type="text/javascript" src="/lib/requirejs/require.js" data-main="/lib/pi/js/init.js"></script>
```

init.js (a compiled version of init.cs) just loads our requirejs conf and instantiates the router.cs:

```
require ["../conf/requirejs"], (Conf) ->
   require.config Conf

   require ["router", "lib/jquery"], (Router, JQuery) ->
      router = new Router
```

# How to use

Every pi-element should be subclassed from Pi class. Let define a button:

```
define ["a/Pi"], (P) -> class Button extends P

   attr: -> super.concat ["target", "click"]

   init: ->
      @rpc @a.target, @e.data(), (->) if @a.click
      @e.click (ev) =>
         ev.preventDefault()
         @rpc @a.target, @e.data(), -> 

```

This allows us to make a live button with:

```
<div pi="Button" target="#target" class="btn">Button</div>
```

It defines the target attribute for html-element to be read and stored as @a.target,
and sets up a click handler for element.

Here on click we just send a message to another pi-element identified by jquery-compatible
css-selector target.

See more examples in src folder.
