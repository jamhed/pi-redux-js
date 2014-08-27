// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["pi/Pi"], function(aPi) {
  var Button, _ref;
  return Button = (function(_super) {
    __extends(Button, _super);

    function Button() {
      _ref = Button.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    Button.prototype.attr = function() {
      return Button.__super__.attr.apply(this, arguments).concat(["target", "click"]);
    };

    Button.prototype.init = function() {
      var _this = this;
      if (this.a.click) {
        this.rpc(this.a.target, this.e.data(), (function() {}));
      }
      return this.e.click(function(ev) {
        ev.preventDefault();
        return _this.rpc(_this.a.target, _this.e.data(), function() {});
      });
    };

    return Button;

  })(aPi);
});
