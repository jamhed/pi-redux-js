// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["a/Pi"], function(aPi) {
  var TablePager, _ref;
  return TablePager = (function(_super) {
    __extends(TablePager, _super);

    function TablePager() {
      _ref = TablePager.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    TablePager.prototype.page = 0;

    TablePager.prototype.page_count = 0;

    TablePager.prototype.key = 'page';

    TablePager.prototype.attr = function() {
      return TablePager.__super__.attr.apply(this, arguments).concat(["bind", "name"]);
    };

    TablePager.prototype.init = function() {
      var _this = this;
      this.subscribe(this.a.bind, "data", function(rs) {
        return _this.set(rs);
      });
      this.rpc_to(this.a.bind, "get_id", [], function(key) {
        _this.key = key;
        return console.log(_this.key);
      });
      this.rpc_to(this.a.bind, "page", [this.get_page() - 1], function() {});
      return this.rpc_to(this.a.bind, "get_r", [], function(rs) {
        if (rs) {
          return _this.set(rs);
        }
      });
    };

    TablePager.prototype.set_page_to_uri = function() {
      var new_uri, old_uri, p;
      old_uri = this.rt.uri.search();
      p = this.rt.uri.search(true);
      p[this.key] = 1 + this.page;
      this.rt.uri.search(p);
      new_uri = this.rt.uri.search();
      if (old_uri !== new_uri) {
        return this.rt.set_uri_without_event(this.rt.uri);
      }
    };

    TablePager.prototype.notify = function() {
      return this.rpc_to(this.a.bind, "page", [this.page], (function() {}));
    };

    TablePager.prototype.get_page = function() {
      return this.rt.uri.search(true)[this.key];
    };

    TablePager.prototype.forward = function(n) {
      this.page = this.page + n >= this.page_count ? this.page_count - 1 : this.page + n;
      return this.set_page_to_uri();
    };

    TablePager.prototype.backward = function(n) {
      this.page = this.page - n < 0 ? 0 : this.page - n;
      return this.set_page_to_uri();
    };

    TablePager.prototype.nav = function(name) {
      switch (name) {
        case "forward":
          return this.forward(1);
        case "fastforward":
          return this.forward(10);
        case "back":
          return this.backward(1);
        case "fastback":
          return this.backward(10);
      }
    };

    TablePager.prototype.set = function(pager) {
      this.tmpl = this.rt.source(this.a.name);
      this.page = pager.page;
      this.page_count = pager.page_count;
      this.set_page_to_uri();
      this.e.html(this.tmpl({
        e: this,
        pager: {
          page: this.page + 1,
          page_count: this.page_count
        }
      }));
      return this.rt.pi(this.e);
    };

    return TablePager;

  })(aPi);
});
