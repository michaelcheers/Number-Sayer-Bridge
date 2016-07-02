(function (globals) {
    "use strict";

    Bridge.define('Number_Sayer_Bridge.HTML', {
        statics: {
            sayers: null,
            config: {
                init: function () {
                    this.sayers = new System.Collections.Generic.Dictionary$2(String,Number_Sayer_Bridge.NumberSayer)();
                    Bridge.ready(this.start);
                }
            },
            start: function () {
                var $t;
                document.getElementById("number").onkeydown = $_.Number_Sayer_Bridge.HTML.f1;
    
                document.getElementById("language").onchange = $_.Number_Sayer_Bridge.HTML.f2;
    
                document.getElementById("submit").onclick = Number_Sayer_Bridge.HTML.submit;
    
                document.getElementById("language").innerHTML = "";
                $t = Bridge.getEnumerator(System.Enum.getValues(Number_Sayer_Bridge.NumberSayer.Language));
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    document.getElementById("language").appendChild(Bridge.merge(document.createElement('option'), {
                        value: System.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, item),
                        innerHTML: System.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, item)
                    } ));
                }
                document.getElementById("language").selectedIndex = 0;
    
                Number_Sayer_Bridge.HTML.update();
            },
            submit: function (arg) {
                var $t;
                var key = document.getElementById("voice").value + System.Enum.toString(Number_Sayer_Bridge.NumberSayer.Language, document.getElementById("language").selectedIndex);
                var sayer;
    
                sayer = Number_Sayer_Bridge.HTML.sayers.containsKey(key) ? Number_Sayer_Bridge.HTML.sayers.get(key) : (($t = new Number_Sayer_Bridge.NumberSayer(document.getElementById("language").selectedIndex, document.getElementById("voice").value), Number_Sayer_Bridge.HTML.sayers.set(key, $t), $t));
    
                var sound = sayer.say(bigInt(document.getElementById("number").value, 10));
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
                sound.play$1($_.Number_Sayer_Bridge.HTML.f3);
            },
            update: function () {
                var $t;
                document.getElementById("voice").innerHTML = "";
                var currentKnownVoices = Number_Sayer_Bridge.NumberSayer.knownVoices.get(document.getElementById("language").selectedIndex);
    
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
            Number_Sayer_Bridge.HTML.update();
        },
        f3: function (index) {
            document.getElementById("s" + index).style.color = "red";
        }
    });
    
    
    
    Bridge.init();
})(this);
