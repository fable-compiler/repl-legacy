namespace Fable.Import

open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.JS
open Fable.Import.Browser

module monaco =
    type [<AllowNullLiteral>] Thenable<'T> =
        abstract ``then``: ?onfulfilled: Func<'T, U2<'TResult, Thenable<'TResult>>> * ?onrejected: Func<obj, U2<'TResult, Thenable<'TResult>>> -> Thenable<'TResult>
        abstract ``then``: ?onfulfilled: Func<'T, U2<'TResult, Thenable<'TResult>>> * ?onrejected: Func<obj, unit> -> Thenable<'TResult>

    and [<AllowNullLiteral>] IDisposable =
        abstract dispose: unit -> unit

    and [<AllowNullLiteral>] IEvent<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: listener: Func<'T, obj> * ?thisArg: obj -> IDisposable

    and [<AllowNullLiteral>] [<Import("Emitter","monaco")>] Emitter<'T>() =
        member __.``event`` with get(): IEvent<'T> = jsNative and set(v: IEvent<'T>): unit = jsNative
        member __.fire(?``event``: 'T): unit = jsNative
        member __.dispose(): unit = jsNative

    and Severity =
        | Ignore = 0
        | Info = 1
        | Warning = 2
        | Error = 3

    and [<AllowNullLiteral>] TValueCallback<'T> =
        [<Emit("$0($1...)")>] abstract Invoke: value: 'T -> unit

    and [<AllowNullLiteral>] ProgressCallback =
        [<Emit("$0($1...)")>] abstract Invoke: progress: obj -> obj

    and [<AllowNullLiteral>] [<Import("Promise","monaco")>] Promise<'V>(init: Func<TValueCallback<'V>, Func<obj, unit>, ProgressCallback, unit>, ?oncancel: obj) =
        member __.``then``(?success: Func<'V, Promise<'U>>, ?error: Func<obj, Promise<'U>>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, Promise<'U>>, ?error: Func<obj, U2<Promise<'U>, 'U>>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, Promise<'U>>, ?error: Func<obj, 'U>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, Promise<'U>>, ?error: Func<obj, unit>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, U2<Promise<'U>, 'U>>, ?error: Func<obj, Promise<'U>>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, U2<Promise<'U>, 'U>>, ?error: Func<obj, U2<Promise<'U>, 'U>>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, U2<Promise<'U>, 'U>>, ?error: Func<obj, 'U>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, U2<Promise<'U>, 'U>>, ?error: Func<obj, unit>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, 'U>, ?error: Func<obj, Promise<'U>>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, 'U>, ?error: Func<obj, U2<Promise<'U>, 'U>>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, 'U>, ?error: Func<obj, 'U>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``then``(?success: Func<'V, 'U>, ?error: Func<obj, unit>, ?progress: ProgressCallback): Promise<'U> = jsNative
        member __.``done``(?success: Func<'V, unit>, ?error: Func<obj, obj>, ?progress: ProgressCallback): unit = jsNative
        member __.cancel(): unit = jsNative
        static member ``as``(value: 'ValueType): Promise<'ValueType> = jsNative
        static member is(value: obj): obj = jsNative
        static member timeout(delay: float): Promise<unit> = jsNative
        static member join(promises: ResizeArray<Promise<'ValueType>>): Promise<ResizeArray<'ValueType>> = jsNative
        static member join(promises: ResizeArray<Thenable<'ValueType>>): Thenable<ResizeArray<'ValueType>> = jsNative
        static member join(promises: obj): Promise<obj> = jsNative
        static member any(promises: ResizeArray<Promise<'ValueType>>): Promise<obj> = jsNative
        static member wrap(value: Thenable<'ValueType>): Promise<'ValueType> = jsNative
        static member wrap(value: 'ValueType): Promise<'ValueType> = jsNative
        static member wrapError(error: obj): Promise<'ValueType> = jsNative

    and [<AllowNullLiteral>] [<Import("CancellationTokenSource","monaco")>] CancellationTokenSource() =
        member __.token with get(): CancellationToken = jsNative and set(v: CancellationToken): unit = jsNative
        member __.cancel(): unit = jsNative
        member __.dispose(): unit = jsNative

    and [<AllowNullLiteral>] CancellationToken =
        abstract isCancellationRequested: bool with get, set
        abstract onCancellationRequested: IEvent<obj> with get, set

    and [<AllowNullLiteral>] [<Import("Uri","monaco")>] Uri() =
        member __.scheme with get(): string = jsNative and set(v: string): unit = jsNative
        member __.authority with get(): string = jsNative and set(v: string): unit = jsNative
        member __.path with get(): string = jsNative and set(v: string): unit = jsNative
        member __.query with get(): string = jsNative and set(v: string): unit = jsNative
        member __.fragment with get(): string = jsNative and set(v: string): unit = jsNative
        member __.fsPath with get(): string = jsNative and set(v: string): unit = jsNative
        static member isUri(thing: obj): obj = jsNative
        member __.``with``(change: obj): Uri = jsNative
        static member parse(value: string): Uri = jsNative
        static member file(path: string): Uri = jsNative
        static member from(components: obj): Uri = jsNative
        member __.toString(?skipEncoding: bool): string = jsNative
        member __.toJSON(): obj = jsNative
        static member revive(data: obj): Uri = jsNative

    and KeyCode =
        | Unknown = 0
        | Backspace = 1
        | Tab = 2
        | Enter = 3
        | Shift = 4
        | Ctrl = 5
        | Alt = 6
        | PauseBreak = 7
        | CapsLock = 8
        | Escape = 9
        | Space = 10
        | PageUp = 11
        | PageDown = 12
        | End = 13
        | Home = 14
        | LeftArrow = 15
        | UpArrow = 16
        | RightArrow = 17
        | DownArrow = 18
        | Insert = 19
        | Delete = 20
        | KEY_0 = 21
        | KEY_1 = 22
        | KEY_2 = 23
        | KEY_3 = 24
        | KEY_4 = 25
        | KEY_5 = 26
        | KEY_6 = 27
        | KEY_7 = 28
        | KEY_8 = 29
        | KEY_9 = 30
        | KEY_A = 31
        | KEY_B = 32
        | KEY_C = 33
        | KEY_D = 34
        | KEY_E = 35
        | KEY_F = 36
        | KEY_G = 37
        | KEY_H = 38
        | KEY_I = 39
        | KEY_J = 40
        | KEY_K = 41
        | KEY_L = 42
        | KEY_M = 43
        | KEY_N = 44
        | KEY_O = 45
        | KEY_P = 46
        | KEY_Q = 47
        | KEY_R = 48
        | KEY_S = 49
        | KEY_T = 50
        | KEY_U = 51
        | KEY_V = 52
        | KEY_W = 53
        | KEY_X = 54
        | KEY_Y = 55
        | KEY_Z = 56
        | Meta = 57
        | ContextMenu = 58
        | F1 = 59
        | F2 = 60
        | F3 = 61
        | F4 = 62
        | F5 = 63
        | F6 = 64
        | F7 = 65
        | F8 = 66
        | F9 = 67
        | F10 = 68
        | F11 = 69
        | F12 = 70
        | F13 = 71
        | F14 = 72
        | F15 = 73
        | F16 = 74
        | F17 = 75
        | F18 = 76
        | F19 = 77
        | NumLock = 78
        | ScrollLock = 79
        | US_SEMICOLON = 80
        | US_EQUAL = 81
        | US_COMMA = 82
        | US_MINUS = 83
        | US_DOT = 84
        | US_SLASH = 85
        | US_BACKTICK = 86
        | US_OPEN_SQUARE_BRACKET = 87
        | US_BACKSLASH = 88
        | US_CLOSE_SQUARE_BRACKET = 89
        | US_QUOTE = 90
        | OEM_8 = 91
        | OEM_102 = 92
        | NUMPAD_0 = 93
        | NUMPAD_1 = 94
        | NUMPAD_2 = 95
        | NUMPAD_3 = 96
        | NUMPAD_4 = 97
        | NUMPAD_5 = 98
        | NUMPAD_6 = 99
        | NUMPAD_7 = 100
        | NUMPAD_8 = 101
        | NUMPAD_9 = 102
        | NUMPAD_MULTIPLY = 103
        | NUMPAD_ADD = 104
        | NUMPAD_SEPARATOR = 105
        | NUMPAD_SUBTRACT = 106
        | NUMPAD_DECIMAL = 107
        | NUMPAD_DIVIDE = 108
        | MAX_VALUE = 109

    and [<AllowNullLiteral>] [<Import("KeyMod","monaco")>] KeyMod() =
        static member CtrlCmd with get(): float = jsNative and set(v: float): unit = jsNative
        static member Shift with get(): float = jsNative and set(v: float): unit = jsNative
        static member Alt with get(): float = jsNative and set(v: float): unit = jsNative
        static member WinCtrl with get(): float = jsNative and set(v: float): unit = jsNative
        static member chord(firstPart: float, secondPart: float): float = jsNative

    and [<AllowNullLiteral>] [<Import("Keybinding","monaco")>] Keybinding(keybinding: float) =
        member __.value with get(): float = jsNative and set(v: float): unit = jsNative
        member __.equals(other: Keybinding): bool = jsNative
        member __.hasCtrlCmd(): bool = jsNative
        member __.hasShift(): bool = jsNative
        member __.hasAlt(): bool = jsNative
        member __.hasWinCtrl(): bool = jsNative
        member __.isModifierKey(): bool = jsNative
        member __.getKeyCode(): KeyCode = jsNative

    and MarkedString =
        // U2<string, obj>
        | Case1 of string option
        | Case2 of language: string * value: string

    and [<AllowNullLiteral>] IKeyboardEvent =
        abstract browserEvent: KeyboardEvent with get, set
        abstract target: HTMLElement with get, set
        abstract ctrlKey: bool with get, set
        abstract shiftKey: bool with get, set
        abstract altKey: bool with get, set
        abstract metaKey: bool with get, set
        abstract keyCode: KeyCode with get, set
        abstract toKeybinding: unit -> Keybinding
        abstract equals: keybinding: float -> bool
        abstract preventDefault: unit -> unit
        abstract stopPropagation: unit -> unit

    and [<AllowNullLiteral>] IMouseEvent =
        abstract browserEvent: MouseEvent with get, set
        abstract leftButton: bool with get, set
        abstract middleButton: bool with get, set
        abstract rightButton: bool with get, set
        abstract target: HTMLElement with get, set
        abstract detail: float with get, set
        abstract posx: float with get, set
        abstract posy: float with get, set
        abstract ctrlKey: bool with get, set
        abstract shiftKey: bool with get, set
        abstract altKey: bool with get, set
        abstract metaKey: bool with get, set
        abstract timestamp: float with get, set
        abstract preventDefault: unit -> unit
        abstract stopPropagation: unit -> unit

    and [<AllowNullLiteral>] IScrollEvent =
        abstract scrollTop: float with get, set
        abstract scrollLeft: float with get, set
        abstract scrollWidth: float with get, set
        abstract scrollHeight: float with get, set
        abstract scrollTopChanged: bool with get, set
        abstract scrollLeftChanged: bool with get, set
        abstract scrollWidthChanged: bool with get, set
        abstract scrollHeightChanged: bool with get, set

    and [<AllowNullLiteral>] IPosition =
        abstract lineNumber: float with get, set
        abstract column: float with get, set

    and [<AllowNullLiteral>] IRange =
        abstract startLineNumber: float with get, set
        abstract startColumn: float with get, set
        abstract endLineNumber: float with get, set
        abstract endColumn: float with get, set

    and [<AllowNullLiteral>] ISelection =
        abstract selectionStartLineNumber: float with get, set
        abstract selectionStartColumn: float with get, set
        abstract positionLineNumber: float with get, set
        abstract positionColumn: float with get, set

    and [<AllowNullLiteral>] [<Import("Position","monaco")>] Position(lineNumber: float, column: float) =
        member __.lineNumber with get(): float = jsNative and set(v: float): unit = jsNative
        member __.column with get(): float = jsNative and set(v: float): unit = jsNative
        member __.equals(other: IPosition): bool = jsNative
        static member equals(a: IPosition, b: IPosition): bool = jsNative
        member __.isBefore(other: IPosition): bool = jsNative
        static member isBefore(a: IPosition, b: IPosition): bool = jsNative
        member __.isBeforeOrEqual(other: IPosition): bool = jsNative
        static member isBeforeOrEqual(a: IPosition, b: IPosition): bool = jsNative
        static member compare(a: IPosition, b: IPosition): float = jsNative
        member __.clone(): Position = jsNative
        member __.toString(): string = jsNative
        static member lift(pos: IPosition): Position = jsNative
        static member isIPosition(obj: obj): obj = jsNative

    and [<AllowNullLiteral>] [<Import("Range","monaco")>] Range(startLineNumber: float, startColumn: float, endLineNumber: float, endColumn: float) =
        member __.startLineNumber with get(): float = jsNative and set(v: float): unit = jsNative
        member __.startColumn with get(): float = jsNative and set(v: float): unit = jsNative
        member __.endLineNumber with get(): float = jsNative and set(v: float): unit = jsNative
        member __.endColumn with get(): float = jsNative and set(v: float): unit = jsNative
        member __.isEmpty(): bool = jsNative
        static member isEmpty(range: IRange): bool = jsNative
        member __.containsPosition(position: IPosition): bool = jsNative
        static member containsPosition(range: IRange, position: IPosition): bool = jsNative
        member __.containsRange(range: IRange): bool = jsNative
        static member containsRange(range: IRange, otherRange: IRange): bool = jsNative
        member __.plusRange(range: IRange): Range = jsNative
        static member plusRange(a: IRange, b: IRange): Range = jsNative
        member __.intersectRanges(range: IRange): Range = jsNative
        static member intersectRanges(a: IRange, b: IRange): Range = jsNative
        member __.equalsRange(other: IRange): bool = jsNative
        static member equalsRange(a: IRange, b: IRange): bool = jsNative
        member __.getEndPosition(): Position = jsNative
        member __.getStartPosition(): Position = jsNative
        member __.cloneRange(): Range = jsNative
        member __.toString(): string = jsNative
        member __.setEndPosition(endLineNumber: float, endColumn: float): Range = jsNative
        member __.setStartPosition(startLineNumber: float, startColumn: float): Range = jsNative
        member __.collapseToStart(): Range = jsNative
        static member collapseToStart(range: IRange): Range = jsNative
        static member lift(range: IRange): Range = jsNative
        static member isIRange(obj: obj): obj = jsNative
        static member areIntersectingOrTouching(a: IRange, b: IRange): bool = jsNative
        static member compareRangesUsingStarts(a: IRange, b: IRange): float = jsNative
        static member compareRangesUsingEnds(a: IRange, b: IRange): float = jsNative
        static member spansMultipleLines(range: IRange): bool = jsNative

    and [<AllowNullLiteral>] [<Import("Selection","monaco")>] Selection(selectionStartLineNumber: float, selectionStartColumn: float, positionLineNumber: float, positionColumn: float) =
        inherit Range(selectionStartLineNumber, selectionStartColumn, positionLineNumber, positionColumn)
        member __.selectionStartLineNumber with get(): float = jsNative and set(v: float): unit = jsNative
        member __.selectionStartColumn with get(): float = jsNative and set(v: float): unit = jsNative
        member __.positionLineNumber with get(): float = jsNative and set(v: float): unit = jsNative
        member __.positionColumn with get(): float = jsNative and set(v: float): unit = jsNative
        member __.clone(): Selection = jsNative
        member __.toString(): string = jsNative
        member __.equalsSelection(other: ISelection): bool = jsNative
        static member selectionsEqual(a: ISelection, b: ISelection): bool = jsNative
        member __.getDirection(): SelectionDirection = jsNative
        member __.setEndPosition(endLineNumber: float, endColumn: float): Selection = jsNative
        member __.setStartPosition(startLineNumber: float, startColumn: float): Selection = jsNative
        static member liftSelection(sel: ISelection): Selection = jsNative
        static member selectionsArrEqual(a: ResizeArray<ISelection>, b: ResizeArray<ISelection>): bool = jsNative
        static member isISelection(obj: obj): obj = jsNative
        static member createWithDirection(startLineNumber: float, startColumn: float, endLineNumber: float, endColumn: float, direction: SelectionDirection): Selection = jsNative

    and SelectionDirection =
        | LTR = 0
        | RTL = 1

    and [<AllowNullLiteral>] [<Import("Token","monaco")>] Token(offset: float, ``type``: string, language: string) =
        member __._tokenBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
        member __.offset with get(): float = jsNative and set(v: float): unit = jsNative
        member __.``type`` with get(): string = jsNative and set(v: string): unit = jsNative
        member __.language with get(): string = jsNative and set(v: string): unit = jsNative
        member __.toString(): string = jsNative

    module editor =
        type [<AllowNullLiteral>] IDiffNavigator =
            abstract revealFirst: bool with get, set
            abstract canNavigate: unit -> bool
            abstract next: unit -> unit
            abstract previous: unit -> unit
            abstract dispose: unit -> unit

        and [<AllowNullLiteral>] IDiffNavigatorOptions =
            abstract followsCaret: bool option with get, set
            abstract ignoreCharChanges: bool option with get, set
            abstract alwaysRevealFirst: bool option with get, set

        and BuiltinTheme =
            U3<obj, obj, obj>

        and [<AllowNullLiteral>] ITheme =
            abstract ``base``: BuiltinTheme with get, set
            abstract ``inherit``: bool with get, set
            abstract rules: ResizeArray<IThemeRule> with get, set

        and [<AllowNullLiteral>] IThemeRule =
            abstract token: string with get, set
            abstract foreground: string option with get, set
            abstract background: string option with get, set
            abstract fontStyle: string option with get, set

        and [<AllowNullLiteral>] MonacoWebWorker<'T> =
            abstract dispose: unit -> unit
            abstract getProxy: unit -> Promise<'T>
            abstract withSyncedResources: resources: ResizeArray<Uri> -> Promise<'T>

        and [<AllowNullLiteral>] IWebWorkerOptions =
            abstract moduleId: string with get, set
            abstract createData: obj option with get, set
            abstract label: string option with get, set

        and [<AllowNullLiteral>] IEditorConstructionOptions =
            inherit ICodeEditorWidgetCreationOptions
            abstract value: string option with get, set
            abstract language: string option with get, set

        and [<AllowNullLiteral>] IDiffEditorConstructionOptions =
            inherit IDiffEditorOptions


        and [<AllowNullLiteral>] IStandaloneCodeEditor =
            inherit ICodeEditor
            abstract addCommand: keybinding: float * handler: ICommandHandler * context: string -> string
            abstract createContextKey: key: string * defaultValue: 'T -> IContextKey<'T>
            abstract addAction: descriptor: IActionDescriptor -> IDisposable

        and [<AllowNullLiteral>] IStandaloneDiffEditor =
            inherit IDiffEditor
            abstract addCommand: keybinding: float * handler: ICommandHandler * context: string -> string
            abstract createContextKey: key: string * defaultValue: 'T -> IContextKey<'T>
            abstract addAction: descriptor: IActionDescriptor -> IDisposable
            abstract getOriginalEditor: unit -> IStandaloneCodeEditor
            abstract getModifiedEditor: unit -> IStandaloneCodeEditor

        and [<AllowNullLiteral>] ICommandHandler =
            [<Emit("$0($1...)")>] abstract Invoke: [<ParamArray>] args: obj[] -> unit

        and [<AllowNullLiteral>] IContextKey<'T> =
            abstract set: value: 'T -> unit
            abstract reset: unit -> unit
            abstract get: unit -> 'T

        and [<AllowNullLiteral>] IEditorOverrideServices =
            interface end

        and [<AllowNullLiteral>] IMarkerData =
            abstract code: string option with get, set
            abstract severity: Severity with get, set
            abstract message: string with get, set
            abstract source: string option with get, set
            abstract startLineNumber: float with get, set
            abstract startColumn: float with get, set
            abstract endLineNumber: float with get, set
            abstract endColumn: float with get, set

        and [<AllowNullLiteral>] IColorizerOptions =
            abstract tabSize: float option with get, set

        and [<AllowNullLiteral>] IColorizerElementOptions =
            inherit IColorizerOptions
            abstract theme: string option with get, set
            abstract mimeType: string option with get, set

        and ScrollbarVisibility =
            | Auto = 1
            | Hidden = 2
            | Visible = 3

        and [<AllowNullLiteral>] IEditorScrollbarOptions =
            abstract arrowSize: float option with get, set
            abstract vertical: string option with get, set
            abstract horizontal: string option with get, set
            abstract useShadows: bool option with get, set
            abstract verticalHasArrows: bool option with get, set
            abstract horizontalHasArrows: bool option with get, set
            abstract handleMouseWheel: bool option with get, set
            abstract horizontalScrollbarSize: float option with get, set
            abstract verticalScrollbarSize: float option with get, set
            abstract verticalSliderSize: float option with get, set
            abstract horizontalSliderSize: float option with get, set

        and WrappingIndent =
            | None = 0
            | Same = 1
            | Indent = 2

        and LineNumbersOption =
            U4<obj, obj, obj, Func<float, string>>

        and [<AllowNullLiteral>] IEditorOptions =
            abstract experimentalScreenReader: bool option with get, set
            abstract ariaLabel: string option with get, set
            abstract rulers: ResizeArray<float> option with get, set
            abstract wordSeparators: string option with get, set
            abstract selectionClipboard: bool option with get, set
            abstract lineNumbers: LineNumbersOption option with get, set
            abstract selectOnLineNumbers: bool option with get, set
            abstract lineNumbersMinChars: float option with get, set
            abstract glyphMargin: bool option with get, set
            abstract lineDecorationsWidth: U2<float, string> option with get, set
            abstract revealHorizontalRightPadding: float option with get, set
            abstract roundedSelection: bool option with get, set
            abstract theme: string option with get, set
            abstract readOnly: bool option with get, set
            abstract scrollbar: IEditorScrollbarOptions option with get, set
            abstract fixedOverflowWidgets: bool option with get, set
            abstract overviewRulerLanes: float option with get, set
            abstract cursorBlinking: string option with get, set
            abstract mouseWheelZoom: bool option with get, set
            abstract cursorStyle: string option with get, set
            abstract fontLigatures: bool option with get, set
            abstract disableTranslate3d: bool option with get, set
            abstract disableMonospaceOptimizations: bool option with get, set
            abstract hideCursorInOverviewRuler: bool option with get, set
            abstract scrollBeyondLastLine: bool option with get, set
            abstract automaticLayout: bool option with get, set
            abstract wrappingColumn: float option with get, set
            abstract wordWrap: bool option with get, set
            abstract wrappingIndent: string option with get, set
            abstract wordWrapBreakBeforeCharacters: string option with get, set
            abstract wordWrapBreakAfterCharacters: string option with get, set
            abstract wordWrapBreakObtrusiveCharacters: string option with get, set
            abstract stopRenderingLineAfter: float option with get, set
            abstract hover: bool option with get, set
            abstract contextmenu: bool option with get, set
            abstract mouseWheelScrollSensitivity: float option with get, set
            abstract quickSuggestions: bool option with get, set
            abstract quickSuggestionsDelay: float option with get, set
            abstract parameterHints: bool option with get, set
            abstract iconsInSuggestions: bool option with get, set
            abstract autoClosingBrackets: bool option with get, set
            abstract formatOnType: bool option with get, set
            abstract formatOnPaste: bool option with get, set
            abstract suggestOnTriggerCharacters: bool option with get, set
            abstract acceptSuggestionOnEnter: bool option with get, set
            abstract acceptSuggestionOnCommitCharacter: bool option with get, set
            abstract snippetSuggestions: U4<obj, obj, obj, obj> option with get, set
            abstract emptySelectionClipboard: bool option with get, set
            abstract tabCompletion: bool option with get, set
            abstract wordBasedSuggestions: bool option with get, set
            abstract suggestFontSize: float option with get, set
            abstract suggestLineHeight: float option with get, set
            abstract selectionHighlight: bool option with get, set
            abstract codeLens: bool option with get, set
            abstract folding: bool option with get, set
            abstract renderWhitespace: U3<obj, obj, obj> option with get, set
            abstract renderControlCharacters: bool option with get, set
            abstract renderIndentGuides: bool option with get, set
            abstract renderLineHighlight: U4<obj, obj, obj, obj> option with get, set
            abstract useTabStops: bool option with get, set
            abstract fontFamily: string option with get, set
            abstract fontWeight: obj option with get, set
            abstract fontSize: float option with get, set
            abstract lineHeight: float option with get, set

        and [<AllowNullLiteral>] IDiffEditorOptions =
            inherit IEditorOptions
            abstract enableSplitViewResizing: bool option with get, set
            abstract renderSideBySide: bool option with get, set
            abstract ignoreTrimWhitespace: bool option with get, set
            abstract renderIndicators: bool option with get, set
            abstract originalEditable: bool option with get, set

        and [<AllowNullLiteral>] [<Import("editor.InternalEditorScrollbarOptions","monaco")>] InternalEditorScrollbarOptions() =
            member __._internalEditorScrollbarOptionsBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.arrowSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.vertical with get(): ScrollbarVisibility = jsNative and set(v: ScrollbarVisibility): unit = jsNative
            member __.horizontal with get(): ScrollbarVisibility = jsNative and set(v: ScrollbarVisibility): unit = jsNative
            member __.useShadows with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.verticalHasArrows with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.horizontalHasArrows with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.handleMouseWheel with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.horizontalScrollbarSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.horizontalSliderSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.verticalScrollbarSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.verticalSliderSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.mouseWheelScrollSensitivity with get(): float = jsNative and set(v: float): unit = jsNative

        and [<AllowNullLiteral>] [<Import("editor.EditorWrappingInfo","monaco")>] EditorWrappingInfo() =
            member __._editorWrappingInfoBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.isViewportWrapping with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.wrappingColumn with get(): float = jsNative and set(v: float): unit = jsNative
            member __.wrappingIndent with get(): WrappingIndent = jsNative and set(v: WrappingIndent): unit = jsNative
            member __.wordWrapBreakBeforeCharacters with get(): string = jsNative and set(v: string): unit = jsNative
            member __.wordWrapBreakAfterCharacters with get(): string = jsNative and set(v: string): unit = jsNative
            member __.wordWrapBreakObtrusiveCharacters with get(): string = jsNative and set(v: string): unit = jsNative

        and [<AllowNullLiteral>] [<Import("editor.InternalEditorViewOptions","monaco")>] InternalEditorViewOptions() =
            member __._internalEditorViewOptionsBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.theme with get(): string = jsNative and set(v: string): unit = jsNative
            member __.canUseTranslate3d with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.disableMonospaceOptimizations with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.experimentalScreenReader with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.rulers with get(): ResizeArray<float> = jsNative and set(v: ResizeArray<float>): unit = jsNative
            member __.ariaLabel with get(): string = jsNative and set(v: string): unit = jsNative
            member __.renderLineNumbers with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.renderCustomLineNumbers with get(): Func<float, string> = jsNative and set(v: Func<float, string>): unit = jsNative
            member __.renderRelativeLineNumbers with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.selectOnLineNumbers with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.glyphMargin with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.revealHorizontalRightPadding with get(): float = jsNative and set(v: float): unit = jsNative
            member __.roundedSelection with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.overviewRulerLanes with get(): float = jsNative and set(v: float): unit = jsNative
            member __.cursorBlinking with get(): TextEditorCursorBlinkingStyle = jsNative and set(v: TextEditorCursorBlinkingStyle): unit = jsNative
            member __.mouseWheelZoom with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.cursorStyle with get(): TextEditorCursorStyle = jsNative and set(v: TextEditorCursorStyle): unit = jsNative
            member __.hideCursorInOverviewRuler with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.scrollBeyondLastLine with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.editorClassName with get(): string = jsNative and set(v: string): unit = jsNative
            member __.stopRenderingLineAfter with get(): float = jsNative and set(v: float): unit = jsNative
            member __.renderWhitespace with get(): U3<obj, obj, obj> = jsNative and set(v: U3<obj, obj, obj>): unit = jsNative
            member __.renderControlCharacters with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.renderIndentGuides with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.renderLineHighlight with get(): U4<obj, obj, obj, obj> = jsNative and set(v: U4<obj, obj, obj, obj>): unit = jsNative
            member __.scrollbar with get(): InternalEditorScrollbarOptions = jsNative and set(v: InternalEditorScrollbarOptions): unit = jsNative
            member __.fixedOverflowWidgets with get(): bool = jsNative and set(v: bool): unit = jsNative

        and [<AllowNullLiteral>] IViewConfigurationChangedEvent =
            abstract theme: bool with get, set
            abstract canUseTranslate3d: bool with get, set
            abstract disableMonospaceOptimizations: bool with get, set
            abstract experimentalScreenReader: bool with get, set
            abstract rulers: bool with get, set
            abstract ariaLabel: bool with get, set
            abstract renderLineNumbers: bool with get, set
            abstract renderCustomLineNumbers: bool with get, set
            abstract renderRelativeLineNumbers: bool with get, set
            abstract selectOnLineNumbers: bool with get, set
            abstract glyphMargin: bool with get, set
            abstract revealHorizontalRightPadding: bool with get, set
            abstract roundedSelection: bool with get, set
            abstract overviewRulerLanes: bool with get, set
            abstract cursorBlinking: bool with get, set
            abstract mouseWheelZoom: bool with get, set
            abstract cursorStyle: bool with get, set
            abstract hideCursorInOverviewRuler: bool with get, set
            abstract scrollBeyondLastLine: bool with get, set
            abstract editorClassName: bool with get, set
            abstract stopRenderingLineAfter: bool with get, set
            abstract renderWhitespace: bool with get, set
            abstract renderControlCharacters: bool with get, set
            abstract renderIndentGuides: bool with get, set
            abstract renderLineHighlight: bool with get, set
            abstract scrollbar: bool with get, set
            abstract fixedOverflowWidgets: bool with get, set

        and [<AllowNullLiteral>] [<Import("editor.EditorContribOptions","monaco")>] EditorContribOptions() =
            member __.selectionClipboard with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.hover with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.contextmenu with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.quickSuggestions with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.quickSuggestionsDelay with get(): float = jsNative and set(v: float): unit = jsNative
            member __.parameterHints with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.iconsInSuggestions with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.formatOnType with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.formatOnPaste with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.suggestOnTriggerCharacters with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.acceptSuggestionOnEnter with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.acceptSuggestionOnCommitCharacter with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.snippetSuggestions with get(): U4<obj, obj, obj, obj> = jsNative and set(v: U4<obj, obj, obj, obj>): unit = jsNative
            member __.emptySelectionClipboard with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.tabCompletion with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.wordBasedSuggestions with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.suggestFontSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.suggestLineHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.selectionHighlight with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.codeLens with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.folding with get(): bool = jsNative and set(v: bool): unit = jsNative

        and [<AllowNullLiteral>] [<Import("editor.InternalEditorOptions","monaco")>] InternalEditorOptions() =
            member __._internalEditorOptionsBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.lineHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.readOnly with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.wordSeparators with get(): string = jsNative and set(v: string): unit = jsNative
            member __.autoClosingBrackets with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.useTabStops with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.tabFocusMode with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.layoutInfo with get(): EditorLayoutInfo = jsNative and set(v: EditorLayoutInfo): unit = jsNative
            member __.fontInfo with get(): FontInfo = jsNative and set(v: FontInfo): unit = jsNative
            member __.viewInfo with get(): InternalEditorViewOptions = jsNative and set(v: InternalEditorViewOptions): unit = jsNative
            member __.wrappingInfo with get(): EditorWrappingInfo = jsNative and set(v: EditorWrappingInfo): unit = jsNative
            member __.contribInfo with get(): EditorContribOptions = jsNative and set(v: EditorContribOptions): unit = jsNative

        and [<AllowNullLiteral>] IConfigurationChangedEvent =
            abstract lineHeight: bool with get, set
            abstract readOnly: bool with get, set
            abstract wordSeparators: bool with get, set
            abstract autoClosingBrackets: bool with get, set
            abstract useTabStops: bool with get, set
            abstract tabFocusMode: bool with get, set
            abstract layoutInfo: bool with get, set
            abstract fontInfo: bool with get, set
            abstract viewInfo: IViewConfigurationChangedEvent with get, set
            abstract wrappingInfo: bool with get, set
            abstract contribInfo: bool with get, set

        and OverviewRulerLane =
            | Left = 1
            | Center = 2
            | Right = 4
            | Full = 7

        and [<AllowNullLiteral>] IModelDecorationOverviewRulerOptions =
            abstract color: string with get, set
            abstract darkColor: string with get, set
            abstract position: OverviewRulerLane with get, set

        and [<AllowNullLiteral>] IModelDecorationOptions =
            abstract stickiness: TrackedRangeStickiness option with get, set
            abstract className: string option with get, set
            abstract glyphMarginHoverMessage: U2<MarkedString, ResizeArray<MarkedString>> option with get, set
            abstract hoverMessage: U2<MarkedString, ResizeArray<MarkedString>> option with get, set
            abstract isWholeLine: bool option with get, set
            abstract showInOverviewRuler: string option with get, set
            abstract overviewRuler: IModelDecorationOverviewRulerOptions option with get, set
            abstract glyphMarginClassName: string option with get, set
            abstract linesDecorationsClassName: string option with get, set
            abstract marginClassName: string option with get, set
            abstract inlineClassName: string option with get, set
            abstract beforeContentClassName: string option with get, set
            abstract afterContentClassName: string option with get, set

        and [<AllowNullLiteral>] IModelDeltaDecoration =
            abstract range: IRange with get, set
            abstract options: IModelDecorationOptions with get, set

        and [<AllowNullLiteral>] IModelDecoration =
            abstract id: string with get, set
            abstract ownerId: float with get, set
            abstract range: Range with get, set
            abstract options: IModelDecorationOptions with get, set
            abstract isForValidation: bool with get, set

        and [<AllowNullLiteral>] IWordAtPosition =
            abstract word: string with get, set
            abstract startColumn: float with get, set
            abstract endColumn: float with get, set

        and EndOfLinePreference =
            | TextDefined = 0
            | LF = 1
            | CRLF = 2

        and DefaultEndOfLine =
            | LF = 1
            | CRLF = 2

        and EndOfLineSequence =
            | LF = 0
            | CRLF = 1

        and [<AllowNullLiteral>] ISingleEditOperationIdentifier =
            abstract major: float with get, set
            abstract minor: float with get, set

        and [<AllowNullLiteral>] IEditOperationBuilder =
            abstract addEditOperation: range: Range * text: string -> unit
            abstract trackSelection: selection: Selection * ?trackPreviousOnEmpty: bool -> string

        and [<AllowNullLiteral>] ICursorStateComputerData =
            abstract getInverseEditOperations: unit -> ResizeArray<IIdentifiedSingleEditOperation>
            abstract getTrackedSelection: id: string -> Selection

        and [<AllowNullLiteral>] ICommand =
            abstract getEditOperations: model: ITokenizedModel * builder: IEditOperationBuilder -> unit
            abstract computeCursorState: model: ITokenizedModel * helper: ICursorStateComputerData -> Selection

        and [<AllowNullLiteral>] ISingleEditOperation =
            abstract range: IRange with get, set
            abstract text: string with get, set
            abstract forceMoveMarkers: bool option with get, set

        and [<AllowNullLiteral>] IIdentifiedSingleEditOperation =
            abstract identifier: ISingleEditOperationIdentifier with get, set
            abstract range: Range with get, set
            abstract text: string with get, set
            abstract forceMoveMarkers: bool with get, set
            abstract isAutoWhitespaceEdit: bool option with get, set

        and [<AllowNullLiteral>] ICursorStateComputer =
            [<Emit("$0($1...)")>] abstract Invoke: inverseEditOperations: ResizeArray<IIdentifiedSingleEditOperation> -> ResizeArray<Selection>

        and [<AllowNullLiteral>] [<Import("editor.TextModelResolvedOptions","monaco")>] TextModelResolvedOptions() =
            member __._textModelResolvedOptionsBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.tabSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.insertSpaces with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.defaultEOL with get(): DefaultEndOfLine = jsNative and set(v: DefaultEndOfLine): unit = jsNative
            member __.trimAutoWhitespace with get(): bool = jsNative and set(v: bool): unit = jsNative

        and [<AllowNullLiteral>] ITextModelUpdateOptions =
            abstract tabSize: float option with get, set
            abstract insertSpaces: bool option with get, set
            abstract trimAutoWhitespace: bool option with get, set

        and [<AllowNullLiteral>] IModelOptionsChangedEvent =
            abstract tabSize: bool with get, set
            abstract insertSpaces: bool with get, set
            abstract trimAutoWhitespace: bool with get, set

        and [<AllowNullLiteral>] ITextModel =
            abstract getOptions: unit -> TextModelResolvedOptions
            abstract getVersionId: unit -> float
            abstract getAlternativeVersionId: unit -> float
            abstract setValue: newValue: string -> unit
            abstract getValue: ?eol: EndOfLinePreference * ?preserveBOM: bool -> string
            abstract getValueLength: ?eol: EndOfLinePreference * ?preserveBOM: bool -> float
            abstract getValueInRange: range: IRange * ?eol: EndOfLinePreference -> string
            abstract getValueLengthInRange: range: IRange -> float
            abstract getLineCount: unit -> float
            abstract getLineContent: lineNumber: float -> string
            abstract getLinesContent: unit -> ResizeArray<string>
            abstract getEOL: unit -> string
            abstract setEOL: eol: EndOfLineSequence -> unit
            abstract getLineMinColumn: lineNumber: float -> float
            abstract getLineMaxColumn: lineNumber: float -> float
            abstract getLineFirstNonWhitespaceColumn: lineNumber: float -> float
            abstract getLineLastNonWhitespaceColumn: lineNumber: float -> float
            abstract validatePosition: position: IPosition -> Position
            abstract modifyPosition: position: IPosition * offset: float -> Position
            abstract validateRange: range: IRange -> Range
            abstract getOffsetAt: position: IPosition -> float
            abstract getPositionAt: offset: float -> Position
            abstract getFullModelRange: unit -> Range
            abstract isDisposed: unit -> bool
            abstract findMatches: searchString: string * searchOnlyEditableRange: bool * isRegex: bool * matchCase: bool * wholeWord: bool * captureMatches: bool * ?limitResultCount: float -> ResizeArray<FindMatch>
            abstract findMatches: searchString: string * searchScope: IRange * isRegex: bool * matchCase: bool * wholeWord: bool * captureMatches: bool * ?limitResultCount: float -> ResizeArray<FindMatch>
            abstract findNextMatch: searchString: string * searchStart: IPosition * isRegex: bool * matchCase: bool * wholeWord: bool * captureMatches: bool -> FindMatch
            abstract findPreviousMatch: searchString: string * searchStart: IPosition * isRegex: bool * matchCase: bool * wholeWord: bool * captureMatches: bool -> FindMatch

        and [<AllowNullLiteral>] [<Import("editor.FindMatch","monaco")>] FindMatch() =
            member __._findMatchBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.range with get(): Range = jsNative and set(v: Range): unit = jsNative
            member __.matches with get(): ResizeArray<string> = jsNative and set(v: ResizeArray<string>): unit = jsNative

        and [<AllowNullLiteral>] IReadOnlyModel =
            inherit ITextModel
            abstract uri: Uri with get, set
            abstract getModeId: unit -> string
            abstract getWordAtPosition: position: IPosition -> IWordAtPosition
            abstract getWordUntilPosition: position: IPosition -> IWordAtPosition

        and [<AllowNullLiteral>] ITokenizedModel =
            inherit ITextModel
            abstract getModeId: unit -> string
            abstract getWordAtPosition: position: IPosition -> IWordAtPosition
            abstract getWordUntilPosition: position: IPosition -> IWordAtPosition

        and [<AllowNullLiteral>] ITextModelWithMarkers =
            inherit ITextModel


        and TrackedRangeStickiness =
            | AlwaysGrowsWhenTypingAtEdges = 0
            | NeverGrowsWhenTypingAtEdges = 1
            | GrowsOnlyWhenTypingBefore = 2
            | GrowsOnlyWhenTypingAfter = 3

        and [<AllowNullLiteral>] ITextModelWithDecorations =
            abstract deltaDecorations: oldDecorations: ResizeArray<string> * newDecorations: ResizeArray<IModelDeltaDecoration> * ?ownerId: float -> ResizeArray<string>
            abstract getDecorationOptions: id: string -> IModelDecorationOptions
            abstract getDecorationRange: id: string -> Range
            abstract getLineDecorations: lineNumber: float * ?ownerId: float * ?filterOutValidation: bool -> ResizeArray<IModelDecoration>
            abstract getLinesDecorations: startLineNumber: float * endLineNumber: float * ?ownerId: float * ?filterOutValidation: bool -> ResizeArray<IModelDecoration>
            abstract getDecorationsInRange: range: IRange * ?ownerId: float * ?filterOutValidation: bool -> ResizeArray<IModelDecoration>
            abstract getAllDecorations: ?ownerId: float * ?filterOutValidation: bool -> ResizeArray<IModelDecoration>

        and [<AllowNullLiteral>] IEditableTextModel =
            inherit ITextModelWithMarkers
            abstract normalizeIndentation: str: string -> string
            abstract getOneIndent: unit -> string
            abstract updateOptions: newOpts: ITextModelUpdateOptions -> unit
            abstract detectIndentation: defaultInsertSpaces: bool * defaultTabSize: float -> unit
            abstract pushStackElement: unit -> unit
            abstract pushEditOperations: beforeCursorState: ResizeArray<Selection> * editOperations: ResizeArray<IIdentifiedSingleEditOperation> * cursorStateComputer: ICursorStateComputer -> ResizeArray<Selection>
            abstract applyEdits: operations: ResizeArray<IIdentifiedSingleEditOperation> -> ResizeArray<IIdentifiedSingleEditOperation>

        and [<AllowNullLiteral>] IModel =
            inherit IReadOnlyModel
            inherit IEditableTextModel
            inherit ITextModelWithMarkers
            inherit ITokenizedModel
            inherit ITextModelWithDecorations
            inherit IEditorModel
            abstract id: string with get, set
            abstract onDidChangeContent: listener: Func<IModelContentChangedEvent2, unit> -> IDisposable
            abstract onDidChangeDecorations: listener: Func<IModelDecorationsChangedEvent, unit> -> IDisposable
            abstract onDidChangeOptions: listener: Func<IModelOptionsChangedEvent, unit> -> IDisposable
            abstract onDidChangeLanguage: listener: Func<IModelLanguageChangedEvent, unit> -> IDisposable
            abstract onWillDispose: listener: Func<unit, unit> -> IDisposable
            abstract dispose: unit -> unit

        and [<AllowNullLiteral>] IModelLanguageChangedEvent =
            abstract oldLanguage: string with get, set
            abstract newLanguage: string with get, set

        and [<AllowNullLiteral>] IModelContentChangedEvent2 =
            abstract range: IRange with get, set
            abstract rangeLength: float with get, set
            abstract text: string with get, set
            abstract eol: string with get, set
            abstract versionId: float with get, set
            abstract isUndoing: bool with get, set
            abstract isRedoing: bool with get, set

        and [<AllowNullLiteral>] IModelDecorationsChangedEvent =
            abstract addedDecorations: ResizeArray<string> with get, set
            abstract changedDecorations: ResizeArray<string> with get, set
            abstract removedDecorations: ResizeArray<string> with get, set

        and [<AllowNullLiteral>] IModelTokensChangedEvent =
            abstract ranges: ResizeArray<obj> with get, set

        and CursorChangeReason =
            | NotSet = 0
            | ContentFlush = 1
            | RecoverFromMarkers = 2
            | Explicit = 3
            | Paste = 4
            | Undo = 5
            | Redo = 6

        and [<AllowNullLiteral>] ICursorPositionChangedEvent =
            abstract position: Position with get, set
            abstract viewPosition: Position with get, set
            abstract secondaryPositions: ResizeArray<Position> with get, set
            abstract secondaryViewPositions: ResizeArray<Position> with get, set
            abstract reason: CursorChangeReason with get, set
            abstract source: string with get, set
            abstract isInEditableRange: bool with get, set

        and [<AllowNullLiteral>] ICursorSelectionChangedEvent =
            abstract selection: Selection with get, set
            abstract viewSelection: Selection with get, set
            abstract secondarySelections: ResizeArray<Selection> with get, set
            abstract secondaryViewSelections: ResizeArray<Selection> with get, set
            abstract source: string with get, set
            abstract reason: CursorChangeReason with get, set

        and [<AllowNullLiteral>] IModelChangedEvent =
            abstract oldModelUrl: Uri with get, set
            abstract newModelUrl: Uri with get, set

        and [<AllowNullLiteral>] [<Import("editor.OverviewRulerPosition","monaco")>] OverviewRulerPosition() =
            member __._overviewRulerPositionBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.width with get(): float = jsNative and set(v: float): unit = jsNative
            member __.height with get(): float = jsNative and set(v: float): unit = jsNative
            member __.top with get(): float = jsNative and set(v: float): unit = jsNative
            member __.right with get(): float = jsNative and set(v: float): unit = jsNative

        and [<AllowNullLiteral>] [<Import("editor.EditorLayoutInfo","monaco")>] EditorLayoutInfo() =
            member __._editorLayoutInfoBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.width with get(): float = jsNative and set(v: float): unit = jsNative
            member __.height with get(): float = jsNative and set(v: float): unit = jsNative
            member __.glyphMarginLeft with get(): float = jsNative and set(v: float): unit = jsNative
            member __.glyphMarginWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.glyphMarginHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.lineNumbersLeft with get(): float = jsNative and set(v: float): unit = jsNative
            member __.lineNumbersWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.lineNumbersHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.decorationsLeft with get(): float = jsNative and set(v: float): unit = jsNative
            member __.decorationsWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.decorationsHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.contentLeft with get(): float = jsNative and set(v: float): unit = jsNative
            member __.contentWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.contentHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.verticalScrollbarWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.horizontalScrollbarHeight with get(): float = jsNative and set(v: float): unit = jsNative
            member __.overviewRuler with get(): OverviewRulerPosition = jsNative and set(v: OverviewRulerPosition): unit = jsNative

        and [<AllowNullLiteral>] ICodeEditorWidgetCreationOptions =
            inherit IEditorOptions
            abstract model: IModel option with get, set

        and [<AllowNullLiteral>] IEditorModel =
            interface end

        and [<AllowNullLiteral>] IEditorViewState =
            interface end

        and [<AllowNullLiteral>] IDimension =
            abstract width: float with get, set
            abstract height: float with get, set

        and [<AllowNullLiteral>] ICursorState =
            abstract inSelectionMode: bool with get, set
            abstract selectionStart: IPosition with get, set
            abstract position: IPosition with get, set

        and [<AllowNullLiteral>] IViewState =
            abstract scrollTop: float with get, set
            abstract scrollTopWithoutViewZones: float with get, set
            abstract scrollLeft: float with get, set

        and [<AllowNullLiteral>] ICodeEditorViewState =
            inherit IEditorViewState
            abstract cursorState: ResizeArray<ICursorState> with get, set
            abstract viewState: IViewState with get, set
            abstract contributionsState: obj with get, set

        and MouseTargetType =
            | UNKNOWN = 0
            | TEXTAREA = 1
            | GUTTER_GLYPH_MARGIN = 2
            | GUTTER_LINE_NUMBERS = 3
            | GUTTER_LINE_DECORATIONS = 4
            | GUTTER_VIEW_ZONE = 5
            | CONTENT_TEXT = 6
            | CONTENT_EMPTY = 7
            | CONTENT_VIEW_ZONE = 8
            | CONTENT_WIDGET = 9
            | OVERVIEW_RULER = 10
            | SCROLLBAR = 11
            | OVERLAY_WIDGET = 12

        and [<AllowNullLiteral>] IDiffEditorModel =
            inherit IEditorModel
            abstract original: IModel with get, set
            abstract modified: IModel with get, set

        and [<AllowNullLiteral>] IDiffEditorViewState =
            inherit IEditorViewState
            abstract original: ICodeEditorViewState with get, set
            abstract modified: ICodeEditorViewState with get, set

        and [<AllowNullLiteral>] IChange =
            abstract originalStartLineNumber: float with get, set
            abstract originalEndLineNumber: float with get, set
            abstract modifiedStartLineNumber: float with get, set
            abstract modifiedEndLineNumber: float with get, set

        and [<AllowNullLiteral>] ICharChange =
            inherit IChange
            abstract originalStartColumn: float with get, set
            abstract originalEndColumn: float with get, set
            abstract modifiedStartColumn: float with get, set
            abstract modifiedEndColumn: float with get, set

        and [<AllowNullLiteral>] ILineChange =
            inherit IChange
            abstract charChanges: ResizeArray<ICharChange> with get, set

        and [<AllowNullLiteral>] IDiffLineInformation =
            abstract equivalentLineNumber: float with get, set

        and [<AllowNullLiteral>] INewScrollPosition =
            abstract scrollLeft: float option with get, set
            abstract scrollTop: float option with get, set

        and [<AllowNullLiteral>] IActionDescriptor =
            abstract id: string with get, set
            abstract label: string with get, set
            abstract precondition: string option with get, set
            abstract keybindings: ResizeArray<float> option with get, set
            abstract keybindingContext: string option with get, set
            abstract contextMenuGroupId: string option with get, set
            abstract contextMenuOrder: float option with get, set
            abstract run: editor: ICommonCodeEditor -> U2<unit, Promise<unit>>

        and [<AllowNullLiteral>] IEditorAction =
            abstract id: string with get, set
            abstract label: string with get, set
            abstract alias: string with get, set
            abstract isSupported: unit -> bool
            abstract run: unit -> Promise<unit>

        and [<AllowNullLiteral>] IEditor =
            abstract onDidChangeModelContent: listener: Func<IModelContentChangedEvent2, unit> -> IDisposable
            abstract onDidChangeModelLanguage: listener: Func<IModelLanguageChangedEvent, unit> -> IDisposable
            abstract onDidChangeModelOptions: listener: Func<IModelOptionsChangedEvent, unit> -> IDisposable
            abstract onDidChangeConfiguration: listener: Func<IConfigurationChangedEvent, unit> -> IDisposable
            abstract onDidChangeCursorPosition: listener: Func<ICursorPositionChangedEvent, unit> -> IDisposable
            abstract onDidChangeCursorSelection: listener: Func<ICursorSelectionChangedEvent, unit> -> IDisposable
            abstract onDidDispose: listener: Func<unit, unit> -> IDisposable
            abstract dispose: unit -> unit
            abstract getId: unit -> string
            abstract getEditorType: unit -> string
            abstract updateOptions: newOptions: IEditorOptions -> unit
            abstract layout: ?dimension: IDimension -> unit
            abstract focus: unit -> unit
            abstract isFocused: unit -> bool
            abstract getActions: unit -> ResizeArray<IEditorAction>
            abstract getSupportedActions: unit -> ResizeArray<IEditorAction>
            abstract saveViewState: unit -> IEditorViewState
            abstract restoreViewState: state: IEditorViewState -> unit
            abstract getVisibleColumnFromPosition: position: IPosition -> float
            abstract getPosition: unit -> Position
            abstract setPosition: position: IPosition -> unit
            abstract revealLine: lineNumber: float -> unit
            abstract revealLineInCenter: lineNumber: float -> unit
            abstract revealLineInCenterIfOutsideViewport: lineNumber: float -> unit
            abstract revealPosition: position: IPosition -> unit
            abstract revealPositionInCenter: position: IPosition -> unit
            abstract revealPositionInCenterIfOutsideViewport: position: IPosition -> unit
            abstract getSelection: unit -> Selection
            abstract getSelections: unit -> ResizeArray<Selection>
            abstract setSelection: selection: IRange -> unit
            abstract setSelection: selection: Range -> unit
            abstract setSelection: selection: ISelection -> unit
            abstract setSelection: selection: Selection -> unit
            abstract setSelections: selections: ResizeArray<ISelection> -> unit
            abstract revealLines: startLineNumber: float * endLineNumber: float -> unit
            abstract revealLinesInCenter: lineNumber: float * endLineNumber: float -> unit
            abstract revealLinesInCenterIfOutsideViewport: lineNumber: float * endLineNumber: float -> unit
            abstract revealRange: range: IRange -> unit
            abstract revealRangeInCenter: range: IRange -> unit
            abstract revealRangeAtTop: range: IRange -> unit
            abstract revealRangeInCenterIfOutsideViewport: range: IRange -> unit
            abstract trigger: source: string * handlerId: string * payload: obj -> unit
            abstract getModel: unit -> IEditorModel
            abstract setModel: model: IEditorModel -> unit

        and [<AllowNullLiteral>] IEditorContribution =
            abstract getId: unit -> string
            abstract dispose: unit -> unit
            abstract saveViewState: unit -> obj
            abstract restoreViewState: state: obj -> unit

        and [<AllowNullLiteral>] ICommonCodeEditor =
            inherit IEditor
            abstract onDidChangeModel: listener: Func<IModelChangedEvent, unit> -> IDisposable
            abstract onDidChangeModelDecorations: listener: Func<IModelDecorationsChangedEvent, unit> -> IDisposable
            abstract onDidFocusEditorText: listener: Func<unit, unit> -> IDisposable
            abstract onDidBlurEditorText: listener: Func<unit, unit> -> IDisposable
            abstract onDidFocusEditor: listener: Func<unit, unit> -> IDisposable
            abstract onDidBlurEditor: listener: Func<unit, unit> -> IDisposable
            abstract hasWidgetFocus: unit -> bool
            abstract getContribution: id: string -> 'T
            abstract getModel: unit -> IModel
            abstract getConfiguration: unit -> InternalEditorOptions
            abstract getValue: ?options: obj -> string
            abstract setValue: newValue: string -> unit
            abstract getScrollWidth: unit -> float
            abstract getScrollLeft: unit -> float
            abstract getScrollHeight: unit -> float
            abstract getScrollTop: unit -> float
            abstract setScrollLeft: newScrollLeft: float -> unit
            abstract setScrollTop: newScrollTop: float -> unit
            abstract setScrollPosition: position: INewScrollPosition -> unit
            abstract getAction: id: string -> IEditorAction
            abstract executeCommand: source: string * command: ICommand -> unit
            abstract pushUndoStop: unit -> bool
            abstract executeEdits: source: string * edits: ResizeArray<IIdentifiedSingleEditOperation> * ?endCursoState: ResizeArray<Selection> -> bool
            abstract executeCommands: source: string * commands: ResizeArray<ICommand> -> unit
            abstract getLineDecorations: lineNumber: float -> ResizeArray<IModelDecoration>
            abstract deltaDecorations: oldDecorations: ResizeArray<string> * newDecorations: ResizeArray<IModelDeltaDecoration> -> ResizeArray<string>
            abstract getLayoutInfo: unit -> EditorLayoutInfo

        and [<AllowNullLiteral>] ICommonDiffEditor =
            inherit IEditor
            abstract onDidUpdateDiff: listener: Func<unit, unit> -> IDisposable
            abstract getModel: unit -> IDiffEditorModel
            abstract getOriginalEditor: unit -> ICommonCodeEditor
            abstract getModifiedEditor: unit -> ICommonCodeEditor
            abstract getLineChanges: unit -> ResizeArray<ILineChange>
            abstract getDiffLineInformationForOriginal: lineNumber: float -> IDiffLineInformation
            abstract getDiffLineInformationForModified: lineNumber: float -> IDiffLineInformation
            abstract getValue: ?options: obj -> string

        and [<AllowNullLiteral>] EditorTypeType =
            abstract ICodeEditor: string with get, set
            abstract IDiffEditor: string with get, set

        and [<AllowNullLiteral>] CursorMovePositionType =
            abstract Left: string with get, set
            abstract Right: string with get, set
            abstract Up: string with get, set
            abstract Down: string with get, set
            abstract WrappedLineStart: string with get, set
            abstract WrappedLineFirstNonWhitespaceCharacter: string with get, set
            abstract WrappedLineColumnCenter: string with get, set
            abstract WrappedLineEnd: string with get, set
            abstract WrappedLineLastNonWhitespaceCharacter: string with get, set
            abstract ViewPortTop: string with get, set
            abstract ViewPortCenter: string with get, set
            abstract ViewPortBottom: string with get, set
            abstract ViewPortIfOutside: string with get, set

        and [<AllowNullLiteral>] CursorMoveByUnitType =
            abstract Line: string with get, set
            abstract WrappedLine: string with get, set
            abstract Character: string with get, set
            abstract HalfLine: string with get, set

        and [<AllowNullLiteral>] CursorMoveArguments =
            abstract ``to``: string with get, set
            abstract select: bool option with get, set
            abstract by: string option with get, set
            abstract value: float option with get, set

        and [<AllowNullLiteral>] EditorScrollDirectionType =
            abstract Up: string with get, set
            abstract Down: string with get, set

        and [<AllowNullLiteral>] EditorScrollByUnitType =
            abstract Line: string with get, set
            abstract WrappedLine: string with get, set
            abstract Page: string with get, set
            abstract HalfPage: string with get, set

        and [<AllowNullLiteral>] EditorScrollArguments =
            abstract ``to``: string with get, set
            abstract by: string option with get, set
            abstract value: float option with get, set
            abstract revealCursor: bool option with get, set

        and [<AllowNullLiteral>] RevealLineArguments =
            abstract lineNumber: float option with get, set
            abstract at: string option with get, set

        and [<AllowNullLiteral>] RevealLineAtArgumentType =
            abstract Top: string with get, set
            abstract Center: string with get, set
            abstract Bottom: string with get, set

        and [<AllowNullLiteral>] HandlerType =
            abstract ExecuteCommand: string with get, set
            abstract ExecuteCommands: string with get, set
            abstract CursorLeft: string with get, set
            abstract CursorLeftSelect: string with get, set
            abstract CursorWordLeft: string with get, set
            abstract CursorWordStartLeft: string with get, set
            abstract CursorWordEndLeft: string with get, set
            abstract CursorWordLeftSelect: string with get, set
            abstract CursorWordStartLeftSelect: string with get, set
            abstract CursorWordEndLeftSelect: string with get, set
            abstract CursorRight: string with get, set
            abstract CursorRightSelect: string with get, set
            abstract CursorWordRight: string with get, set
            abstract CursorWordStartRight: string with get, set
            abstract CursorWordEndRight: string with get, set
            abstract CursorWordRightSelect: string with get, set
            abstract CursorWordStartRightSelect: string with get, set
            abstract CursorWordEndRightSelect: string with get, set
            abstract CursorUp: string with get, set
            abstract CursorUpSelect: string with get, set
            abstract CursorDown: string with get, set
            abstract CursorDownSelect: string with get, set
            abstract CursorPageUp: string with get, set
            abstract CursorPageUpSelect: string with get, set
            abstract CursorPageDown: string with get, set
            abstract CursorPageDownSelect: string with get, set
            abstract CursorHome: string with get, set
            abstract CursorHomeSelect: string with get, set
            abstract CursorEnd: string with get, set
            abstract CursorEndSelect: string with get, set
            abstract ExpandLineSelection: string with get, set
            abstract CursorTop: string with get, set
            abstract CursorTopSelect: string with get, set
            abstract CursorBottom: string with get, set
            abstract CursorBottomSelect: string with get, set
            abstract CursorColumnSelectLeft: string with get, set
            abstract CursorColumnSelectRight: string with get, set
            abstract CursorColumnSelectUp: string with get, set
            abstract CursorColumnSelectPageUp: string with get, set
            abstract CursorColumnSelectDown: string with get, set
            abstract CursorColumnSelectPageDown: string with get, set
            abstract CursorMove: string with get, set
            abstract AddCursorDown: string with get, set
            abstract AddCursorUp: string with get, set
            abstract CursorUndo: string with get, set
            abstract MoveTo: string with get, set
            abstract MoveToSelect: string with get, set
            abstract ColumnSelect: string with get, set
            abstract CreateCursor: string with get, set
            abstract LastCursorMoveToSelect: string with get, set
            abstract Type: string with get, set
            abstract ReplacePreviousChar: string with get, set
            abstract CompositionStart: string with get, set
            abstract CompositionEnd: string with get, set
            abstract Paste: string with get, set
            abstract Tab: string with get, set
            abstract Indent: string with get, set
            abstract Outdent: string with get, set
            abstract DeleteLeft: string with get, set
            abstract DeleteRight: string with get, set
            abstract DeleteWordLeft: string with get, set
            abstract DeleteWordStartLeft: string with get, set
            abstract DeleteWordEndLeft: string with get, set
            abstract DeleteWordRight: string with get, set
            abstract DeleteWordStartRight: string with get, set
            abstract DeleteWordEndRight: string with get, set
            abstract RemoveSecondaryCursors: string with get, set
            abstract CancelSelection: string with get, set
            abstract Cut: string with get, set
            abstract Undo: string with get, set
            abstract Redo: string with get, set
            abstract WordSelect: string with get, set
            abstract WordSelectDrag: string with get, set
            abstract LastCursorWordSelect: string with get, set
            abstract LineSelect: string with get, set
            abstract LineSelectDrag: string with get, set
            abstract LastCursorLineSelect: string with get, set
            abstract LastCursorLineSelectDrag: string with get, set
            abstract LineInsertBefore: string with get, set
            abstract LineInsertAfter: string with get, set
            abstract LineBreakInsert: string with get, set
            abstract SelectAll: string with get, set
            abstract EditorScroll: string with get, set
            abstract ScrollLineUp: string with get, set
            abstract ScrollLineDown: string with get, set
            abstract ScrollPageUp: string with get, set
            abstract ScrollPageDown: string with get, set
            abstract RevealLine: string with get, set

        and TextEditorCursorStyle =
            | Line = 1
            | Block = 2
            | Underline = 3

        and TextEditorCursorBlinkingStyle =
            | Hidden = 0
            | Blink = 1
            | Smooth = 2
            | Phase = 3
            | Expand = 4
            | Solid = 5

        and [<AllowNullLiteral>] IViewZone =
            abstract afterLineNumber: float with get, set
            abstract afterColumn: float option with get, set
            abstract suppressMouseDown: bool option with get, set
            abstract heightInLines: float option with get, set
            abstract heightInPx: float option with get, set
            abstract domNode: HTMLElement with get, set
            abstract marginDomNode: HTMLElement option with get, set
            abstract onDomNodeTop: Func<float, unit> option with get, set
            abstract onComputedHeight: Func<float, unit> option with get, set

        and [<AllowNullLiteral>] IViewZoneChangeAccessor =
            abstract addZone: zone: IViewZone -> float
            abstract removeZone: id: float -> unit
            abstract layoutZone: id: float -> unit

        and ContentWidgetPositionPreference =
            | EXACT = 0
            | ABOVE = 1
            | BELOW = 2

        and [<AllowNullLiteral>] IContentWidgetPosition =
            abstract position: IPosition with get, set
            abstract preference: ResizeArray<ContentWidgetPositionPreference> with get, set

        and [<AllowNullLiteral>] IContentWidget =
            abstract allowEditorOverflow: bool option with get, set
            abstract suppressMouseDown: bool option with get, set
            abstract getId: unit -> string
            abstract getDomNode: unit -> HTMLElement
            abstract getPosition: unit -> IContentWidgetPosition

        and OverlayWidgetPositionPreference =
            | TOP_RIGHT_CORNER = 0
            | BOTTOM_RIGHT_CORNER = 1
            | TOP_CENTER = 2

        and [<AllowNullLiteral>] IOverlayWidgetPosition =
            abstract preference: OverlayWidgetPositionPreference with get, set

        and [<AllowNullLiteral>] IOverlayWidget =
            abstract getId: unit -> string
            abstract getDomNode: unit -> HTMLElement
            abstract getPosition: unit -> IOverlayWidgetPosition

        and [<AllowNullLiteral>] IMouseTarget =
            abstract element: Element with get, set
            abstract ``type``: MouseTargetType with get, set
            abstract position: Position with get, set
            abstract mouseColumn: float with get, set
            abstract range: Range with get, set
            abstract detail: obj with get, set

        and [<AllowNullLiteral>] IEditorMouseEvent =
            abstract ``event``: IMouseEvent with get, set
            abstract target: IMouseTarget with get, set

        and [<AllowNullLiteral>] ICodeEditor =
            inherit ICommonCodeEditor
            abstract onMouseUp: listener: Func<IEditorMouseEvent, unit> -> IDisposable
            abstract onMouseDown: listener: Func<IEditorMouseEvent, unit> -> IDisposable
            abstract onContextMenu: listener: Func<IEditorMouseEvent, unit> -> IDisposable
            abstract onMouseMove: listener: Func<IEditorMouseEvent, unit> -> IDisposable
            abstract onMouseLeave: listener: Func<IEditorMouseEvent, unit> -> IDisposable
            abstract onKeyUp: listener: Func<IKeyboardEvent, unit> -> IDisposable
            abstract onKeyDown: listener: Func<IKeyboardEvent, unit> -> IDisposable
            abstract onDidLayoutChange: listener: Func<EditorLayoutInfo, unit> -> IDisposable
            abstract onDidScrollChange: listener: Func<IScrollEvent, unit> -> IDisposable
            abstract getDomNode: unit -> HTMLElement
            abstract addContentWidget: widget: IContentWidget -> unit
            abstract layoutContentWidget: widget: IContentWidget -> unit
            abstract removeContentWidget: widget: IContentWidget -> unit
            abstract addOverlayWidget: widget: IOverlayWidget -> unit
            abstract layoutOverlayWidget: widget: IOverlayWidget -> unit
            abstract removeOverlayWidget: widget: IOverlayWidget -> unit
            abstract changeViewZones: callback: Func<IViewZoneChangeAccessor, unit> -> unit
            abstract getCenteredRangeInViewport: unit -> Range
            abstract getOffsetForColumn: lineNumber: float * column: float -> float
            abstract render: unit -> unit
            abstract getTopForLineNumber: lineNumber: float -> float
            abstract getTopForPosition: lineNumber: float * column: float -> float
            abstract getTargetAtClientPoint: clientX: float * clientY: float -> IMouseTarget
            abstract getScrolledVisiblePosition: position: IPosition -> obj
            abstract applyFontInfo: target: HTMLElement -> unit

        and [<AllowNullLiteral>] IDiffEditor =
            inherit ICommonDiffEditor
            abstract getDomNode: unit -> HTMLElement

        and [<AllowNullLiteral>] [<Import("editor.FontInfo","monaco")>] FontInfo() =
            inherit BareFontInfo()
            member __._editorStylingBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.isMonospace with get(): bool = jsNative and set(v: bool): unit = jsNative
            member __.typicalHalfwidthCharacterWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.typicalFullwidthCharacterWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.spaceWidth with get(): float = jsNative and set(v: float): unit = jsNative
            member __.maxDigitWidth with get(): float = jsNative and set(v: float): unit = jsNative

        and [<AllowNullLiteral>] [<Import("editor.BareFontInfo","monaco")>] BareFontInfo() =
            member __._bareFontInfoBrand with get(): unit = jsNative and set(v: unit): unit = jsNative
            member __.fontFamily with get(): string = jsNative and set(v: string): unit = jsNative
            member __.fontWeight with get(): string = jsNative and set(v: string): unit = jsNative
            member __.fontSize with get(): float = jsNative and set(v: float): unit = jsNative
            member __.lineHeight with get(): float = jsNative and set(v: float): unit = jsNative

        type [<Import("editor","monaco")>] Globals =
            static member EditorType with get(): EditorTypeType = jsNative and set(v: EditorTypeType): unit = jsNative
            static member CursorMovePosition with get(): CursorMovePositionType = jsNative and set(v: CursorMovePositionType): unit = jsNative
            static member CursorMoveByUnit with get(): CursorMoveByUnitType = jsNative and set(v: CursorMoveByUnitType): unit = jsNative
            static member EditorScrollDirection with get(): EditorScrollDirectionType = jsNative and set(v: EditorScrollDirectionType): unit = jsNative
            static member EditorScrollByUnit with get(): EditorScrollByUnitType = jsNative and set(v: EditorScrollByUnitType): unit = jsNative
            static member RevealLineAtArgument with get(): RevealLineAtArgumentType = jsNative and set(v: RevealLineAtArgumentType): unit = jsNative
            static member Handler with get(): HandlerType = jsNative and set(v: HandlerType): unit = jsNative
            static member create(domElement: HTMLElement, ?options: IEditorConstructionOptions, ?``override``: IEditorOverrideServices): IStandaloneCodeEditor = jsNative
            static member onDidCreateEditor(listener: Func<ICodeEditor, unit>): IDisposable = jsNative
            static member createDiffEditor(domElement: HTMLElement, ?options: IDiffEditorConstructionOptions, ?``override``: IEditorOverrideServices): IStandaloneDiffEditor = jsNative
            static member createDiffNavigator(diffEditor: IStandaloneDiffEditor, ?opts: IDiffNavigatorOptions): IDiffNavigator = jsNative
            static member createModel(value: string, ?language: string, ?uri: Uri): IModel = jsNative
            static member setModelLanguage(model: IModel, language: string): unit = jsNative
            static member setModelMarkers(model: IModel, owner: string, markers: ResizeArray<IMarkerData>): unit = jsNative
            static member getModel(uri: Uri): IModel = jsNative
            static member getModels(): ResizeArray<IModel> = jsNative
            static member onDidCreateModel(listener: Func<IModel, unit>): IDisposable = jsNative
            static member onWillDisposeModel(listener: Func<IModel, unit>): IDisposable = jsNative
            static member onDidChangeModelLanguage(listener: Func<obj, unit>): IDisposable = jsNative
            static member createWebWorker(opts: IWebWorkerOptions): MonacoWebWorker<'T> = jsNative
            static member colorizeElement(domNode: HTMLElement, options: IColorizerElementOptions): Promise<unit> = jsNative
            static member colorize(text: string, languageId: string, options: IColorizerOptions): Promise<string> = jsNative
            static member colorizeModelLine(model: IModel, lineNumber: float, ?tabSize: float): string = jsNative
            static member tokenize(text: string, languageId: string): ResizeArray<ResizeArray<Token>> = jsNative
            static member defineTheme(themeName: string, themeData: ITheme): unit = jsNative



    module languages =
        type [<AllowNullLiteral>] IToken =
            abstract startIndex: float with get, set
            abstract scopes: string with get, set

        and [<AllowNullLiteral>] ILineTokens =
            abstract tokens: ResizeArray<IToken> with get, set
            abstract endState: IState with get, set

        and [<AllowNullLiteral>] TokensProvider =
            abstract getInitialState: unit -> IState
            abstract tokenize: line: string * state: IState -> ILineTokens

        and [<AllowNullLiteral>] CodeActionContext =
            abstract markers: ResizeArray<editor.IMarkerData> with get, set

        and [<AllowNullLiteral>] CodeActionProvider =
            abstract provideCodeActions: model: editor.IReadOnlyModel * range: Range * context: CodeActionContext * token: CancellationToken -> U2<ResizeArray<CodeAction>, Promise<ResizeArray<CodeAction>>>

        and CompletionItemKind =
            | Text = 0
            | Method = 1
            | Function = 2
            | Constructor = 3
            | Field = 4
            | Variable = 5
            | Class = 6
            | Interface = 7
            | Module = 8
            | Property = 9
            | Unit = 10
            | Value = 11
            | Enum = 12
            | Keyword = 13
            | Snippet = 14
            | Color = 15
            | File = 16
            | Reference = 17
            | Folder = 18

        and [<AllowNullLiteral>] SnippetString =
            abstract value: string with get, set

        and [<AllowNullLiteral>] CompletionItem =
            abstract label: string with get, set
            abstract kind: CompletionItemKind with get, set
            abstract detail: string option with get, set
            abstract documentation: string option with get, set
            abstract sortText: string option with get, set
            abstract filterText: string option with get, set
            abstract insertText: U2<string, SnippetString> option with get, set
            abstract range: Range option with get, set
            abstract textEdit: editor.ISingleEditOperation option with get, set

        and [<AllowNullLiteral>] CompletionList =
            abstract isIncomplete: bool option with get, set
            abstract items: ResizeArray<CompletionItem> with get, set

        and [<AllowNullLiteral>] CompletionItemProvider =
            abstract triggerCharacters: ResizeArray<string> option with get //, set
            abstract provideCompletionItems: model: editor.IReadOnlyModel * position: Position * token: CancellationToken -> U4<ResizeArray<CompletionItem>, Promise<ResizeArray<CompletionItem>>, CompletionList, Promise<CompletionList>>
            abstract resolveCompletionItem: item: CompletionItem * token: CancellationToken -> U2<CompletionItem, Promise<CompletionItem>>

        and [<AllowNullLiteral>] CommentRule =
            abstract lineComment: string option with get, set
            abstract blockComment: CharacterPair option with get, set

        and [<AllowNullLiteral>] LanguageConfiguration =
            abstract comments: CommentRule option with get, set
            abstract brackets: ResizeArray<CharacterPair> option with get, set
            abstract wordPattern: Regex option with get, set
            abstract indentationRules: IndentationRule option with get, set
            abstract onEnterRules: ResizeArray<OnEnterRule> option with get, set
            abstract autoClosingPairs: ResizeArray<IAutoClosingPairConditional> option with get, set
            abstract surroundingPairs: ResizeArray<IAutoClosingPair> option with get, set
            abstract ___electricCharacterSupport: IBracketElectricCharacterContribution option with get, set

        and [<AllowNullLiteral>] IndentationRule =
            abstract decreaseIndentPattern: Regex with get, set
            abstract increaseIndentPattern: Regex with get, set
            abstract indentNextLinePattern: Regex option with get, set
            abstract unIndentedLinePattern: Regex option with get, set

        and [<AllowNullLiteral>] OnEnterRule =
            abstract beforeText: Regex with get, set
            abstract afterText: Regex option with get, set
            abstract action: EnterAction with get, set

        and [<AllowNullLiteral>] IBracketElectricCharacterContribution =
            abstract docComment: IDocComment option with get, set

        and [<AllowNullLiteral>] IDocComment =
            abstract ``open``: string with get, set
            abstract close: string with get, set

        and CharacterPair =
            string * string

        and [<AllowNullLiteral>] IAutoClosingPair =
            abstract ``open``: string with get, set
            abstract close: string with get, set

        and [<AllowNullLiteral>] IAutoClosingPairConditional =
            inherit IAutoClosingPair
            abstract notIn: ResizeArray<string> option with get, set

        and IndentAction =
            | None = 0
            | Indent = 1
            | IndentOutdent = 2
            | Outdent = 3

        and [<AllowNullLiteral>] EnterAction =
            abstract indentAction: IndentAction with get, set
            abstract outdentCurrentLine: bool option with get, set
            abstract appendText: string option with get, set
            abstract removeText: float option with get, set

        and [<AllowNullLiteral>] IState =
            abstract clone: unit -> IState
            abstract equals: other: IState -> bool

        and [<AllowNullLiteral>] Hover =
            abstract contents: ResizeArray<MarkedString> with get, set
            abstract range: IRange with get, set

        and [<AllowNullLiteral>] HoverProvider =
            abstract provideHover: model: editor.IReadOnlyModel * position: Position * token: CancellationToken -> U2<Hover, Promise<Hover>>

        and [<AllowNullLiteral>] CodeAction =
            abstract command: Command with get, set
            abstract score: float with get, set

        and [<AllowNullLiteral>] ParameterInformation =
            abstract label: string with get, set
            abstract documentation: string option with get, set

        and [<AllowNullLiteral>] SignatureInformation =
            abstract label: string with get, set
            abstract documentation: string option with get, set
            abstract parameters: ResizeArray<ParameterInformation> with get, set

        and [<AllowNullLiteral>] SignatureHelp =
            abstract signatures: ResizeArray<SignatureInformation> with get, set
            abstract activeSignature: float with get, set
            abstract activeParameter: float with get, set

        and [<AllowNullLiteral>] SignatureHelpProvider =
            abstract signatureHelpTriggerCharacters: ResizeArray<string> with get //, set
            abstract provideSignatureHelp: model: editor.IReadOnlyModel * position: Position * token: CancellationToken -> U2<SignatureHelp, Promise<SignatureHelp>>

        and DocumentHighlightKind =
            | Text = 0
            | Read = 1
            | Write = 2

        and [<AllowNullLiteral>] DocumentHighlight =
            abstract range: IRange with get, set
            abstract kind: DocumentHighlightKind with get, set

        and [<AllowNullLiteral>] DocumentHighlightProvider =
            abstract provideDocumentHighlights: model: editor.IReadOnlyModel * position: Position * token: CancellationToken -> U2<ResizeArray<DocumentHighlight>, Promise<ResizeArray<DocumentHighlight>>>

        and [<AllowNullLiteral>] ReferenceContext =
            abstract includeDeclaration: bool with get, set

        and [<AllowNullLiteral>] ReferenceProvider =
            abstract provideReferences: model: editor.IReadOnlyModel * position: Position * context: ReferenceContext * token: CancellationToken -> U2<ResizeArray<Location>, Promise<ResizeArray<Location>>>

        and [<AllowNullLiteral>] Location =
            abstract uri: Uri with get, set
            abstract range: IRange with get, set

        and Definition =
            U2<Location, ResizeArray<Location>>

        and [<AllowNullLiteral>] DefinitionProvider =
            abstract provideDefinition: model: editor.IReadOnlyModel * position: Position * token: CancellationToken -> U2<Definition, Promise<Definition>>

        and [<AllowNullLiteral>] ImplementationProvider =
            abstract provideImplementation: model: editor.IReadOnlyModel * position: Position * token: CancellationToken -> U2<Definition, Promise<Definition>>

        and SymbolKind =
            | File = 0
            | Module = 1
            | Namespace = 2
            | Package = 3
            | Class = 4
            | Method = 5
            | Property = 6
            | Field = 7
            | Constructor = 8
            | Enum = 9
            | Interface = 10
            | Function = 11
            | Variable = 12
            | Constant = 13
            | String = 14
            | Number = 15
            | Boolean = 16
            | Array = 17
            | Object = 18
            | Key = 19
            | Null = 20

        and [<AllowNullLiteral>] SymbolInformation =
            abstract name: string with get, set
            abstract containerName: string option with get, set
            abstract kind: SymbolKind with get, set
            abstract location: Location with get, set

        and [<AllowNullLiteral>] DocumentSymbolProvider =
            abstract provideDocumentSymbols: model: editor.IReadOnlyModel * token: CancellationToken -> U2<ResizeArray<SymbolInformation>, Promise<ResizeArray<SymbolInformation>>>

        and [<AllowNullLiteral>] FormattingOptions =
            abstract tabSize: float with get, set
            abstract insertSpaces: bool with get, set

        and [<AllowNullLiteral>] DocumentFormattingEditProvider =
            abstract provideDocumentFormattingEdits: model: editor.IReadOnlyModel * options: FormattingOptions * token: CancellationToken -> U2<ResizeArray<editor.ISingleEditOperation>, Promise<ResizeArray<editor.ISingleEditOperation>>>

        and [<AllowNullLiteral>] DocumentRangeFormattingEditProvider =
            abstract provideDocumentRangeFormattingEdits: model: editor.IReadOnlyModel * range: Range * options: FormattingOptions * token: CancellationToken -> U2<ResizeArray<editor.ISingleEditOperation>, Promise<ResizeArray<editor.ISingleEditOperation>>>

        and [<AllowNullLiteral>] OnTypeFormattingEditProvider =
            abstract autoFormatTriggerCharacters: ResizeArray<string> with get, set
            abstract provideOnTypeFormattingEdits: model: editor.IReadOnlyModel * position: Position * ch: string * options: FormattingOptions * token: CancellationToken -> U2<ResizeArray<editor.ISingleEditOperation>, Promise<ResizeArray<editor.ISingleEditOperation>>>

        and [<AllowNullLiteral>] ILink =
            abstract range: IRange with get, set
            abstract url: string with get, set

        and [<AllowNullLiteral>] LinkProvider =
            abstract resolveLink: Func<ILink, CancellationToken, U2<ILink, Promise<ILink>>> option with get, set
            abstract provideLinks: model: editor.IReadOnlyModel * token: CancellationToken -> U2<ResizeArray<ILink>, Promise<ResizeArray<ILink>>>

        and [<AllowNullLiteral>] IResourceEdit =
            abstract resource: Uri with get, set
            abstract range: IRange with get, set
            abstract newText: string with get, set

        and [<AllowNullLiteral>] WorkspaceEdit =
            abstract edits: ResizeArray<IResourceEdit> with get, set
            abstract rejectReason: string option with get, set

        and [<AllowNullLiteral>] RenameProvider =
            abstract provideRenameEdits: model: editor.IReadOnlyModel * position: Position * newName: string * token: CancellationToken -> U2<WorkspaceEdit, Promise<WorkspaceEdit>>

        and [<AllowNullLiteral>] Command =
            abstract id: string with get, set
            abstract title: string with get, set
            abstract arguments: ResizeArray<obj> option with get, set

        and [<AllowNullLiteral>] ICodeLensSymbol =
            abstract range: IRange with get, set
            abstract id: string option with get, set
            abstract command: Command option with get, set

        and [<AllowNullLiteral>] CodeLensProvider =
            abstract onDidChange: IEvent<obj> option with get, set
            abstract provideCodeLenses: model: editor.IReadOnlyModel * token: CancellationToken -> U2<ResizeArray<ICodeLensSymbol>, Promise<ResizeArray<ICodeLensSymbol>>>
            abstract resolveCodeLens: model: editor.IReadOnlyModel * codeLens: ICodeLensSymbol * token: CancellationToken -> U2<ICodeLensSymbol, Promise<ICodeLensSymbol>>

        and [<AllowNullLiteral>] ILanguageExtensionPoint =
            abstract id: string with get, set
            abstract extensions: ResizeArray<string> option with get, set
            abstract filenames: ResizeArray<string> option with get, set
            abstract filenamePatterns: ResizeArray<string> option with get, set
            abstract firstLine: string option with get, set
            abstract aliases: ResizeArray<string> option with get, set
            abstract mimetypes: ResizeArray<string> option with get, set
            abstract configuration: string option with get, set

        and [<AllowNullLiteral>] IMonarchLanguage =
            abstract tokenizer: obj with get, set
            abstract ignoreCase: bool option with get, set
            abstract defaultToken: string option with get, set
            abstract brackets: ResizeArray<IMonarchLanguageBracket> option with get, set
            abstract start: string option with get, set
            abstract tokenPostfix: string with get, set

        and [<AllowNullLiteral>] IMonarchLanguageRule =
            abstract regex: U2<string, Regex> option with get, set
            abstract action: IMonarchLanguageAction option with get, set
            abstract ``include``: string option with get, set

        and [<AllowNullLiteral>] IMonarchLanguageAction =
            abstract group: ResizeArray<IMonarchLanguageAction> option with get, set
            abstract cases: obj option with get, set
            abstract token: string option with get, set
            abstract next: string option with get, set
            abstract switchTo: string option with get, set
            abstract goBack: float option with get, set
            abstract bracket: string option with get, set
            abstract nextEmbedded: string option with get, set
            abstract log: string option with get, set

        and [<AllowNullLiteral>] IMonarchLanguageBracket =
            abstract ``open``: string with get, set
            abstract close: string with get, set
            abstract token: string with get, set

        type [<Import("languages","monaco")>] Globals =
            static member register(language: ILanguageExtensionPoint): unit = jsNative
            static member getLanguages(): ResizeArray<ILanguageExtensionPoint> = jsNative
            static member onLanguage(languageId: string, callback: Func<unit, unit>): IDisposable = jsNative
            static member setLanguageConfiguration(languageId: string, configuration: LanguageConfiguration): IDisposable = jsNative
            static member setTokensProvider(languageId: string, provider: TokensProvider): IDisposable = jsNative
            static member setMonarchTokensProvider(languageId: string, languageDef: IMonarchLanguage): IDisposable = jsNative
            static member registerReferenceProvider(languageId: string, provider: ReferenceProvider): IDisposable = jsNative
            static member registerRenameProvider(languageId: string, provider: RenameProvider): IDisposable = jsNative
            static member registerSignatureHelpProvider(languageId: string, provider: SignatureHelpProvider): IDisposable = jsNative
            static member registerHoverProvider(languageId: string, provider: HoverProvider): IDisposable = jsNative
            static member registerDocumentSymbolProvider(languageId: string, provider: DocumentSymbolProvider): IDisposable = jsNative
            static member registerDocumentHighlightProvider(languageId: string, provider: DocumentHighlightProvider): IDisposable = jsNative
            static member registerDefinitionProvider(languageId: string, provider: DefinitionProvider): IDisposable = jsNative
            static member registerImplementationProvider(languageId: string, provider: ImplementationProvider): IDisposable = jsNative
            static member registerCodeLensProvider(languageId: string, provider: CodeLensProvider): IDisposable = jsNative
            static member registerCodeActionProvider(languageId: string, provider: CodeActionProvider): IDisposable = jsNative
            static member registerDocumentFormattingEditProvider(languageId: string, provider: DocumentFormattingEditProvider): IDisposable = jsNative
            static member registerDocumentRangeFormattingEditProvider(languageId: string, provider: DocumentRangeFormattingEditProvider): IDisposable = jsNative
            static member registerOnTypeFormattingEditProvider(languageId: string, provider: OnTypeFormattingEditProvider): IDisposable = jsNative
            static member registerLinkProvider(languageId: string, provider: LinkProvider): IDisposable = jsNative
            static member registerCompletionItemProvider(languageId: string, provider: CompletionItemProvider): IDisposable = jsNative

        module typescript =
            type ModuleKind =
                | None = 0
                | CommonJS = 1
                | AMD = 2
                | UMD = 3
                | System = 4
                | ES2015 = 5

            and JsxEmit =
                | None = 0
                | Preserve = 1
                | React = 2

            and NewLineKind =
                | CarriageReturnLineFeed = 0
                | LineFeed = 1

            and ScriptTarget =
                | ES3 = 0
                | ES5 = 1
                | ES2015 = 2
                | ES2016 = 3
                | ES2017 = 4
                | ESNext = 5
                | Latest = 5

            and ModuleResolutionKind =
                | Classic = 1
                | NodeJs = 2

            and CompilerOptionsValue =
                obj

            and [<AllowNullLiteral>] CompilerOptions =
                abstract allowJs: bool option with get, set
                abstract allowSyntheticDefaultImports: bool option with get, set
                abstract allowUnreachableCode: bool option with get, set
                abstract allowUnusedLabels: bool option with get, set
                abstract alwaysStrict: bool option with get, set
                abstract baseUrl: string option with get, set
                abstract charset: string option with get, set
                abstract declaration: bool option with get, set
                abstract declarationDir: string option with get, set
                abstract disableSizeLimit: bool option with get, set
                abstract emitBOM: bool option with get, set
                abstract emitDecoratorMetadata: bool option with get, set
                abstract experimentalDecorators: bool option with get, set
                abstract forceConsistentCasingInFileNames: bool option with get, set
                abstract importHelpers: bool option with get, set
                abstract inlineSourceMap: bool option with get, set
                abstract inlineSources: bool option with get, set
                abstract isolatedModules: bool option with get, set
                abstract jsx: JsxEmit option with get, set
                abstract lib: ResizeArray<string> option with get, set
                abstract locale: string option with get, set
                abstract mapRoot: string option with get, set
                abstract maxNodeModuleJsDepth: float option with get, set
                abstract ``module``: ModuleKind option with get, set
                abstract moduleResolution: ModuleResolutionKind option with get, set
                abstract newLine: NewLineKind option with get, set
                abstract noEmit: bool option with get, set
                abstract noEmitHelpers: bool option with get, set
                abstract noEmitOnError: bool option with get, set
                abstract noErrorTruncation: bool option with get, set
                abstract noFallthroughCasesInSwitch: bool option with get, set
                abstract noImplicitAny: bool option with get, set
                abstract noImplicitReturns: bool option with get, set
                abstract noImplicitThis: bool option with get, set
                abstract noUnusedLocals: bool option with get, set
                abstract noUnusedParameters: bool option with get, set
                abstract noImplicitUseStrict: bool option with get, set
                abstract noLib: bool option with get, set
                abstract noResolve: bool option with get, set
                abstract out: string option with get, set
                abstract outDir: string option with get, set
                abstract outFile: string option with get, set
                abstract preserveConstEnums: bool option with get, set
                abstract project: string option with get, set
                abstract reactNamespace: string option with get, set
                abstract jsxFactory: string option with get, set
                abstract removeComments: bool option with get, set
                abstract rootDir: string option with get, set
                abstract rootDirs: ResizeArray<string> option with get, set
                abstract skipLibCheck: bool option with get, set
                abstract skipDefaultLibCheck: bool option with get, set
                abstract sourceMap: bool option with get, set
                abstract sourceRoot: string option with get, set
                abstract strictNullChecks: bool option with get, set
                abstract suppressExcessPropertyErrors: bool option with get, set
                abstract suppressImplicitAnyIndexErrors: bool option with get, set
                abstract target: ScriptTarget option with get, set
                abstract traceResolution: bool option with get, set
                abstract types: ResizeArray<string> option with get, set
                abstract typeRoots: ResizeArray<string> option with get, set
                [<Emit("$0[$1]{{=$2}}")>] abstract Item: option: string -> U2<CompilerOptionsValue, obj> with get, set

            and [<AllowNullLiteral>] DiagnosticsOptions =
                abstract noSemanticValidation: bool option with get, set
                abstract noSyntaxValidation: bool option with get, set

            and [<AllowNullLiteral>] LanguageServiceDefaults =
                abstract addExtraLib: content: string * ?filePath: string -> IDisposable
                abstract setCompilerOptions: options: CompilerOptions -> unit
                abstract setDiagnosticsOptions: options: DiagnosticsOptions -> unit
                abstract setMaximunWorkerIdleTime: value: float -> unit

            type [<Import("languages.typescript","monaco")>] Globals =
                static member typescriptDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative
                static member javascriptDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative
                static member getTypeScriptWorker with get(): Func<unit, Promise<obj>> = jsNative and set(v: Func<unit, Promise<obj>>): unit = jsNative
                static member getJavaScriptWorker with get(): Func<unit, Promise<obj>> = jsNative and set(v: Func<unit, Promise<obj>>): unit = jsNative



        module css =
            type [<AllowNullLiteral>] DiagnosticsOptions =
                abstract validate: bool option with get, set
                abstract lint: obj option with get, set

            and [<AllowNullLiteral>] LanguageServiceDefaults =
                abstract onDidChange: IEvent<LanguageServiceDefaults> with get, set
                abstract diagnosticsOptions: DiagnosticsOptions with get, set
                abstract setDiagnosticsOptions: options: DiagnosticsOptions -> unit

            type [<Import("languages.css","monaco")>] Globals =
                static member cssDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative
                static member lessDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative
                static member scssDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative



        module json =
            type [<AllowNullLiteral>] DiagnosticsOptions =
                abstract validate: bool option with get, set
                abstract allowComments: bool option with get, set
                abstract schemas: ResizeArray<obj> option with get, set

            and [<AllowNullLiteral>] LanguageServiceDefaults =
                abstract onDidChange: IEvent<LanguageServiceDefaults> with get, set
                abstract diagnosticsOptions: DiagnosticsOptions with get, set
                abstract setDiagnosticsOptions: options: DiagnosticsOptions -> unit

            type [<Import("languages.json","monaco")>] Globals =
                static member jsonDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative



        module html =
            type [<AllowNullLiteral>] HTMLFormatConfiguration =
                abstract tabSize: float with get, set
                abstract insertSpaces: bool with get, set
                abstract wrapLineLength: float with get, set
                abstract unformatted: string with get, set
                abstract contentUnformatted: string with get, set
                abstract indentInnerHtml: bool with get, set
                abstract preserveNewLines: bool with get, set
                abstract maxPreserveNewLines: float with get, set
                abstract indentHandlebars: bool with get, set
                abstract endWithNewline: bool with get, set
                abstract extraLiners: string with get, set
                abstract wrapAttributes: U4<obj, obj, obj, obj> with get, set

            and [<AllowNullLiteral>] CompletionConfiguration =
                [<Emit("$0[$1]{{=$2}}")>] abstract Item: provider: string -> bool with get, set

            and [<AllowNullLiteral>] Options =
                abstract format: HTMLFormatConfiguration option with get, set
                abstract suggest: CompletionConfiguration option with get, set

            and [<AllowNullLiteral>] LanguageServiceDefaults =
                abstract onDidChange: IEvent<LanguageServiceDefaults> with get, set
                abstract options: Options with get, set
                abstract setOptions: options: Options -> unit

            type [<Import("languages.html","monaco")>] Globals =
                static member htmlDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative
                static member handlebarDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative
                static member razorDefaults with get(): LanguageServiceDefaults = jsNative and set(v: LanguageServiceDefaults): unit = jsNative



    module worker =
        type [<AllowNullLiteral>] IMirrorModel =
            abstract uri: Uri with get, set
            abstract version: float with get, set
            abstract getValue: unit -> string

        and [<AllowNullLiteral>] IWorkerContext =
            abstract getMirrorModels: unit -> ResizeArray<IMirrorModel>


