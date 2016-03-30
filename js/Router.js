// Generated by CoffeeScript 1.10.0
var extend = function(child, parent) { for (var key in parent) { if (hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
  hasProp = {}.hasOwnProperty;

define(["pi/Pi", "lib/URI/URI"], function(P, URI) {
  var Router;
  return Router = (function(superClass) {
    extend(Router, superClass);

    function Router() {
      return Router.__super__.constructor.apply(this, arguments);
    }

    Router.prototype.skipHashChangeOnce = false;

    Router.prototype.uri = null;

    Router.prototype.init = function() {
      this.uri = this.parse_uri();
      return window.onhashchange = (function(_this) {
        return function(ev) {
          if (_this.skipHashChangeOnce) {
            _this.skipHashChangeOnce = false;
            return;
          }
          return _this.event("hash/change", _this.parse_uri());
        };
      })(this);
    };

    Router.prototype.set_uri = function(uri, skip) {
      if (uri == null) {
        uri = this.uri;
      }
      if (skip == null) {
        skip = false;
      }
      if (skip) {
        this.skipHashChangeOnce = true;
      }
      return window.location.hash = uri.hash() + uri.search();
    };

    Router.prototype.set_uri_text = function(text, skip) {
      if (skip == null) {
        skip = false;
      }
      if (skip) {
        this.skipHashChangeOnce = true;
      }
      return window.location = text;
    };

    Router.prototype.set_hash = function(s, skip) {
      if (skip) {
        this.skipHashChangeOnce = true;
      }
      return window.location.hash = s;
    };

    return Router;

  })(P);
});