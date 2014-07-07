# Introduction

There are several problems with client side javascript development:

* reuse of code (libraries, dependencies, etc)
* modularization (classes, modules)
* messaging
* persistance

# Concept

To associate with html elements an instances of cs classes.

# Pi.cs

To make an html element pi-compatible just add pi="ClassName" attribute. More attributes could be defined
as instantiation parameters, also data-* can be used.

Messaging is based on jquery events. Each message has this format "css-selector@event" where
css-selector matches for one or many pi-elements

## Pi methods

### subscribe(target, event, callback)

### msg(event, args)

### rpc(targets, args, callback)

### post()

## router.cs

* done event (module loading queue is empty)
* responsible for instantiation
* logging
* app-wide and server-side messaging
