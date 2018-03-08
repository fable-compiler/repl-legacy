(*
Interop Examples V 1.0 by Gary Bilkus March 2018

This app is intended to demonstrate the various ways of interoperating with existing javascript.
It goes progressively from a quick and dirty approach in which everything is untyped and you access the global namespace directly, through to use of modules and typed interfaces.

The intention is to consolidate, in one reasonably small app, working examples of all the techniques available,from the perspective of someone who knows how to do things in javascript and wants to translate that into F#.
The fable.io website does contain most of what's in here, but not necessarily in a way which is easy for a newbie to find.

I am making no attempt to teach either javascript or F#.
The index.html page which is the main page of this demo app contains a small script which defines various objects in javascript for us to use.

http://fable.io/docs/compatibility.html
and
https://medium.com/@zaid.naom/f-interop-with-javascript-in-fable-the-complete-guide-ccc5b896a59f
are (March 2018) the current main references, and worth reading.
But there is some stuff in here which isn't in there, particularly the use of $ for function application
and examples of Global, along with working examples of some things which are only briefly mentioned.
*)
namespace FableApp1
(*
Our first module demonstrates an approach to java interop in which we assume nothing at all about the underlying javascript objects, we just know how to use them from within existing javascript
We can do the same with Fable - the syntax is a bit different, but the principles are the same.
*)
module Utils =
 open Fable.Core
 open Fable.Core.JsInterop
 open Fable.Import

// This method allows us to write comments which will appear in the
// compiled code and the console as well as the source....
 let c s = Browser.console.log(s)

module VeryDirtyFable =
 open Fable.Core
 open Fable.Core.JsInterop
 open Fable.Import
 open Utils
// Suppose we have some javascript which creates some globals of various sorts
// We can import them by name as generic objects
// At this stage, we don't care what they really are
 let [<Global("boringObj")>] boring1Obj : obj = jsNative
 let [<Global("boringArr")>] boring1Arr : obj = jsNative
 let [<Global("boringInt")>] boring1Int : obj = jsNative
 let [<Global("boringString")>] boring1String : obj = jsNative
 let [<Global("boringFunction")>] boring1Function : obj = jsNative
 let [<Global("callbackFunction")>] callback1Function : obj = jsNative
 let [<Global("nully")>] nully1Function : obj = jsNative
 let [<Global("ExternalClass")>] ExternalClass : obj = jsNative
// Now we can start playing with them
 let init() =
   c"//START HERE"
   c "//Manipulating a value which is really a pojo object under the hood"
   c "// Add a new value to an object"
   c "// boringObj.newValue = \"newValue\""

   boring1Obj?newValue <- "newValue"
   Browser.console.log("boring = ",boring1Obj)
   c "// and access its values like this"
   c "// var boringObjInt1 = boringObj.int1"
   let boringObjInt1 = boring1Obj?int1
   Browser.console.log("boring.int1 = ",boringObjInt1)
   c "// or dynamically"
   c "// var boringObjString1 = boringObj[\"string1\"]"
   let boringObjString1 = boring1Obj?("string1")
   c "// If a value in an object is a function, we can call it like this:"
   c " //boringObj.func(100)"
   let boringObjFn1Result1 = boring1Obj?func(100)
   Browser.console.log("result of calling should be 101 = ",boringObjFn1Result1)
   c "// What's returned is chainable, so if we want to manipulate the value, we need to unbox it first"
   c "// let l = boringObjFn1Result1 + 2"
   let l = !!boringObjFn1Result1 + 2
   Browser.console.log("Result should be 103 = ",l)

   c"// Now if our imported object is really a javascript function we can use $ to call it"
   c"// var ans1 = boringFunction(1,2)"
   let ans1 = boring1Function $ (1,2)
   Browser.console.log("Prints params 1 and 2 as side effect and then boring ", ans1)
   c" //var ans2 = nully(0) show what happens when null is produced"
   let ans2 = nully1Function $ (0)
   Browser.console.log("This is null:",ans2)
   c"// If our imported opject is really an array"
   c"// var arrel1 = boringArr[2]"
   let arrel1 = boring1Arr?(2)
   Browser.console.log("This is three",arrel1)
   c"// boringArr[5] = \"Five\""
   boring1Arr?(5) <- "Five"
   Browser.console.log("Shows the array after updates:",boring1Arr)
   c"// And of course our imported int and string are just that when unboxed"
   Browser.console.log(!!boring1Int + 2) // 25
   Browser.console.log(!!boring1String + " - hello") // Forty-two - Hello

   c"// So far we have shown how to dynamically access existing objects"
   c"// The next thing we need to know is how to create js friendly stuff"
   c"//var s1 = \"String\""
   let s1 = "String"
   Browser.console.log("s1=",s1)
   c" var i1 = 23"
   let i1 = 23
   Browser.console.log("i1=",i1)
   c"var localArray1 = [ 1,2,\"three\"]"
   let localArray1 : obj array = [| 1;2;"three"|] // note the (obj array) type - that's important
   Browser.console.log("localArray is ",localArray1)
   c"// Otherwise we have to box each element, or we would be forced to get a homogeneous array"
   c"// which might be transformed to a more specific JS class. For example"
   let localIntArray1 = [| 1;2;3|]
   Browser.console.log("localIntArray1=",localIntArray1) // this is an Int32Array - not the same thing
   c" //Now to create some objects"
   c"// var localObj1 = { \"a\":\"string\",\"b\":2 }"
   let localObj1 =
     createObj [
         "a" ==> "string"
         "b" ==> 2
     ]
   Browser.console.log("localObj1=",localObj1)



   c"// Now what about javascript classes"
   c"// const newClass1 = new ExternalClass();"
   let newClass1 = createNew ExternalClass ()

   Browser.console.log("instance of ExternalClass = ",newClass1)

   c"//create a function and pass a javascript function as parameter"
   let myfun a b =
      a b
   c"// applies boring function to 2 and undefined"
   let result = myfun !!boring1Function 2
   Browser.console.log("Result should be boring and = ",result)
   Browser.console.log("End of dirty section")

   c"Of course you can go the other way and use an f# function as a callback to js"
   let myCallback a =
       a * 2
   let callbackResult = callback1Function $ (myCallback,10)
   Browser.console.log("should be 20:",callbackResult)

   (*
    Somewhat nicer than accessing globals is taking advantage of javascript modules. This allows us to
    interoperate with things like npm packages and so on.

    http://fable.io/docs/interacting.html starts with a very good description of how to do this
    so I won't repeat it here. But some quick aides-memoire:

      open Fable.Import

      import * as buttons from "my-lib/buttons" translates as
      let buttons: obj = importAll "my-lib/buttons"

      import { myString } from "my-lib" is:
      let myString: obj = importMember "my-lib"
              or:
      let aDifferentName: obj = import "myString" "my-lib"
      if we want to use a different name

      We can also use the [<Import...>] directives which do much the same
      let [<Import("*","redux")>] Redux: obj = jsNative
*)


module TypedFable =
 open Fable.Core
 open Fable.Core.JsInterop
 open Fable.Import
// open Fable.Core.DynamicExtensions Doesn't work in REPL
 open Utils
   // We are going back to our globals to make it easy to see the difference with the original untyped
   // code. All these techniques work the same with modules

 type BoringObjType = {
     mutable string1: string
     int1: int
// DOESN'T WORK IN REPL     func: int->int
 }

// If we want to import a class in a strongly typed way, we need to create an interface with the right
// signature. Tools exist to do this automatically from typescript...

 type ExternalClassType =
    abstract member doit : unit->unit

 let [<Global("boringObj")>] boring1Obj : BoringObjType = jsNative
 let [<Global("boringArr")>] boring1Arr : obj array = jsNative
 let [<Global("boringInt")>] boring1Int : int = jsNative
 let [<Global("boringString")>] boring1String : string = jsNative
 // Now a function: there are several variants...
 // notice here that we restrict the parameter type to int even though the underlying javascript doesn't care
 // This is a design choice - we could leave the parameters as obj if we still need that flexibility

 // This version produces a normal F# function which doesn't allow a variable
 // number of arguments. If you call it with too few arguments, F# will think it's curried, but javascript
 // won't
 let [<Global("boringFunction")>] boring1Function : int -> int ->string  = jsNative
  // This version passes its parameters as an array and supports a variable number of arguments
 // Note the use of Emit to override the default generated javascript
 // Emit is a potentially useful but rather low-down escape hatch
 // It's useful but this feels clunky
 // Note that the DynamicExtensions Invoke does this automatically for obj parameters

 // Fable will interpret null from javascript as None
 let [<Global("nully")>] nully1Function : int -> int option  = jsNative

 type BoringVar1 =
 // This allows us to handle an array of the same type as a variable number of parameters
 // Because F# needs a type for this, we use an abstract interface and Emit to produce the right code
    [<Emit("$0($1...)")>]
    abstract member MyInvoke : [<System.ParamArray>] parameters: int array -> string
// This variant accepts either one or two parameters, which could have different types
    [<Emit("$0($1,$2)")>]
    abstract member FullInvoke :  p1: int * ?p2: int -> string
 let [<Global("boringFunction")>] boring1FunctionVar1 : BoringVar1  = jsNative

// But suppose we want a variable number of parameters of different types, i.e. optional ones

 let [<Global("ExternalClass")>] ExternalClass : obj = jsNative

 let [<Global("myExternalClassInstance")>] myExternalClassInstance1 : ExternalClassType = jsNative

 let init() =
       c"Start of more strongly typed section"
       c"// Boring1Obj is now strongly typed, so we are restricted in what we can do...."
       boring1Obj.string1 <- "String 1 can still be accessed and assigned to"
       Browser.console.log("Access string1 = ",boring1Obj.string1)

       c"boring1Obj.int1 <- 123 would fail to compile as field was not declared mutable"

       c" boring1Obj.newValue <- \"This doesn't work and won't compile as not known field\""
       boring1Obj?newValue <- "Can still get round the type restrictions this way"
       Browser.console.log("boring1Obj now is ",boring1Obj)

       c"// Now functions - we can call our function cleanly..."
       let ans = boring1Function 8 9
       Browser.console.log("output from boring function is ",ans)

       c"// Or, with the DynamicExtensions (not available in REPL) we can also say:"
       c"// let ans2 = boring1Function.Invoke(12,15)"
       c"// but we've lost our typesafety as Invoke no longer checks for an integer"

       c"// This demonstrates that we can create a more strongly typed version of invoke ourselves"
       let ans3 = boring1FunctionVar1.MyInvoke(18,19)
       c"// We have to be a bit careful if we want to call the function with only one parameter"
       let ans4 = boring1FunctionVar1.MyInvoke (17)
       c"// and these work fine too and are fully type-safe"
       let ans5 = boring1FunctionVar1.FullInvoke(234,567)
       let ans6 = boring1FunctionVar1.FullInvoke(234)
       c"// Note that MyInvoke with one parameter has undefined as the second parameter, whereas"
       c"// FullInvoke with one parameter has null as the second parameter. This may or may not matter"


       c"// But - functions aren't curried by default in Fable, unlike in normal F#, but the type system"
       c"// doesn't know that. So"
       let ans6 = boring1Function 13
       c"// looks like it's been curried, and has a signature which suggests that ans6 is a function"
       c"// but that's not what happens, so:"
       c"// let ans7 = ans6 15 // compiles but doesn't work as the function doesn't return a function"
       c"// Instead, ans6 is the result of calling boring(13,undefined)"

       c"// Our optional output works as expected"
       match nully1Function(0) with
       | None -> Browser.console.log("Nully returned null")
       | Some x -> Browser.console.log("Nully returned:",x)

       match nully1Function(10) with
       | None -> Browser.console.log("Nully returned null")
       | Some x -> Browser.console.log("Nully returned:",x)


       c"// Now lets consider our class and its instance"
       c"// Since the class has a known type, our syntax is much cleaner and properly type-checked"
       myExternalClassInstance1.doit ()
       ()

module JSTypedFable =
 open Fable.Core
 open Fable.Core.JsInterop
 open Fable.Import
 open Utils

(*
    There are some more useful features of Fable which can aid in writing expressive and typesafe code
    The pojo, erase, and stringenum attributes in particular
*)
// The Pojo attribute ensures that the result is a plain js object
 type [<Pojo>] MyPojoObject1 = {s1:string;i1:int}
// without it we get a class
 type MyNonPojoObject1 = {s1:string;i1:int}

// Erase does something similar for unions - we just get the value, not the discriminator
 [<Erase>]
 type MyErased1 =
    | EStr1 of string
    | EInt1 of int

 type MyUnion1 =
    | Str1 of string
    | Int1 of int
// And stringenum gives us the ability to treat a string value like an enum
// so Vertical will compile to "Vertical"
 [<StringEnum>]
 type MyStrings =
    | Vertical
    | [<CompiledName("Horizontal")>] Horizontal // get round automatic lowering of case of first character

 let init() =
    let p1: MyPojoObject1 = {s1="a";i1=1}
    let p2: MyNonPojoObject1 = {s1="a";i1=1}
    c"// p1 is {} and p2 is a new Class()"
    Browser.console.log(p1,p2)
    c"// We can demonstrate the use of the Erased attribute to create js friendly unions"
    let e1: MyErased1 = EStr1 "Erased"
    let u1: MyUnion1 = Str1 "Not Erased"

    Browser.console.log(e1,u1) // e1 is just a string, e2 is a class

    c"// And showing off StringEnum"
    Browser.console.log(Vertical,Horizontal ) // just prints the string
    c"// Of course, in javascript it's quite common to have objects with optional values"
    c"// So how do we ensure typesafety for those?"
    c"// There are several possibilities:"
    c"// One is to use a list of union and some converter functions:"
    c"// Remember the MyUnion type"
    let u1list = [ Str1 "One"; Int1 1]
    let u1obj = keyValueList CaseRules.LowerFirst u1list
    Browser.console.log(u1list,u1obj)
    c"//u1list is a javascript List, but u1obj is a pojo"
    ()
//
module MainProgram =
   VeryDirtyFable.init()
   TypedFable.init()
   JSTypedFable.init()
   Utils.c"That's all, folks"