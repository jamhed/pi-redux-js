// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["a/El"], function(P) {
  var aContent, _ref;
  return aContent = (function(_super) {
    __extends(aContent, _super);

    function aContent() {
      _ref = aContent.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    aContent.prototype.cache = null;

    aContent.prototype.ee = null;

    aContent.prototype.init = function() {
      var _this = this;
      this.sub("router@hash/change", function(ev, args) {
        _this.active(args.uri);
        return _this.load();
      });
      return this.load();
    };

    aContent.prototype.active = function(uri) {
      $(".navbar li").removeClass("active");
      return $(".navbar a[href=\"" + (uri.hash()) + "\"]").parent().addClass("active");
    };

    aContent.prototype.set = function(text) {
      this.text = text;
      this.e.empty();
      this.ee = $("<div>").html(this.text).appendTo(this.e);
      return this.rt.pi(this.ee);
    };

    aContent.prototype.load = function() {
      this.uri = this.rt.uri;
      this.active(this.uri);
      return aContent.__super__.load.apply(this, arguments);
    };

    return aContent;

  })(P);
});
