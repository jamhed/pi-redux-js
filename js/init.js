// Generated by CoffeeScript 1.10.0
require(["conf/requirejs"], function(Conf) {
  require.config(Conf);
  return require(["pi/processor"], function(Processor) {
    var processor;
    require(["lib/bootstrap/js/collapse.js"], function() {});
    require(["lib/bootstrap/js/dropdown.js"], function() {});
    return processor = new Processor;
  });
});
