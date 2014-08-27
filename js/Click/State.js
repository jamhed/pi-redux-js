// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["pi/Pi"], function(P) {
  var aClickState, _ref;
  return aClickState = (function(_super) {
    __extends(aClickState, _super);

    function aClickState() {
      _ref = aClickState.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    aClickState.prototype.attr = function() {
      return aClickState.__super__.attr.apply(this, arguments).concat(["target"]);
    };

    aClickState.prototype.init = function() {
      var _this = this;
      return this.e.click(function(ev) {
        ev.preventDefault();
        return _this.click();
      });
    };

    aClickState.prototype.click = function() {
      var _this = this;
      return this.rpc(this.a.target, [this.data], function(p) {
        return p.then(function(r) {
          return _this["default"]();
        });
      });
    };

    aClickState.prototype["default"] = function() {
      return this.e.removeClass("btn-danger").addClass("btn-default");
    };

    aClickState.prototype.warn = function() {
      return this.e.removeClass("btn-default").addClass("btn-danger");
    };

    return aClickState;

  })(P);
});