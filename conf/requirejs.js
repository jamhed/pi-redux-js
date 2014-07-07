define({
   catchError: true,
   paths: {
      "lib/URI":        "/lib/uri.js/src",
      "lib/jquery":     "/lib/jquery/dist/jquery.min",
      "lib/jquery-ui":  "/lib/jquery-ui/ui/minified/jquery-ui.min",
      "lib/doT":        "/lib/doT/doT",
      "lib/bootstrap":  "/lib/bootstrap/dist/js/bootstrap.min",
      "lib/markdown":   "/lib/markdown/lib/markdown"
   },
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
});
