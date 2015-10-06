define({
   catchError: true,
   baseUrl: "/js",
   paths: {
      "pi":                "/pi",
      "pi/lib/jquery":     "/lib/jquery/dist/jquery.min",
      "pi/lib/jquery-ui":  "/lib/jquery-ui/jquery-ui.min",
      "pi/lib/doT":        "/lib/doT/doT.min",
      "pi/lib/URI":        "/lib/uri.js/src",
      "pi/lib/upload":     "/lib/jquery-file-upload/js/jquery.fileupload",
      "jquery.ui.widget":  "/lib/jquery-file-upload/js/vendor/jquery.ui.widget",
      "lib/bootstrap":     "/lib/bootstrap/dist/js/bootstrap",
      "lib/x-editable":    "/lib/x-editable/dist/bootstrap3-editable/js/bootstrap-editable"
   },
   shim: {
      "pi/lib/jquery": {
         exports: "jQuery"
      },
      "pi/lib/jquery-ui": {
         deps: ["pi/lib/jquery"]
      },
      "lib/bootstrap": {
         deps: ["pi/lib/jquery"]
      },
      "lib/x-editable": {
         deps: ["lib/bootstrap"]
      }
   }
});
