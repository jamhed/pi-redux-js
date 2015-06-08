# Core concepts

# Processing Instructions

After the index page loads also loads and instantiates main processing module named
router.js, responsible for html elements processing. In order element to be processed
it should have the attribute pi with corresponded javascript module name. After
processing the element instance of javascript module is created and associated
with this html element.

# Javascript module dependencies

All javascript modules load by [require.js](http://requirejs.org) library. Require.js also manages
module dependencies. Each module specify its dependencies by define call:
```
define ["pi/Promise", "pi/lib/URI/URI"], (Promise, URI) -> class aPi
```
# Templates

Templates are text chunks of selected templating system. Default (and one actually implemented)
is based on [doT](https://github.com/olado/doT) templating, chosen for spead. doT templates are just HTML with {{ INST }} instructions.

Templates are either files served from server or named chunks in source html files. Later
templates are referenced by the name specified or the file name.

Here is how to define a template in HTML:

```
<script pi="pi/Source" name="button/login" type="text/x-dot">
<button type="submit" class="btn btn-default" pi="Login" dialog="dialog/login">Login</button>
</script>
```

# Messages

Messaging is done by using jQuery events (.on() and .trigger() methods namely using
following format:
```
selector@message
```
where selector is standard jQuery html selector, and message is a text string.

# Pub/sub mechanism

Each element can subscribe to events on other elements by callin @sub method:
```
@sub "#bullet@conv/status/part", (e,args) => @clear()
```

# Call a function on element

```
@rpc "selector@method", [arg1, ..., argN], callback
```

# Postbacks

