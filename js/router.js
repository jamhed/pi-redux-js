// Generated by CoffeeScript 1.6.2
var __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; },
  __slice = [].slice;

define(["lib/jquery", "lib/doT", "lib/URI/URI", "m/Source"], function(jQuery, doT, URI, mSource) {
  var Router;
  return Router = (function() {
    Router.prototype.skipHashChangeOnce = false;

    Router.prototype["class"] = null;

    Router.prototype.count = null;

    Router.prototype.css = null;

    Router.prototype.uri = null;

    Router.prototype.sse = null;

    Router.prototype.rte = null;

    Router.prototype.status_e = null;

    Router.prototype.pi_run = 0;

    Router.prototype.pi_ajax = 0;

    Router.prototype.debug = function() {};

    Router.prototype.error = function() {};

    Router.prototype.server_log = function() {
      var err, img;
      err = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
      img = new Image();
      img.src = "/jserr?msg=" + encodeURIComponent(err.join(" "));
      return this.error(err.join(" "));
    };

    Router.prototype.status = function(pi_run, pi_ajax) {
      this.pi_run = pi_run;
      this.pi_ajax = pi_ajax;
      this.debug("pi status: " + this.pi_ajax + " " + this.pi_run);
      this.status_e.attr("run", this.pi_run);
      return this.status_e.attr("ajax", this.pi_ajax);
    };

    function Router() {
      this.pi = __bind(this.pi, this);
      var _this = this;
      this["class"] = {};
      this.count = {};
      this.css = {};
      this.uri = this.parse_uri();
      if (window.console && Function.prototype.bind && (typeof console.log === "object" || typeof console.log === "function")) {
        this.debug = Function.prototype.bind.call(console.log, console);
        this.error = Function.prototype.bind.call(console.error, console);
      }
      window.onhashchange = function(ev) {
        if (_this.skipHashChangeOnce) {
          _this.skipHashChangeOnce = false;
          return;
        }
        return _this.rte.triggerHandler("hash/change", {
          args: {
            ev: ev,
            uri: _this.parse_uri()
          }
        });
      };
      $(document).ajaxSend(function(ev, xhr, r) {
        return _this.status(_this.pi_run, _this.pi_ajax + 1);
      });
      $(document).ajaxComplete(function(ev, xhr, s) {
        var e, nonjson, r, uri;
        _this.status(_this.pi_run, _this.pi_ajax - 1);
        uri = URI(s.url);
        try {
          r = JSON.parse(xhr.responseText);
        } catch (_error) {
          e = _error;
          r = xhr.responseText;
          nonjson = true;
        }
        return _this.sse.triggerHandler(uri.filename(), {
          args: {
            uri: uri,
            ev: ev,
            xhr: xhr,
            s: s,
            r: r,
            nonjson: nonjson
          }
        });
      });
      window.onerror = function(msg, url, line) {
        return _this.server_log("onerror()", url, msg, line);
      };
      requirejs.onError = function(err) {
        return _this.server_log("requirejs()", err.requireType, err.requireModules);
      };
      this.sse = $("<div>").attr("id", "pi-sse");
      this.rte = $("<div>").attr("id", "pi-rte");
      this.status_e = $("<div>").attr("id", "pi-status");
      $("body").append(this.status_e);
      $("body").append(this.sse);
      $("body").append(this.rte);
      this.pi(document);
    }

    Router.prototype.source = function(name, r) {
      var tmpl;
      tmpl = mSource.get(name);
      if (tmpl) {
        if (r) {
          return tmpl(r);
        } else {
          return tmpl;
        }
      } else {
        return this.server_log("No template with name " + name);
      }
    };

    Router.prototype.set_uri = function(uri) {
      if (uri == null) {
        uri = this.uri;
      }
      return window.location.hash = uri.hash() + uri.search();
    };

    Router.prototype.set_hash = function(s) {
      return window.location.hash = s;
    };

    Router.prototype.set_uri_without_event = function() {
      this.skipHashChangeOnce = true;
      return window.location.hash = this.uri.hash() + this.uri.search();
    };

    Router.prototype.parse_uri = function() {
      this.uri = URI(window.location.hash.replace(/^\#\!/, "#").replace(/^(.*)\?(.*)$/, "?$2$1"));
      if (this.uri.hash() === "" || this.uri.hash() === "index") {
        this.uri.hash("content");
      }
      return this.uri;
    };

    Router.prototype.pi_bind = function(name, e) {
      if (e.attr("processed")) {
        return this.debug("pi already bound:", name, e);
      }
      this.count[name] = this.count[name] ? this.count[name] + 1 : 1;
      e.attr("processed", this.count[name]);
      return new this["class"][name](this, e, "[pi='" + name + "'][processed=" + this.count[name] + "]");
    };

    Router.prototype.pi = function(context, callback) {
      var s, seen, stack, unames, ustack, _fn, _i, _len,
        _this = this;
      this.status(this.pi_run + 1, this.pi_ajax);
      stack = [];
      $("[pi]", context).each(function(i, _e) {
        var e, name;
        e = $(_e);
        name = e.attr("pi");
        if (_this["class"][name]) {
          return _this.pi_bind(name, e);
        } else {
          return stack.push({
            name: name,
            e: e
          });
        }
      });
      seen = {};
      ustack = [];
      _fn = function(s) {
        if (!seen[s.name]) {
          ustack.push(s);
          return seen[s.name] = 1;
        }
      };
      for (_i = 0, _len = stack.length; _i < _len; _i++) {
        s = stack[_i];
        _fn(s);
      }
      unames = (function() {
        var _j, _len1, _results;
        _results = [];
        for (_j = 0, _len1 = ustack.length; _j < _len1; _j++) {
          s = ustack[_j];
          _results.push(s.name);
        }
        return _results;
      })();
      return require(unames, function() {
        var Name, Names;
        Names = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
        while (s = ustack.shift()) {
          Name = Names.shift();
          if (!_this["class"][name]) {
            _this.debug("pi stacked", s.name);
            _this["class"][s.name] = Name;
            Name.init(_this);
          }
        }
        while (s = stack.shift()) {
          _this.pi_bind(s.name, s.e);
        }
        _this.status(_this.pi_run - 1, _this.pi_ajax);
        if (callback) {
          return callback();
        }
      });
    };

    Router.prototype.injectCSS = function(uri) {
      var link;
      if (this.css[uri]) {
        return;
      } else {
        this.css[uri] = 1;
      }
      link = document.createElement("link");
      link.type = "text/css";
      link.rel = "stylesheet";
      link.href = uri;
      return document.getElementsByTagName("head")[0].appendChild(link);
    };

    Router.prototype.append = function(tmpl, args) {
      return this.pi($("<div>").append(mSource.get(tmpl)(args)));
    };

    return Router;

  })();
});
