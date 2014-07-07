# Introduction

There are several problems with client side javascript development:

* reuse of code (libraries, dependencies, etc)
* modularization (classes, modules)
* messaging
* persistence

# Concept

To associate with html elements an instances of cs classes.

# Pi.cs

To make an html element pi-compatible just add pi="ClassName" attribute. More attributes could be defined
as instantiation parameters, also data-* can be used.

Messaging is based on jquery events. Each message has this format "css-selector@event" where
css-selector matches for one or many pi-elements, and arbitrary json object could be passed
as argument.

## Pi methods

### subscribe(target, event, callback)

Subscribes this element for event "event" of elements selected by selector target. Callback will
be called on event.

### msg(event, args)

Generate event on this element, triggering all subscribed callbacks.

### rpc(targets, args, callback)

Targets are "css-selector@method", and this method just calls "method" for selected targets,
with args as argument. On completion callback will be called with return value as argument.

### post(uri, args, callback)

Makes a post request to server with json-serialized body in following format:

packet: { args: ... , query: ... , data: ... }

Where args are passed args, query is parsed query part of current uri, and data is html elements data-*.


## router.cs

* done event (module loading queue is empty)
* responsible for instantiation
* logging
* app-wide and server-side messaging
