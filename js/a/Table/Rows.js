// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["a/Pi"], function(aPi) {
  var aTableRows, _ref;
  return aTableRows = (function(_super) {
    __extends(aTableRows, _super);

    function aTableRows() {
      _ref = aTableRows.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    aTableRows.prototype.attr = function() {
      return aTableRows.__super__.attr.apply(this, arguments).concat(["bind", "name"]);
    };

    aTableRows.prototype.init = function() {
      var _this = this;
      this.subscribe(this.a.bind, "data", function(rs) {
        return _this.append(rs.rr);
      });
      return this.rpc_to(this.a.bind, "get_r", [], function(rs) {
        if (rs) {
          return _this.append(rs.rr);
        }
      });
    };

    aTableRows.prototype.append = function(rr) {
      var r, _i, _len;
      this.tmpl = this.rt.source(this.a.name);
      this.e.empty();
      for (_i = 0, _len = rr.length; _i < _len; _i++) {
        r = rr[_i];
        this.e.append(this.tmpl(r));
      }
      return this.rt.pi(this.e);
    };

    return aTableRows;

  })(aPi);
});
