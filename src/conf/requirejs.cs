define
   catchError: true
   baseUrl: "/js"
   paths: 
      "pi":                "/lib/pi/js"
      "lib":               "/lib"
      "lib/jquery":        "/lib/jquery/dist/jquery.min"
      "lib/jquery-ui":     "/lib/jquery-ui/jquery-ui.min"
      "lib/doT":           "/lib/doT/doT.min"
      "lib/URI":           "/lib/uri.js/src"
      "lib/upload":        "/lib/jquery-file-upload/js/jquery.fileupload"
      "jquery.ui.widget":  "/lib/jquery-file-upload/js/vendor/jquery.ui.widget"
      "lib/bootstrap":     "/lib/bootstrap/dist/js/bootstrap"
      "lib/x-editable":    "/lib/x-editable/dist/bootstrap3-editable/js/bootstrap-editable"
   shim:
      "lib/jquery": 
         exports: "jQuery"
      "lib/jquery-ui":
         deps: ["lib/jquery"]
      "lib/bootstrap": 
         deps: ["lib/jquery"]
      "lib/x-editable": 
         deps: ["lib/bootstrap"]
