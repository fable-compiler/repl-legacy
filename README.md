# fable-repl

Bringing power of [Ionide](https://ionide.io) to the web browsers. Powered by [monaco](https://github.com/Microsoft/monaco-editor) - editor part of VS Code and VS Online, [Fable](https://github.com/fsprojects/Fable) - modern F# to JS compiler, [FsAutoComplete](https://github.com/fsharp/FsAutoComplete) - interface for F# Compiler Services, and [Suave](https://suave.io/) - lightweight, non-blocking web server.

# Requirements

Node and mono/.Net with F# installed


# Features
 
Many features of Ionide are already ported including:

 * AutoComplete
 * Tooltips
 * Functions overloads
 * Highlighting references
 * Finding usages
 * Going to definition
 * Peak view
 * Rename

# Planned features

 * Error highlighting
 * Better syntax highlighting 
 * Multiple file support
 * Paket support
 * Executing code

# How to run

From project folder run `npm start`. It will start both FSAC and application server. Editor will be hosted on `localhost:8888`

# How to hack

From project folder run `npm watch`. It will start both FSAC and application server. Editor will be hosted on `localhost:8888`.

Any changes to `src\editor.fsx` file will automaticly recompile it. Browser refresh required.

# Contributing and copyright

The project is hosted on GitHub where you can report issues, fork the project and submit pull requests.

The library is available under MIT license, which allows modification and redistribution for both commercial and non-commercial purposes.