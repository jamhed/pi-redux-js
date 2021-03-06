// Generated by CoffeeScript 1.10.0
var slice = [].slice;

define(["lib/URI/URI", "pi/m/Source", "pi/Logger"], function(URI, mSource, Logger) {
  var aPi;
  return aPi = (function() {
    aPi.prototype.a = null;

    aPi.prototype.data = null;

    aPi.prototype.e = null;

    aPi.prototype.uid = null;

    aPi.prototype.processor = null;

    aPi.prototype.cb_table = null;

    aPi.prototype.hn_table = null;

    aPi.prototype.waitTimeout = 5000;

    aPi.prototype.retryTimeout = 100;

    aPi.init = function() {};

    aPi.prototype.attr = function() {
      return ["uid"];
    };

    aPi.prototype._localget = function(k) {
      var e, error1;
      try {
        return localStorage[k];
      } catch (error1) {
        e = error1;
        return this.err("localstorage fail.");
      }
    };

    aPi.prototype._localset = function(k, v) {
      var e, error1;
      try {
        return localStorage[k] = v;
      } catch (error1) {
        e = error1;
        return this.err("localstorage fail set.");
      }
    };

    aPi.prototype.localSet = function(k, v) {
      return this._localset(this.uid + "/" + k, v);
    };

    aPi.prototype.localGet = function(k) {
      return this._localget(this.uid + "/" + k);
    };

    aPi.prototype._parse_uid = function(uid) {
      var m, re;
      re = /\=\'(.+?)\'.*?=(\d+)/;
      m = re.exec(uid);
      return m[1] + "[" + m[2] + "]";
    };

    aPi.prototype.self = function() {
      return this;
    };

    function aPi(processor, e1, uid1) {
      var a, base, j, len, ref;
      this.processor = processor;
      this.e = e1;
      this.uid = uid1;
      this.cb_table = {};
      this.hn_table = {};
      if (this.a == null) {
        this.a = {};
      }
      ref = this.attr();
      for (j = 0, len = ref.length; j < len; j++) {
        a = ref[j];
        if ((base = this.a)[a] == null) {
          base[a] = this.e.attr(a);
        }
      }
      this.data = $.extend({}, this.e.data());
      if (this.a.uid) {
        this.uid = this.a.uid;
      }
      this.e.data("pi", this);
      this.logger = new Logger;
      this.handler("rpc", (function(_this) {
        return function(e, args) {
          var r;
          if (!_this[args.method]) {
            return _this.err("Method is not defined:", args.method, "for:", _this.uid);
          }
          r = _this[args.method].apply(_this, args.args);
          if (args.callback) {
            return args.callback(r);
          }
        };
      })(this));
      this.handler("bound", (function(_this) {
        return function(e, args) {
          return _this;
        };
      })(this));
      this.init();
    }

    aPi.prototype.debug = function() {
      var ref;
      (ref = this.logger).debug.apply(ref, [this.constructor.name].concat(slice.call(arguments)));
      return $("#log").append(arguments, "\n");
    };

    aPi.prototype.err = function() {
      var ref;
      return (ref = this.logger).err.apply(ref, arguments);
    };

    aPi.prototype.init = function() {};

    aPi.prototype.rpc = function(targets, args, callback) {
      var m, method, msgre, ref, seen, selector;
      msgre = /\s*(.*?)\@(\S+)\s*/g;
      seen = 0;
      while (m = msgre.exec(targets)) {
        seen = 1;
        ref = [m[1], m[2]], selector = ref[0], method = ref[1];
        this.pub(selector + "@rpc", {
          method: method,
          args: args,
          callback: callback
        });
      }
      if (seen === 0) {
        return this.err("Targets syntax error:", targets, this.uid);
      }
    };

    aPi.prototype.rpc_to = function(target, method, args, callback) {
      return this.pub(target + "@rpc", {
        method: method,
        args: args,
        callback: callback
      });
    };

    aPi.prototype.rpc_el = function(el, method, args, callback) {
      return this.msg_to(el, "rpc", {
        method: method,
        args: args,
        callback: callback
      });
    };

    aPi.prototype.handler = function(ev, f) {
      return this.e.on(ev, (function(_this) {
        return function(_e, args) {
          return f(_e, args.args, args.caller);
        };
      })(this));
    };

    aPi.prototype.sub = function(ev_full, f) {
      var ev_short, m, ref, target;
      this.cb_table[ev_full] = f;
      if (m = /^(.*)\@(.*)$/.exec(ev_full)) {
        ref = [m[1], m[2]], target = ref[0], ev_short = ref[1];
        return this.rpc_to(target, "handler_table", [this.uid, ev_full, ev_short]);
      }
    };

    aPi.prototype.unsub = function(ev_full) {
      var ev_short, m, ref, target;
      delete this.cb_table[ev_full];
      if (m = /^(.*)\@(.*)$/.exec(ev_full)) {
        ref = [m[1], m[2]], target = ref[0], ev_short = ref[1];
        return this.rpc_to(target, "unhandler_table", [this.uid, ev_full, ev_short]);
      }
    };

    aPi.prototype.unhandler_table = function(sender_uid, ev_full, ev_short) {
      if (this.hn_table[ev_short]) {
        return delete this.hn_table[ev_short][sender_uid];
      }
    };

    aPi.prototype.callback = function(ev_full, _e, args) {
      return this.cb_table[ev_full](_e, args.args);
    };

    aPi.prototype.handler_table = function(sender_uid, ev_full, ev_short) {
      if (this.hn_table[ev_short] === void 0) {
        this.hn_table[ev_short] = {};
        this.hn_table[ev_short][sender_uid] = ev_full;
        return this.e.on(ev_short, (function(_this) {
          return function(_e, args) {
            return _this.handler_table_call(ev_short, _e, args);
          };
        })(this));
      } else {
        return this.hn_table[ev_short][sender_uid] = ev_full;
      }
    };

    aPi.prototype.handler_table_call = function(ev_short, _e, args) {
      var ev_full, ref, results, sender_uid;
      ref = this.hn_table[ev_short];
      results = [];
      for (sender_uid in ref) {
        ev_full = ref[sender_uid];
        results.push(this.rpc(sender_uid + "@callback", [ev_full, _e, args]));
      }
      return results;
    };

    aPi.prototype.pub = function(targets, args) {
      var cl, m, message, msgre, ref, results, selector;
      msgre = /\s*(.*?)\@(\S+)\s*/g;
      results = [];
      while (m = msgre.exec(targets)) {
        ref = [m[1] || "[pi]", m[2]], selector = ref[0], message = ref[1];
        if (!message) {
          next;
        }
        if (selector === "parent") {
          results.push(this.pub_to_parent(message, args));
        } else if ((cl = /^closest\((.*?)\)\s*(.*)$/.exec(selector))) {
          results.push(this.pub_to_closest(cl, message, args));
        } else {
          results.push(this.pub_to_selector(selector, message, args));
        }
      }
      return results;
    };

    aPi.prototype.pub_to_parent = function(message, args) {
      var el;
      el = this.e.parent().closest("[pi]");
      if (el.attr("processed")) {
        return this.msg_to(el, message, args);
      } else {
        return this.wait_to(el, message, args);
      }
    };

    aPi.prototype.pub_to_closest = function(cl, message, args) {
      var el;
      el = this.e.closest(cl[1]);
      if (cl[2]) {
        el = el.find(cl[2]);
      }
      if (el.attr("processed")) {
        return this.msg_to(el, message, args);
      } else {
        return this.wait_to(el, message, args);
      }
    };

    aPi.prototype.pub_to_selector = function(selector, message, args) {
      if (!$(selector).length || !$(selector).attr("processed")) {
        return (function(_this) {
          return function(selector, message, args) {
            return _this.wait((function() {
              return _this.exists(selector);
            }), (function() {
              return _this.send_message(selector, message, args);
            }), "pub_to_selector() " + selector + " " + message);
          };
        })(this)(selector, message, args);
      } else {
        return this.send_message(selector, message, args);
      }
    };

    aPi.prototype.wait_to = function(el, message, args) {
      return this.wait(((function(_this) {
        return function() {
          return el.attr("processed");
        };
      })(this)), ((function(_this) {
        return function() {
          return _this.msg_to(el, message, args);
        };
      })(this)), "wait_to() " + message);
    };

    aPi.prototype.send_message_to = function(el, message, args) {
      var e, ref;
      e = $(el);
      if (!((ref = e.data("events") || $._data(el, "events")) != null ? ref[message] : void 0)) {
        return this.err(("No handler for message: " + message + ", target: " + el) + " dst: pi=" + e.attr("pi") + " src: pi=" + o.uid);
      } else {
        return this.msg_to(e, message, args);
      }
    };

    aPi.prototype.send_message = function(selector, message, args) {
      return $(selector).each((function(_this) {
        return function(i, _e) {
          return _this.send_message_to(_e, message, args);
        };
      })(this));
    };

    aPi.prototype.msg_to = function(target, message, args) {
      return target.triggerHandler(message, {
        args: args,
        caller: this
      });
    };

    aPi.prototype.event = function(message, args) {
      return this.msg_to(this.e, message, args);
    };

    aPi.prototype.exists = function(selector) {
      return $(selector).length > 0;
    };

    aPi.prototype.wait = function(check, action, error) {
      var handler, start;
      if (error == null) {
        error = "";
      }
      start = new Date().getTime();
      return handler = setInterval(((function(_this) {
        return function() {
          var status;
          status = check();
          if (status) {
            clearInterval(handler);
            action();
          }
          if (new Date().getTime() - start > _this.waitTimeout) {
            _this.err("wait() timeout", error);
            return clearInterval(handler);
          }
        };
      })(this)), this.retryTimeout);
    };

    aPi.prototype.wait_existance = function(selector, action) {
      return this.wait(((function(_this) {
        return function() {
          return _this.exists(selector);
        };
      })(this)), action);
    };

    aPi.prototype.wait_ajax_done = function(action) {
      return this.wait_existance("#pi-status[ajax=0][run=0]", action);
    };

    aPi.prototype.tmpl = function(name, r) {
      var tmpl;
      tmpl = mSource.get(name);
      if (tmpl) {
        if (r) {
          return tmpl(r);
        } else {
          return tmpl;
        }
      } else {
        return this.err("No template with name " + name);
      }
    };

    aPi.prototype.process = function(e) {
      if (e == null) {
        e = this.e;
      }
      return this.processor.pi(e);
    };

    aPi.prototype.append = function(tmpl, args) {
      return this.process($("<div>").append(this.tmpl(tmpl, args)));
    };

    aPi.prototype.parse_uri = function() {
      this.uri = URI(window.location.hash.replace(/^\#\!/, "#").replace(/^(.*)\?(.*)$/, "?$2$1"));
      if (this.uri.hash() === "" || this.uri.hash() === "index") {
        this.uri.hash("content");
      }
      return this.uri;
    };

    aPi.prototype.uri_value = function(key) {
      var Uri;
      if (key == null) {
        key = "id";
      }
      Uri = this.parse_uri();
      return Uri.search(true)[key];
    };

    aPi.prototype.get_uri = function() {
      return URI(window.location);
    };

    aPi.prototype.redirect = function(uri) {
      return window.location.hash = uri;
    };

    aPi.prototype.reload = function(uri) {
      return window.location = uri;
    };

    aPi.prototype.die = function() {
      var ev_full, ref, results, v;
      ref = this.cb_table;
      results = [];
      for (ev_full in ref) {
        v = ref[ev_full];
        results.push(this.unsub(ev_full));
      }
      return results;
    };

    aPi.prototype.clear = function(scope) {
      if (scope == null) {
        scope = this.e;
      }
      $("[pi]", scope).each((function(_this) {
        return function(i, _e) {
          var e;
          e = $(_e);
          return _this.rpc_el(e, "die");
        };
      })(this));
      return scope.empty();
    };

    aPi.prototype.chain = function(targets, args) {
      var m, method, msgre, ref, rest, results, selector;
      if (args == null) {
        args = this.data;
      }
      msgre = /^\s*(.*?)\@(\S+)\s*(.*)$/g;
      results = [];
      while (m = msgre.exec(targets)) {
        ref = [m[1], m[2], m[3]], selector = ref[0], method = ref[1], rest = ref[2];
        results.push(this.pub(selector + "@rpc", {
          method: method,
          args: [args],
          callback: (function(_this) {
            return function(r) {
              if (r["then"]) {
                return r.then(function(rs) {
                  return _this.chain(rest, rs);
                });
              } else {
                return _this.chain(rest, r);
              }
            };
          })(this)
        }));
      }
      return results;
    };

    return aPi;

  })();
});
