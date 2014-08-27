// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["pi/Pi"], function(P) {
  var aClick, _ref;
  return aClick = (function(_super) {
    __extends(aClick, _super);

    function aClick() {
      _ref = aClick.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    aClick.prototype.attr = function() {
      return aClick.__super__.attr.apply(this, arguments).concat(["uri", "target", "chain"]);
    };

    aClick.prototype.init = function() {
      var _this = this;
      return this.e.click(function(ev) {
        ev.preventDefault();
        if (_this.a.chain) {
          return _this.chain(_this.a.chain);
        }
        if (_this.a.uri) {
          return _this.send();
        } else {
          return _this.click();
        }
      });
    };

    aClick.prototype.click = function() {
      return this.rpc(this.a.target, [this.data]);
    };

    aClick.prototype.send = function() {
      var _this = this;
      return this.post(this.a.uri, this.data, function(r) {
        return _this.rpc(_this.a.target, [r]);
      });
    };

    aClick.prototype.chain = function(targets, args) {
      var m, method, msgre, rest, selector, _ref1, _results,
        _this = this;
      if (args == null) {
        args = this.data;
      }
      msgre = /^\s*(.*?)\@(\S+)\s*(.*)$/g;
      _results = [];
      while (m = msgre.exec(targets)) {
        _ref1 = [m[1], m[2], m[3]], selector = _ref1[0], method = _ref1[1], rest = _ref1[2];
        _results.push(this.pub("" + selector + "@rpc", {
          method: method,
          args: [args],
          callback: function(r) {
            if (r["then"]) {
              return r.then(function(rs) {
                return _this.chain(rest, rs);
              });
            } else {
              return _this.chain(rest, r);
            }
          }
        }));
      }
      return _results;
    };

    return aClick;

  })(P);
});
