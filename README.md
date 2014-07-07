h1. PI redux

PI -- Processing Instructions. Get html-elements live.

h1. How to install

Just use bower.

h1. How to configure requirejs

PI relays on requirejs for module loading. Requirejs should be configured. The configuration
sample file is located in conf/requirejs.js file. There we define relative paths mapping to real paths
mapping and requirejs "shims" -- workarounds for non-AMD modules.

Example:

   paths: {
      "lib/URI":        "lib/uri.js/src",
      "lib/jquery":     "lib/jquery/dist/jquery.min",
      "lib/jquery-ui":  "lib/jquery-ui/ui/minified/jquery-ui.min",
      "lib/doT":        "lib/doT/doT",
      "lib/bootstrap":  "lib/bootstrap/dist/js/bootstrap.min",
      "lib/markdown":   "lib/markdown/lib/markdown"
   },

Shims for non-AMD modules force requirejs to assign a variable with module for later use and dependency
for proper loading sequence:
   
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