// Generated by CoffeeScript 1.10.0
var extend = function(child, parent) { for (var key in parent) { if (hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
  hasProp = {}.hasOwnProperty;

define(["pi/El"], function(P) {
  var aContent;
  return aContent = (function(superClass) {
    extend(aContent, superClass);

    function aContent() {
      return aContent.__super__.constructor.apply(this, arguments);
    }

    aContent.prototype.attr = function() {
      return aContent.__super__.attr.apply(this, arguments).concat(["router"]);
    };

    aContent.prototype.cache = null;

    aContent.prototype.ee = null;

    aContent.prototype.init = function() {
      this.sub(this.a.router + "@hash/change", (function(_this) {
        return function(ev, args) {
          return _this.load(args);
        };
      })(this));
      return this.load();
    };

    aContent.prototype.active = function(uri) {
      $(".navbar li").removeClass("active");
      return $(".navbar a[href=\"" + (uri.hash()) + "\"]").parent().addClass("active");
    };

    aContent.prototype.set = function(text) {
      this.text = text;
      this.clear();
      this.ee = $("<div>").html(this.text).appendTo(this.e);
      return this.process(this.ee);
    };

    aContent.prototype.load = function(uri) {
      if (uri == null) {
        uri = this.parse_uri();
      }
      this.active(uri);
      return aContent.__super__.load.apply(this, arguments);
    };

    return aContent;

  })(P);
});
