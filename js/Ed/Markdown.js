// Generated by CoffeeScript 1.6.2
var __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

define(["a/Ed/CodeMirror", "lib/markdown", "lib/codemirror/mode/markdown/markdown"], function(P, aMarkdown) {
  var EdMarkdown, _ref;
  return EdMarkdown = (function(_super) {
    __extends(EdMarkdown, _super);

    function EdMarkdown() {
      _ref = EdMarkdown.__super__.constructor.apply(this, arguments);
      return _ref;
    }

    EdMarkdown.prototype.init = function() {
      this.a.mode = "markdown";
      return EdMarkdown.__super__.init.apply(this, arguments);
    };

    EdMarkdown.prototype.process = function(text) {
      this.e.html(aMarkdown.toHTML(text));
      return this.rt.pi(this.e);
    };

    return EdMarkdown;

  })(P);
});