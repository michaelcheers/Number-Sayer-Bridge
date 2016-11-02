Bridge.assembly("Number Sayer Bridge", function ($asm, globals) {
    "use strict";

    Bridge.define("Number_Sayer_Bridge.HTML", {
        statics: {
            sayers: null,
            config: {
                init: function () {
                    this.sayers = new (System.Collections.Generic.Dictionary$2(String,NumberSayer))();
                    Bridge.ready(this.start);
                }
            },
            NumberSayer: function () {
                var $t, $t1;
                var key = System.String.concat(document.getElementById("voice").value, ($t=document.getElementById("language").selectedIndex, System.Enum.toString(NumberSayer.Language, $t)));

                return Number_Sayer_Bridge.HTML.sayers.containsKey(key) ? Number_Sayer_Bridge.HTML.sayers.get(key) : (($t1 = new NumberSayer(document.getElementById("language").selectedIndex, document.getElementById("voice").value), Number_Sayer_Bridge.HTML.sayers.set(key, $t1), $t1));
            },
            start: function () {
                var $t;
                document.getElementById("number").onkeydown = $_.Number_Sayer_Bridge.HTML.f1;
                [document.getElementById("from"), document.getElementById("to")].forEach($_.Number_Sayer_Bridge.HTML.f3);

                document.getElementById("language").onchange = $_.Number_Sayer_Bridge.HTML.f4;

                document.getElementById("submit").onclick = Number_Sayer_Bridge.HTML.submit;
                document.getElementById("count").onclick = Number_Sayer_Bridge.HTML.count;

                document.getElementById("language").innerHTML = "";
                $t = Bridge.getEnumerator(System.Enum.getValues(NumberSayer.Language));
                while ($t.moveNext()) {
                    var item = Bridge.cast($t.getCurrent(), NumberSayer.Language);
                    document.getElementById("language").appendChild(Bridge.merge(document.createElement('option'), {
                        value: System.Enum.toString(NumberSayer.Language, item),
                        innerHTML: System.String.replaceAll(System.Enum.toString(NumberSayer.Language, item), String.fromCharCode(95), String.fromCharCode(32))
                    } ));
                }
                document.getElementById("language").selectedIndex = 0;

                Number_Sayer_Bridge.HTML.update();
            },
            count: function (arg) {
                var number = document.getElementById("number");
                var to = bigInt(document.getElementById("to").value, 10);
                for (var n = bigInt(document.getElementById("from").value, 10); n.leq(to); n = n.add(1)) {
                    number.value = n.toString();
                    Number_Sayer_Bridge.HTML.submit(null);
                }
            },
            submit: function (arg) {
                var sound = Number_Sayer_Bridge.HTML.NumberSayer().say(Number_Sayer_Bridge.BigDecimal.parse(document.getElementById("number").value));
                document.getElementById("said").innerHTML = "";
                var bumped = 0;
                for (var n = 0; n < sound.sound.length; n = (n + 1) | 0) {
                    var name = sound.sound[n].name;
                    switch (name) {
                        case "es": 
                        case "ty": 
                        case "teen": 
                            break;
                        default: 
                            if (document.getElementById("language").selectedIndex !== NumberSayer.Language.Roman_Numerals || !System.String.startsWith(name, "d_")) {
                                document.getElementById("said").appendChild(Bridge.merge(document.createElement('span'), {
                                    innerHTML: " "
                                } ));
                            }
                            break;
                    }
                    var bump = false;
                    if (document.getElementById("language").selectedIndex === NumberSayer.Language.Roman_Numerals) {
                        if (!(Bridge.is(sound.sound[n], Number_Sayer_Bridge.RomanNumeralsAudio))) {
                            bump = true;
                        } else {
                            name = System.String.replaceAll(System.String.replaceAll(System.String.replaceAll(System.String.replaceAll(System.String.replaceAll(System.String.replaceAll(System.String.replaceAll(name, "d_0_0", "I"), "d_0_1", "V"), "d_1_0", "X"), "d_1_1", "L"), "d_2_0", "C"), "d_2_1", "D"), "d_3_0", "M");
                        }
                    }
                    if (bump) {
                        bumped = (bumped + 1) | 0;
                    } else {
                        var span = Bridge.merge(document.createElement('span'), {
                            innerHTML: name
                        } );
                        if (Bridge.is(sound.sound[n], Number_Sayer_Bridge.RomanNumeralsAudio)) {
                            for (var idx = 0; idx < sound.sound[n].lineNumbers; idx = (idx + 1) | 0) {
                                var oldSpan = span;
                                span = document.createElement('span');
                                span.style.borderTop = "1px solid black";
                                span.style.marginTop = "1px";
                                span.style.display = "inline-block";
                                span.appendChild(oldSpan);
                            }
                        }
                        span.id = "s" + (((n - bumped) | 0));
                        document.getElementById("said").appendChild(span);
                    }
                }
                var indexBump = 0;
                sound.play$1(function (index) {
                    if (Bridge.is(sound.sound[index], Number_Sayer_Bridge.RomanNumeralsAudio) || document.getElementById("language").selectedIndex !== NumberSayer.Language.Roman_Numerals) {
                        document.getElementById("s" + (((index - indexBump) | 0))).style.color = "red";
                    } else {
                        indexBump = (indexBump + 1) | 0;
                    }
                });
            },
            update: function () {
                var $t;
                document.getElementById("voice").innerHTML = "";
                var currentKnownVoices = NumberSayer.knownVoices.get(document.getElementById("language").selectedIndex);

                $t = Bridge.getEnumerator(currentKnownVoices);
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    document.getElementById("voice").appendChild(Bridge.merge(document.createElement('option'), {
                        value: item.toString(),
                        innerHTML: item.toString()
                    } ));
                }

                document.getElementById("voice").appendChild(Bridge.merge(document.createElement('option'), {
                    value: "mixed",
                    innerHTML: "Mixed"
                } ));
            }
        },
        $entryPoint: true
    });

    var $_ = {};

    Bridge.ns("Number_Sayer_Bridge.HTML", $_);

    Bridge.apply($_.Number_Sayer_Bridge.HTML, {
        f1: function (ev) {
            if (Bridge.is(ev, KeyboardEvent) && ev.keyCode === 13) {
                Number_Sayer_Bridge.HTML.submit(null);
            }
        },
        f2: function (ev) {
            if (Bridge.is(ev, KeyboardEvent) && ev.keyCode === 13) {
                Number_Sayer_Bridge.HTML.count(null);
            }
        },
        f3: function (item) {
            item.onkeydown = $_.Number_Sayer_Bridge.HTML.f2;
        },
        f4: function (e) {
            Number_Sayer_Bridge.HTML.update();
        }
    });
});
