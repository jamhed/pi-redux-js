# PI library

Processing Instructions. Make html elements live.

* package management with bower
* modularization with AMD and require.js
* coffee-script classes
* messages to server and to other pi-elements

# How to install

```
$ bower install pi
```

Then point your web-server to serve files from sample folder as root.

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

# Sample

There is a sample folder with basic file layout. You can setup it with:

```
$ bower_components/pi/sample-setup.sh
```

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



