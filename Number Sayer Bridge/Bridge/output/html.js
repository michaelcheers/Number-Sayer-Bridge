(function (globals) {
    "use strict";

    Bridge.define('Number_Sayer_Bridge.HTML', {
        statics: {
            sayers: null,
            config: {
                init: function () {
                    this.sayers = new System.Collections.Generic.Dictionary$2(String,NumberSayer)();
                    Bridge.ready(this.start);
                }
            },
            getNumberSayer: function () {
                var $t;
                var key = document.getElementById("voice").value + System.Enum.toString(NumberSayer.Language, document.getElementById("language").selectedIndex);
    
                return Number_Sayer_Bridge.HTML.sayers.containsKey(key) ? Number_Sayer_Bridge.HTML.sayers.get(key) : (($t = new NumberSayer(document.getElementById("language").selectedIndex, document.getElementById("voice").value), Number_Sayer_Bridge.HTML.sayers.set(key, $t), $t));
            },
            start: function () {
                var $t;
                document.getElementById("number").onkeydown = $_.Number_Sayer_Bridge.HTML.f1;
                Bridge.Linq.Enumerable.from([document.getElementById("from"), document.getElementById("to")]).forEach($_.Number_Sayer_Bridge.HTML.f3);
    
                document.getElementById("language").onchange = $_.Number_Sayer_Bridge.HTML.f4;
    
                document.getElementById("submit").onclick = Number_Sayer_Bridge.HTML.submit;
                document.getElementById("count").onclick = Number_Sayer_Bridge.HTML.count;
    
                document.getElementById("language").innerHTML = "";
                $t = Bridge.getEnumerator(System.Enum.getValues(NumberSayer.Language));
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    document.getElementById("language").appendChild(Bridge.merge(document.createElement('option'), {
                        value: System.Enum.toString(NumberSayer.Language, item),
                        innerHTML: System.Enum.toString(NumberSayer.Language, item)
                    } ));
                }
                document.getElementById("language").selectedIndex = 0;
    
                Number_Sayer_Bridge.HTML.update();
            },
            count: function (arg) {
                var to = bigInt(document.getElementById("to").value, 10);
                var sound = new Number_Sayer_Bridge.Sound("constructor");
                for (var n = bigInt(document.getElementById("from").value, 10); n.lesserOrEquals(to); n = n.add(bigInt(1))) {
                    sound.appendThis(Number_Sayer_Bridge.HTML.getNumberSayer().say$1(n));
                }
                sound.play();
            },
            submit: function (arg) {
                var sound = Number_Sayer_Bridge.HTML.getNumberSayer().say(Number_Sayer_Bridge.BigDecimal.parse(document.getElementById("number").value));
                document.getElementById("said").innerHTML = "";
                for (var n = 0; n < sound.sound.length; n = (n + 1) | 0) {
                    var name = sound.sound[n].name;
                    switch (name) {
                        case "es": 
                        case "ty": 
                        case "teen": 
                            break;
                        default: 
                            document.getElementById("said").appendChild(Bridge.merge(document.createElement('span'), {
                                innerHTML: " "
                            } ));
                            break;
                    }
                    document.getElementById("said").appendChild(Bridge.merge(document.createElement('span'), {
                        id: "s" + n,
                        innerHTML: name
                    } ));
                }
                sound.play$1($_.Number_Sayer_Bridge.HTML.f5);
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
        },
        f5: function (index) {
            document.getElementById("s" + index).style.color = "red";
        }
    });
    
    
    
    Bridge.init();
})(this);
