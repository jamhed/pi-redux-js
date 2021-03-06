// Generated by CoffeeScript 1.10.0
var extend = function(child, parent) { for (var key in parent) { if (hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
  hasProp = {}.hasOwnProperty;

define(["pi/El", "pi/m/Source"], function(aEl, mSource) {
  var aSource;
  return aSource = (function(superClass) {
    extend(aSource, superClass);

    function aSource() {
      return aSource.__super__.constructor.apply(this, arguments);
    }

    aSource.prototype.attr = function() {
      return aSource.__super__.attr.apply(this, arguments).concat(["name"]);
    };

    aSource.prototype.set = function(text) {
      this.debug(this.a.name);
      return new mSource(this.a.name, text);
    };

    aSource.prototype.up = function(args) {
      var tmpl;
      tmpl = this.tmpl(this.a.name);
      return this.append(tmpl({
        o: args,
        data: this.data
      }));
    };

    return aSource;

  })(aEl);
});
